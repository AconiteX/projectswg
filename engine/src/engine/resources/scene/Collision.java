package engine.resources.scene;

import org.apache.commons.math3.geometry.euclidean.threed.Vector3D;

public class Collision {
	
	public static boolean GetIntersection(float fDst1, float fDst2, Vector3D P1, Vector3D P2, Vector3D Hitpoint) {
		if ( (fDst1 * fDst2) >= 0.0f) return false;
		if ( fDst1 == fDst2) return false; 
		
		Hitpoint = (P2.subtract(P1)).scalarMultiply(-fDst1/(fDst2-fDst1)).add(P1);
		return true;
	}

	public static boolean InBox(Vector3D Hit, Vector3D B1, Vector3D B2, int Axis) {
		if ( Axis==1 && Hit.getZ() > B1.getZ() && Hit.getZ() < B2.getZ() && Hit.getY() > B1.getY() && Hit.getY() < B2.getY()) return true;
		if ( Axis==2 && Hit.getZ() > B1.getZ() && Hit.getZ() < B2.getZ() && Hit.getX() > B1.getX() && Hit.getX() < B2.getX()) return true;
		if ( Axis==3 && Hit.getX() > B1.getX() && Hit.getX() < B2.getX() && Hit.getY() > B1.getY() && Hit.getY() < B2.getY()) return true;
		return false;
	}

	/**
	 * 
	 * @param B1 Point 1 (smallest) of bounding box.
	 * @param B2 Point 2 (largest) of bounding box.
	 * @param L1 Point 1 of Line.
	 * @param L2 Point 2 of Line.
	 * @param Hit The Point at which the line intersects with the box.
	 * @return
	 */
	public static boolean CheckCollisionWithBox(Vector3D B1, Vector3D B2, Vector3D L1, Vector3D L2, Vector3D Hitpoint) {
		if (L2.getX() < B1.getX() && L1.getX() < B1.getX()) return false;
		if (L2.getX() > B2.getX() && L1.getX() > B2.getX()) return false;
		if (L2.getY() < B1.getY() && L1.getY() < B1.getY()) return false;
		if (L2.getY() > B2.getY() && L1.getY() > B2.getY()) return false;
		if (L2.getZ() < B1.getZ() && L1.getZ() < B1.getZ()) return false;
		if (L2.getZ() > B2.getZ() && L1.getZ() > B2.getZ()) return false;
		if (L1.getX() > B1.getX() && L1.getX() < B2.getX() &&
		    L1.getY() > B1.getY() && L1.getY() < B2.getY() &&
		    L1.getZ() > B1.getZ() && L1.getZ() < B2.getZ()) { 
			Hitpoint = L1; 
		    return true;
		}
		if ( (GetIntersection( (float) (L1.getX()-B1.getX()), (float) (L2.getX()-B1.getX()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 1 ))
		  || (GetIntersection( (float) (L1.getY()-B1.getY()), (float) (L2.getY()-B1.getY()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 2 )) 
		  || (GetIntersection( (float) (L1.getZ()-B1.getZ()), (float) (L2.getZ()-B1.getZ()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 3 )) 
		  || (GetIntersection( (float) (L1.getX()-B2.getX()), (float) (L2.getX()-B2.getX()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 1 )) 
		  || (GetIntersection( (float) (L1.getY()-B2.getY()), (float) (L2.getY()-B2.getY()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 2 )) 
		  || (GetIntersection( (float) (L1.getZ()-B2.getZ()), (float) (L2.getZ()-B2.getZ()), L1, L2, Hitpoint) && InBox( Hitpoint, B1, B2, 3 )))
			return true;

		return false;
	}

}
