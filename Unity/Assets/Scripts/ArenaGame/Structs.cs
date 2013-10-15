using UnityEngine;

public struct IntPoint {
	public int x;
	public int y;

	public IntPoint(int x, int y) {
		this.x = x;
		this.y = y;
	}
}

public struct Circle {
	public float radius;
	public Vector2 position;

	public Circle(float radius, Vector2 position) {
		this.radius = radius;
		this.position = position;
	}
}

public struct AABB {
	public Vector2 min;
	public Vector2 max;

	public AABB(Vector2 min, Vector2 max) {
		this.min = min;
		this.max = max;
	}
}

public struct Manifold {
	public PhysicsObject A;
	public PhysicsObject B;
	float penetration;
	Vector2 normal;

	public Manifold(PhysicsObject A, PhysicsObject B, float penetration, Vector2 normal) {
		this.A = A;
		this.B = B;
		this.penetration = penetration;
		this.normal = normal;
	}
}