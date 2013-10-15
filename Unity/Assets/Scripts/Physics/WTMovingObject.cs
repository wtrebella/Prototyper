using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTMovingObject : WTPhysicsNode {
	public Vector2 velocity = Vector2.zero;
	public Vector2 drag = Vector2.zero;
	public float friction = WTConfig.frictionConstant;
	public FSprite sprite;
	public bool isConstantlyMoving = false;
	public bool shouldBounce = true;

	protected bool isOnGround_ = false;

	public WTMovingObject(FSprite sprite, string name, float colliderWidth, float colliderHeight) : base(name) {
		this.sprite = sprite;
		AddChild(this.sprite);

		physicsComponent.AddRigidBody(0f, 0f, 10f);
		physicsComponent.rigidbody.tag = "Solid";
		physicsComponent.rigidbody.freezeRotation = true;
		physicsComponent.AddBoxCollider(colliderWidth, colliderHeight);
		physicsComponent.SetupPhysicMaterial(0.0f, 0.0f, 0.0f, PhysicMaterialCombine.Minimum);

		physicsComponent.gameObject.layer = LayerMask.NameToLayer("Object");

		drag = WTConfig.objectDrag;
	}

	virtual public bool isOnGround {
		get {return isOnGround_;}
		set {
			isOnGround_ = value;
		}
	}

	virtual public void UpdateMovement() {
		int layerMask = 1 << LayerMask.NameToLayer("Environment");

		WTPhysicsRay lLowRay = new WTPhysicsRay(this, new Vector2(0, 0), Vector3.left);
		WTPhysicsRay lHighRay = new WTPhysicsRay(this, new Vector2(0, 1), Vector3.left);
		WTPhysicsRay rLowRay = new WTPhysicsRay(this, new Vector2(1, 0), Vector3.right);
		WTPhysicsRay rHighRay = new WTPhysicsRay(this, new Vector2(1, 1), Vector3.right);

		WTPhysicsRaycastHit lLowHit;
		WTPhysicsRaycastHit lHighHit;
		WTPhysicsRaycastHit rLowHit;
		WTPhysicsRaycastHit rHighHit;

		// add in an intermediary hit variable here. it will need to split up the collider into smaller or equal to the universal block size
		// chunks so that the rays will always hit a block. is this stupid?

		float xRayDistance = Mathf.Abs(velocity.x * Time.deltaTime);

		// leftwards
		if (velocity.x < 0) {
			if (WTPhysicsRay.Raycast(lLowRay, out lLowHit, xRayDistance, layerMask)) {
				if (lLowHit.GetPhysicsNode().CompareTag("Solid")) {
					if (shouldBounce && Mathf.Abs(velocity.x * Time.deltaTime) > WTConfig.minBounceDist) {
						velocity.x *= -WTConfig.bounceConstant;
					}
					else velocity.x = 0;

					this.x = lLowHit.GetPoint().x + physicsComponent.GetGlobalHitBox().width / 2f + 0.01f;
				}
			}

			else if (WTPhysicsRay.Raycast(lHighRay, out lHighHit, xRayDistance, layerMask)) {
				if (lHighHit.GetPhysicsNode().CompareTag("Solid")) {
					if (shouldBounce && Mathf.Abs(velocity.x * Time.deltaTime) > WTConfig.minBounceDist) {
						velocity.x *= -WTConfig.bounceConstant;
					}
					else velocity.x = 0;

					this.x = lHighHit.GetPoint().x + physicsComponent.GetGlobalHitBox().width / 2f + 0.01f;
				}
			}
		}

		// rightwards
		else if (velocity.x > 0) {
			if (WTPhysicsRay.Raycast(rLowRay, out rLowHit, xRayDistance, layerMask)) {
				if (rLowHit.GetPhysicsNode().CompareTag("Solid")) {
					if (shouldBounce && Mathf.Abs(velocity.x * Time.deltaTime) > WTConfig.minBounceDist) {
						velocity.x *= -WTConfig.bounceConstant;
					}
					else velocity.x = 0;

					this.x = rLowHit.GetPoint().x - physicsComponent.GetGlobalHitBox().width / 2f - 0.01f;
				}
			}

			else if (WTPhysicsRay.Raycast(rHighRay, out rHighHit, xRayDistance, layerMask)) {
				if (rHighHit.GetPhysicsNode().CompareTag("Solid")) {
					if (shouldBounce && Mathf.Abs(velocity.x * Time.deltaTime) > WTConfig.minBounceDist) {
						velocity.x *= -WTConfig.bounceConstant;
					}
					else velocity.x = 0;

					this.x = rHighHit.GetPoint().x - physicsComponent.GetGlobalHitBox().width / 2f - 0.01f;
				}
			}
		}

		if (!isConstantlyMoving) {
			float frictionMultiplier = this.isOnGround?WTConfig.frictionConstant:1;
			float dragAmt = drag.x * frictionMultiplier * Time.deltaTime;
			if (velocity.x - dragAmt > 0) velocity.x -= dragAmt;
			else if (velocity.x + dragAmt < 0) velocity.x += dragAmt;
			else velocity.x = 0;
		}

		this.x += velocity.x * Time.deltaTime;

		WTPhysicsRay lCeilingRay = new WTPhysicsRay(this, new Vector2(0, 1), Vector3.up);
		WTPhysicsRay rCeilingRay = new WTPhysicsRay(this, new Vector2(1, 1), Vector3.up);
		WTPhysicsRay lFloorRay = new WTPhysicsRay(this, new Vector2(0, 0), Vector3.down);
		WTPhysicsRay rFloorRay = new WTPhysicsRay(this, new Vector2(1, 0), Vector3.down);

		WTPhysicsRaycastHit lFloorHit;
		WTPhysicsRaycastHit rFloorHit;
		WTPhysicsRaycastHit lCeilingHit;
		WTPhysicsRaycastHit rCeilingHit;

		velocity.y += WTConfig.gravity * Time.deltaTime;

		float yRayDistance = Mathf.Abs(velocity.y * Time.deltaTime);

		// downwards
		if (velocity.y < 0) {
			if (WTPhysicsRay.Raycast(lFloorRay, out lFloorHit, yRayDistance, layerMask)) {
				if (lFloorHit.GetPhysicsNode().CompareTag("Solid")) {
					if (velocity.y <= 0) {
						if (shouldBounce && Mathf.Abs(velocity.y * Time.deltaTime) > WTConfig.minBounceDist) {
							velocity.y *= -WTConfig.bounceConstant;
						}
						else {
							velocity.y = 0;
							this.isOnGround = true;
						}
					}
					this.y = lFloorHit.GetPoint().y + physicsComponent.GetGlobalHitBox().height / 2f + 0.01f;
				}
			}

			else if (WTPhysicsRay.Raycast(rFloorRay, out rFloorHit, yRayDistance, layerMask)) {
				if (rFloorHit.GetPhysicsNode().CompareTag("Solid")) {
					if (velocity.y <= 0) {
						if (shouldBounce && Mathf.Abs(velocity.y * Time.deltaTime) > WTConfig.minBounceDist) {
							velocity.y *= -WTConfig.bounceConstant;
						}
						else {
							velocity.y = 0;
							this.isOnGround = true;
						}
					}
					this.y = rFloorHit.GetPoint().y + physicsComponent.GetGlobalHitBox().height / 2f + 0.01f;
				}
			}
			else this.isOnGround = false;
		}

		// upwards
		else if (velocity.y > 0) {
			if (WTPhysicsRay.Raycast(lCeilingRay, out lCeilingHit, yRayDistance, layerMask)) {
				if (lCeilingHit.GetPhysicsNode().CompareTag("Solid")) {
					this.y = lCeilingHit.GetPoint().y - physicsComponent.GetGlobalHitBox().height / 2f - 0.01f;
					if (shouldBounce) velocity.y *= -WTConfig.bounceConstant;
					else velocity.y = 0;
				}
			}

			else if (WTPhysicsRay.Raycast(rCeilingRay, out rCeilingHit, yRayDistance, layerMask)) {
				if (rCeilingHit.GetPhysicsNode().CompareTag("Solid")) {
					this.y = rCeilingHit.GetPoint().y - physicsComponent.GetGlobalHitBox().height / 2f - 0.01f;
					if (shouldBounce) velocity.y *= -WTConfig.bounceConstant;
					else velocity.y = 0;
				}
			}
		}

		if (velocity.y > WTConfig.maxVelocity.y) velocity.y = WTConfig.maxVelocity.y;
		else if (velocity.y < -WTConfig.maxVelocity.y) velocity.y = -WTConfig.maxVelocity.y;

		this.y += velocity.y * Time.deltaTime;
	}
	
	override public void HandleUpdate() {
		base.HandleUpdate();

		UpdateMovement();
	}
}
