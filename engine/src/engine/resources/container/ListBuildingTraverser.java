package engine.resources.container;

import java.util.ArrayList;
import java.util.List;

import engine.resources.objects.SWGObject;


public class ListBuildingTraverser implements Traverser {

	private List<SWGObject> objects;
	
	/**
	 * Creates a new ListBuildingTraverser
	 */
	public ListBuildingTraverser() {
		objects = new ArrayList<SWGObject>();
	}
	
	@Override
	public void process(SWGObject object) {
		objects.add(object);
	}

	/**
	 * @return the list that was built
	 */
	public List<SWGObject> getList() {
		return objects;
	}
	
}
