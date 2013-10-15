using UnityEngine;
using System.Collections;

//public class ArenaMan : WTMovingObject {
//	public ArenaMan(FSprite sprite, string name, float colliderWidth, float colliderHeight) : base(sprite, name, colliderWidth, colliderHeight) {
//		shouldBounce = false;
//
//		sprite.color = Color.blue;
//		sprite.width = colliderWidth;
//		sprite.height = colliderHeight;
//	}
//
//	override public void HandleUpdate() {
//		base.HandleUpdate();
//
//		if (Input.GetKeyDown(KeyCode.Space)) {
//			if (isOnGround) velocity.y = 400;
//		}
//
//		if (Input.GetKey(KeyCode.LeftArrow)) {
//			isConstantlyMoving = true;
//			velocity.x = -200;
//		}
//		if (Input.GetKey(KeyCode.RightArrow)) {
//			isConstantlyMoving = true;
//			velocity.x = 200;
//		}
//		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
//			isConstantlyMoving = false;
//		}
//	}
//}

public class ArenaMan : WTPhysicsNode {
	FSprite sprite;
	float maxFallingVelocity = 6;

	public ArenaMan(string name, float colliderWidth, float colliderHeight) : base(name) {
		sprite = new FSprite("WhiteBox");
		sprite.color = Color.blue;
		sprite.width = colliderWidth;
		sprite.height = colliderHeight;
		AddChild(sprite);

		physicsComponent.AddRigidBody(0f, 0f, 0.1f);
		physicsComponent.rigidbody.tag = "Solid";
		physicsComponent.rigidbody.freezeRotation = true;
		physicsComponent.AddBoxCollider(colliderWidth, colliderHeight);
		physicsComponent.SetupPhysicMaterial(0.0f, 0.01f, 0.01f, PhysicMaterialCombine.Minimum);
	}

	override public void HandleUpdate() {
		base.HandleUpdate();

		Vector2 v = new Vector2(physicsComponent.rigidbody.velocity.x, physicsComponent.rigidbody.velocity.y);

		if (v.x < 0) {
			v.x = Mathf.Min(0, v.x + WTConfig.playerDrag.x * Time.deltaTime);
		}
		else if (v.x > 0) {
			v.x = Mathf.Max(0, v.x - WTConfig.playerDrag.x * Time.deltaTime);
		}

		physicsComponent.rigidbody.velocity = v;

		if (Input.GetKeyDown(KeyCode.Space)) {
			physicsComponent.rigidbody.velocity = new Vector2(physicsComponent.rigidbody.velocity.x, 7);
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			physicsComponent.rigidbody.velocity = new Vector2(-5, physicsComponent.rigidbody.velocity.y);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			physicsComponent.rigidbody.velocity = new Vector2(5, physicsComponent.rigidbody.velocity.y);
		}

//		if (physicsComponent.rigidbody.velocity.y < -maxFallingVelocity) {
//			physicsComponent.rigidbody.velocity = new Vector2(physicsComponent.rigidbody.velocity.x, -maxFallingVelocity);
//		}
	}
}

