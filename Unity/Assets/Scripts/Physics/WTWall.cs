using UnityEngine;
using System.Collections;

// just a stupid simple rigid wall for things to bounce off of

public class WTWall : WTPhysicsNode {
	public FSprite sprite;

	public WTWall(float width, float height) : base("wall") {
		sprite = new FSprite("WhiteBox");
		sprite.width = width;
		sprite.height = height;
		sprite.color = new Color(0.1f, 0.1f, 0.1f);
		AddChild(sprite);

		physicsComponent.gameObject.isStatic = true;
		physicsComponent.gameObject.tag = "Solid";
		physicsComponent.AddRigidBody(0f, 0.1f, 10f);
		physicsComponent.AddBoxCollider(width, height);
		physicsComponent.SetupPhysicMaterial(0.0f, 1f, 1f, PhysicMaterialCombine.Minimum);
		physicsComponent.gameObject.layer = LayerMask.NameToLayer("Environment");
	}
}
