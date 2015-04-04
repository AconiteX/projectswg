package engine.resources.container;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import resources.objects.cell.CellObject;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;


@Persistent
public class ContainerSlot extends AbstractSlot implements Serializable {
	
		private static final long serialVersionUID = 1L;
		List<SWGObject> objects;
		public ContainerSlot(String name) {
			super(name);
			objects = new ArrayList<SWGObject>();
		}
		
		public ContainerSlot() { }
		
		@Override
		public boolean isFilled() {
			return false;
		}
		
		@Override
		public void insert(SWGObject swgObj) {
			objects.add(swgObj);
		}

		@Override
		public void traverse(SWGObject viewer, boolean topDown, boolean recursive, Traverser traverser) {
			for(SWGObject obj : objects) {
				
				if(obj == null) {
					//System.err.println("Traversing through null object in container");
					continue;
				}
				
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
			objects.remove(swgObj);
		}

		@Override
		public void clear() {
			objects.clear();
		}

		@Override
		public SWGObject getObject() {
			return null;
		}
		
		public void inverse() {
			Collections.reverse(objects);
		}
		
		public void sortCells() {
			SWGObject[] cells = new SWGObject[objects.size() + 1];
			for(SWGObject obj : objects) {
				if(!(obj instanceof CellObject))
					return;
				cells[((CellObject) obj).getCellNumber()] = obj;
			}
			objects = Arrays.asList(cells);
		}
	}
