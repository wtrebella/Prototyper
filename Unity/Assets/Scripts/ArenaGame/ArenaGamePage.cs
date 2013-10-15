using UnityEngine;
using System.Collections;

public class ArenaGamePage : AbstractPage {
	//public static int layerSolidTile = 1;
	//public static int layer
	int tickCounter = 0;
	public static Vector2 gravity = new Vector2(0, -10);
	FContainer boardContainer = new FContainer();
	int boardWidth;
	int boardHeight;
	int tileSize;
	int[][] boardData;
	WTWall[][] boardTiles;
	ArenaMan guy;
	bool gameHasStarted = false;

	public int[][] _levelData = {
		new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		new int[] {1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1},
		new int[] {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
		new int[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1},
		new int[] {1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
		new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
	};


	public ArenaGamePage() {
		FSprite background = new FSprite("dirtTile");
		background.width = background.height = Futile.screen.width * 2;
		background.SetPosition(Futile.screen.halfWidth, Futile.screen.halfHeight);
		background.alpha = 0.5f;
		Go.to(background, 2, new TweenConfig().setIterations(-1).floatProp("rotation", 360));
		AddChild(background);

		LoadLevel(_levelData);

		guy = new ArenaMan("arenaMan", tileSize * 0.8f, tileSize * 0.8f * 2);
		guy.SetPosition(GetLocalPointForIndex(boardWidth-2, boardHeight-3));
		boardContainer.AddChild(guy);
	}

	override public void Start() {
		ListenForUpdate(HandleUpdate);
	}

	public void LoadLevel(int[][] levelData) {
		boardWidth = _levelData[0].Length;
		boardHeight = _levelData.Length;

		boardData = new int[boardWidth][];
		for (int i = 0; i < boardWidth; i++) {
			boardData[i] = new int[boardHeight];

			for (int j = 0; j < boardHeight; j++) {
				boardData[i][j] = levelData[boardHeight - j - 1][i];
			}
		}

		boardTiles = new WTWall[boardWidth][];
		for (int i = 0; i < boardWidth; i++) {
			boardTiles[i] = new WTWall[boardHeight];
		}

		float maxTileSizeHorizontal = (int)(Futile.screen.width / (float)boardWidth);
		float maxTileSizeVertical = (int)(Futile.screen.height / (float)boardHeight);

		tileSize = (int)Mathf.Min(maxTileSizeVertical, maxTileSizeHorizontal);

		for (int i = 0; i < boardWidth; i++) {
			for (int j = 0; j < boardHeight; j++) {
				if (boardData[i][j] == 0) continue;

				WTWall s = new WTWall(tileSize, tileSize);
				Vector2 pos = GetLocalPointForIndex(i, j);
				s.SetPosition(pos.x + 0.5f * tileSize, pos.y + 0.5f * tileSize);
				s.sprite.color = new Color(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f));
				s.rotation = (float)RXRandom.Select(0f, 90f, 180f, 270f);
				boardContainer.AddChild(s);
				boardTiles[i][j] = s;
			}
		}

		boardContainer.x = Futile.screen.halfWidth - boardWidth * tileSize / 2f;
		boardContainer.y = Futile.screen.halfHeight - boardHeight * tileSize / 2f;
		AddChild(boardContainer);
	}

	Vector2 GetLocalPointForIndex(int x, int y) {
		//if (x < 0 || y < 0 || x >= boardWidth || y >= boardHeight) throw new FutileException("invalid index");

		return new Vector2(x * tileSize, y * tileSize);
	}

	IntPoint GetIndexForLocalPoint(Vector2 localPoint) {
		int xIndex = (int)(localPoint.x / tileSize);
		int yIndex = (int)(localPoint.y / tileSize);

		//if (x < 0 || y < 0 || x >= boardWidth || y >= boardHeight) throw new FutileException("invalid index");

		return new IntPoint(xIndex, yIndex);
	}

	IntPoint GetIndexForGlobalPoint(Vector2 globalPoint) {
		return GetIndexForLocalPoint(boardContainer.GlobalToLocal(globalPoint));
	}

	public void HandleUpdate() {
		if (!gameHasStarted) {
			if (Input.GetMouseButtonDown(0)) {
				guy.physicsComponent.StartPhysics();
				gameHasStarted = true;
			}
			else return;
		}

		guy.HandleUpdate();
	}
}
