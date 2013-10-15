using UnityEngine;
using System.Collections;

public class Guy : FContainer {
	public FSprite sprite;

	public Guy() {
		sprite = new FSprite("circle");
		sprite.color = Color.red;
		sprite.scale = 0.2f;
		AddChild(sprite);
	}
}
