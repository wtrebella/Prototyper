using UnityEngine;
using System.Collections;

public static class Utils {
	public static bool AABBvsAABB( AABB a, AABB b )
	{
		// Exit with no intersection if found separated along an axis
		if (a.max.x < b.min.x || a.min.x > b.max.x) return false;
		if (a.max.y < b.min.y || a.min.y > b.max.y) return false;

		// No separating axis found, therefor there is at least one overlapping axis
		return true;
	}

//	public static bool CirclevsCircle(Circle a, Circle b)
//	{
//		float r = a.radius + b.radius;
//		r *= r;
//		return r < Mathf.Pow(a.position.x + b.position.x, 2) + Mathf.Pow(a.position.y + b.position.y, 2);
//	}

//	bool CirclevsCircle(Manifold m) {
//		// Setup a couple pointers to each object
//		PhysicsObject A = m.A;
//		PhysicsObject B = m.B;
//
//		// Vector from A to B
//		Vector2 n = B.GetPosition() - A.GetPosition();
//
//		float r = A.circle.radius + B.circle.radius;
//		r *= r;
//
//		if(n.sqrMagnitude > r) return false;
//
//		// Circles have collided, now compute manifold
//		float d = n.magnitude; // perform actual sqrt
//
//		// If distance between circles is not zero
//		if(d != 0) {
//			// Distance is difference between radius and distance
//			m.penetration = r - d
//
//			// Utilize our d since we performed sqrt on it already within Length( )
//			// Points from A to B, and is a unit vector
//			c->normal = t / d;
//			return true
//		}
//
//		// Circles are on same position
//		else
//		{
//			// Choose random (but consistent) values
//			c->penetration = A->radius
//				c->normal = Vec( 1, 0 )
//					return true
//		}
//	}

	public static void ResolveCollision(PhysicsObject A, PhysicsObject B, Vector2 normal) {
		// Calculate relative velocity
		Vector2 rv = B.velocity - A.velocity;

		// Calculate relative velocity in terms of the normal direction
		float velAlongNormal = DotProduct(rv, normal);

		// Do not resolve if velocities are separating
		if (velAlongNormal > 0) return;

		// Calculate restitution
		float e = Mathf.Min(A.restitution, B.restitution);

		// Calculate impulse scalar
		float j = -(1 + e) * velAlongNormal;
		j /= A.invMass + B.invMass;

		// Apply impulse
		Vector2 impulse = j * normal;
		A.velocity -= A.invMass * impulse;
		B.velocity += B.invMass * impulse;
	}

	public static float DotProduct(Vector2 a, Vector2 b) {
		return a.x * b.x + a.y * b.y;
	}

//	void PositionalCorrection(PhysicsObject A, PhysicsObject B, float penetration) {
//		const float percent = 0.2f; // usually 20% to 80%
//		const float slop = 0.01f; // usually 0.01 to 0.1
//		Vector2 correction = Mathf.Max(penetration - slop, 0.0f) / (A.invMass + B.invMass) * percent * n;
//		Vector2 aPos = A.GetPosition();
//		Vector2 bPos = B.GetPosition();
//		aPos -= A.invMass * correction;
//		bPos += B.invMass * correction;
//		A.SetPosition(aPos);
//		B.SetPosition(bPos);
//	}
}
