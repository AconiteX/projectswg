package engine.resources.scene;



public class Transform {
	
	protected Quaternion orientation;
	protected Point3D position;
	
	public Transform(float orient_w, float orient_x, float orient_y, float orient_z, float x, float y, float z) {
		
		orientation = new Quaternion(orient_w, orient_x, orient_y, orient_z);
		position = new Point3D(x, y, z);
	
	}
	public Transform(Quaternion orientation, Point3D position) {
		
		this.orientation = orientation.clone();
		this.position = position.clone();
		
	}

	public Quaternion getOrientation() { return orientation.clone(); }
	public Point3D getPosition() { return position.clone(); }
	
	public float getDistance(Transform target) {
		
		return position.getDistance(target.getPosition());
		
	}
	
}
