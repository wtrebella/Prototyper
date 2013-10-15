using UnityEngine;
using System.Collections;

public class WindGamePage : AbstractPage, FMultiTouchableInterface {
	Guy guy;

	public WindGamePage () {
		guy = new Guy();
		guy.SetPosition(Futile.screen.halfWidth, 50);
		AddChild(guy);
	}

	override public void Start() {	
		EnableMultiTouch();
		ListenForUpdate(HandleUpdate);
	}

	override public void Destroy() {	

	}

	public void HandleUpdate() {

	}
	
	public void HandleMultiTouch(FTouch[] touches) {
		foreach (FTouch touch in touches) {
			if (touch.phase == TouchPhase.Began) {
			
			}
		}
	}
}




















