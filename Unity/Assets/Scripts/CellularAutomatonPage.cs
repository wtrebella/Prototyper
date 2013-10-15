using UnityEngine;
using System.Collections;

public enum CellType {
	Empty,
	Solid
}



public class CellularAutomatonPage : AbstractPage, FMultiTouchableInterface {
	CellType[][] cellMap;
	FSprite[][] mapSprites;
	FSprite guy;
	FContainer levelContainer;
	Vector2 guyVelocity = Vector2.zero;
	Vector2 gravityVector = new Vector2(0, -15);
	//float nextUpdateTime = 0;
	//float updateTimeStep = 0.2f;
	int numSteps = 10;
	bool mapIsDirty = false;

	const int cellMapWidth = 100;
	const int cellMapHeight = 100;
	const float tileSize = 3;
	const int deathLimit = 4;
	const int birthLimit = 4;
	const int overpopLimit = 8;
	const uint hexColorSolid = 0x443333;
	const uint hexColorEmpty = 0x3355aa;

	public CellularAutomatonPage () {
		CreateCellMap();
		for (int i = 0; i < numSteps; i++) cellMap = DoDataSimulationStep(cellMap);
		CreateMapSprites();

		guy = new FSprite("WhiteBox");
		guy.width = guy.height = tileSize;

		bool foundSpot = false;
		for (int j = cellMapHeight - 1; j >= 0; j--) {
			for (int i = 0; i < cellMapWidth; i++) {
				if (cellMap[i][j] == CellType.Empty) {
					guy.SetPosition(GetPositionForIndex(i, j));
					foundSpot = true;
					break;
				}
			}
			if (foundSpot) break;
		}

		levelContainer.AddChild(guy);
	}

	public Vector2 GetPositionForIndex(int xIndex, int yIndex) {
		return new Vector2((xIndex + 0.5f) * tileSize, (yIndex + 0.5f) * tileSize);
	}

	override public void Start() {	
		EnableMultiTouch();
		ListenForUpdate(HandleUpdate);
	}

	override public void Destroy() {	

	}

	public void HandleUpdate() {
//		if (Time.time > nextUpdateTime) {
//			nextUpdateTime = Time.time + updateTimeStep;
//			cellMap = DoDataSimulationStep(cellMap);
//		}

		guyVelocity += gravityVector;

		guy.x += Time.deltaTime * guyVelocity.x;
		guy.y += Time.deltaTime * guyVelocity.y;

		if (mapIsDirty) UpdateSprites();
	}

	private void CreateCellMap() {
		float chanceToStayAlive = 0.45f;

		cellMap = new CellType[cellMapWidth][];
		for (int i = 0; i < cellMapWidth; i++) {
			cellMap[i] = new CellType[cellMapHeight];

			for (int j = 0; j < cellMapHeight; j++) {
				if (RXRandom.Float() < chanceToStayAlive) cellMap[i][j] = CellType.Solid;
				else cellMap[i][j] = CellType.Empty;
			}
		}
	}

	private void CreateMapSprites() {
		levelContainer = new FContainer();
		levelContainer.SetPosition(Futile.screen.halfWidth - cellMapWidth / 2f * tileSize, Futile.screen.halfHeight - cellMapHeight / 2f * tileSize);
		AddChild(levelContainer);

		mapSprites = new FSprite[cellMapWidth][];
		for (int i = 0; i < cellMapWidth; i++) {
			mapSprites[i] = new FSprite[cellMapHeight];
		}

		for (int i = 0; i < cellMap.Length; i++) {
			for (int j = 0; j < cellMap[i].Length; j++) {
				FSprite wall = new FSprite("WhiteBox");
				wall.width = wall.height = tileSize;
				if (cellMap[i][j] == CellType.Empty) wall.color = RXColor.GetColorFromHex(hexColorEmpty);
				else if (cellMap[i][j] == CellType.Solid) wall.color = RXColor.GetColorFromHex(hexColorSolid);
				wall.SetPosition((i + 0.5f) * tileSize, (j + 0.5f) * tileSize);
				levelContainer.AddChild(wall);
				mapSprites[i][j] = wall;
			}
		}
	}

	private CellType[][] DoDataSimulationStep(CellType[][] oldMap) {
		CellType[][] newMap = new CellType[cellMapWidth][];

		for (int i = 0; i < cellMapWidth; i++) {
			newMap[i] = new CellType[cellMapHeight];

			for (int j = 0; j < cellMapHeight; j++) newMap[i][j] = CellType.Empty;
		}

		//Loop over each row and column of the map
		for (int xIndex = 0; xIndex < oldMap.Length; xIndex++){
			for (int yIndex = 0; yIndex < oldMap[0].Length; yIndex++){
				int nbs = CountSolidNeighbors(oldMap, xIndex, yIndex);

				//First, if a cell is alive but has too few neighbours, kill it.
				if(oldMap[xIndex][yIndex] == CellType.Solid) {
					if (nbs < deathLimit || nbs > overpopLimit) newMap[xIndex][yIndex] = CellType.Empty;
					else newMap[xIndex][yIndex] = CellType.Solid;
				}

				//Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
				else if (oldMap[xIndex][yIndex] == CellType.Empty) {
					if(nbs > birthLimit) newMap[xIndex][yIndex] = CellType.Solid;
					else newMap[xIndex][yIndex] = CellType.Empty;
				}
			}
		}

		mapIsDirty = true;

		return newMap;
	}

	public void HandleMultiTouch(FTouch[] touches) {
		foreach (FTouch touch in touches) {
			if (touch.phase == TouchPhase.Began) {
				//cellMap = DoDataSimulationStep(cellMap);
				//CreateCellMap();
				//for (int i = 0; i < numSteps; i++) cellMap = DoDataSimulationStep(cellMap);
			}
		}
	}

	private void UpdateSprites() {
		for (int i = 0; i < cellMap.Length; i++) {
			for (int j = 0; j < cellMap[i].Length; j++) {
				if (cellMap[i][j] == CellType.Empty) mapSprites[i][j].color = RXColor.GetColorFromHex(hexColorEmpty);
				else if (cellMap[i][j] == CellType.Solid) mapSprites[i][j].color = RXColor.GetColorFromHex(hexColorSolid);
			}
		}

		mapIsDirty = false;
	}

	private int CountSolidNeighbors(CellType[][] map, int xCellIndex, int yCellIndex) {
		int count = 0;
		for(int i = -1; i < 2; i++) {
			for(int j = -1; j < 2; j++) {
				int xNeighbor = xCellIndex + i;
				int yNeighbor = yCellIndex + j;

				if (!(i == 0 && j == 0)) {
					if (xNeighbor < 0 || yNeighbor < 0 || xNeighbor >= map.Length || yNeighbor >= map[0].Length) count++;
					else if(map[xNeighbor][yNeighbor] == CellType.Solid) count++;
				}
			}
		}

		return count;
	}
}




















