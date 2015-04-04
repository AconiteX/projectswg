package engine.resources.objects;

import engine.resources.scene.Planet;
import engine.resources.scene.Point3D;
import engine.resources.scene.Quaternion;

public interface ISWGObject {
	
	public long getObjectID();
	public Planet getPlanet();
	
	public SWGObject getContainer();
	
	public void setPosition(Point3D newPosition);
	public void setOrientation(Quaternion newOrientation);
	public Point3D getPosition();
	public Quaternion getOrientation();
	
}
