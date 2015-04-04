package engine.resources.container;

import engine.resources.objects.SWGObject;

/**
 * An interface to traverse a collection of objects and create an anonymous function for each object in the collection.
 */
public interface Traverser {

	/**
	 * Called for every object that this Traverser is traversing.
	 * 
	 * @param object the current object.
	 */
	public void process(SWGObject object);
	
}
