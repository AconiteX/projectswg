package engine.resources.container;

import java.io.Serializable;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;


@Persistent
public class ExclusiveSlot extends AbstractSlot implements Serializable {
	
		private static final long serialVersionUID = 1L;
		private volatile SWGObject obj;
		
		public ExclusiveSlot(String name) {
			super(name);
		}
		
		public ExclusiveSlot() { }
		
		public SWGObject getObject() { return obj; }
		
		@Override
		public boolean isFilled() {
			return obj != null;
		}
		
		@Override
		public void insert(SWGObject swgObj) {
			obj = swgObj;
		}

		@Override
		public void traverse(SWGObject viewer, boolean topDown, boolean recursive, Traverser traverser) {
			if(obj != null) {
				if(recursive && !topDown) {
					obj.viewChildren(viewer, topDown, recursive, traverser);
				}
				traverser.process(obj);
				if(recursive && topDown) {
					obj.viewChildren(viewer, topDown, recursive, traverser);
				}
			}
		}

		@Override
		public void remove(SWGObject swgObj) {
			if(obj == swgObj) {
				obj = null;
			}
		}

		@Override
		public void clear() {
			obj = null;
		}
		
	}
