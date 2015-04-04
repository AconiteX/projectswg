package engine.resources.container;

import java.io.Serializable;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;


@Persistent
public abstract class AbstractSlot implements Serializable {
	
		private static final long serialVersionUID = 1L;
		protected String name;
		
		public AbstractSlot() { }
		
		/**
		 * Creates a new slot
		 * @param name
		 */
		public AbstractSlot(String name) {
			this.name = name;
		}
		
		/**
		 * @return the name of the slot this object represents
		 */
		public String getName() { 
			return name; 
		}
		
		/**
		 * @return true if this slot is filled, false otherwise 
		 */
		public abstract boolean isFilled();
		
		public abstract SWGObject getObject();
		
		/**
		 * Inserts swgObj into this container. 
		 * 
		 * If the container is filled, the old object will be removed.
		 * 
		 * @param swgObj the object to insert
		 */
		public abstract void insert(SWGObject swgObj);
		
		/**
		 * Removes swgObj from this slot.
		 * @param swgObj the object to remove
		 */
		public abstract void remove(SWGObject swgObj);
		
		/**
		 * Empties this slot.
		 */
		public abstract void clear();
		
		/**
		 * Traverses over this tree from the perspective of viewer.
		 * @param viewer the object who is looking into this slot. Only relevant if recursive
		 * @param topDown the direction in which to recurse. Only relevant if recursive
		 * @param recursive true if subobjects should be traversed
		 * @param traverser the object to call as the tree is traversed.
		 */
		public abstract void traverse(SWGObject viewer, boolean topDown, boolean recursive, Traverser traverser);
}
