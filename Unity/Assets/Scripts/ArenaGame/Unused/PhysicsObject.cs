using UnityEngine;
using System.Collections;

public class PhysicsObject : FContainer {
	public Vector2 velocity = Vector2.zero;
	private float mass_;
	public float invMass;
	public float restitution = 0.2f;
	public Circle circle;
	public AABB aabb;

	public float mass {
		get {return mass_;}
		set {
			mass_ = value;
			if (mass_ == 0) invMass = 0;
			else invMass = 1 / mass_;
		}
	}

	public PhysicsObject() {
		this.mass = 10;
	}
}
