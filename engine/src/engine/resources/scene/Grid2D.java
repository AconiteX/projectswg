package engine.resources.scene;

import java.util.List;
import java.util.ArrayList;
import java.util.Map;
import java.util.HashMap;
import java.util.concurrent.ConcurrentHashMap;

@SuppressWarnings("unused")

public class Grid2D<o> {
	
	private Grid grid;
	private int squareSize;
	
	private class Grid {
		
		private Map<String, Map<String, List<o>>> grid;
		
		public Grid() {
			grid = new ConcurrentHashMap<String, Map<String, List<o>>>();
		}
		
		public Map<String, Map<String, List<o>>> get() {
			return grid;
		}
		
		public Map<String, List<o>> get(int x) {
			return grid.get(str(x));
		}
		
		public List<o> get(int x, int y) {
			return grid.get(str(x)).get(str(y));
		}
		
		public void set(int x, Map<String, List<o>> o) {
			grid.put(str(x), o);
		}
		
		public void set(int x, int y, List<o> container) {
			grid.get(str(x)).put(str(y), container);
		}
		
		public boolean empty(int x) {
			if (grid.get(str(x)) != null) {
				return false;
			} else {
				return true;
			}
		}
		
		public boolean empty(int x, int y) {
			if (grid.get(str(x)).get(str(y)) != null) {
				return false;
			} else {
				return true;
			}
		}
		
		public boolean exists(int x) {
			if (grid.containsKey(str(x))) {
				return true;
			} else {
				return false;
			}
		}
		
		public boolean exists(int x, int y) {
			if (exists(x) && grid.get(str(x)).containsKey(str(y))) {
				return true;
			} else {
				return false;
			}
		}
		
		private String str(int i) {
			return Integer.toString(i);
		}
		
	}

	public Grid2D() {
		grid = new Grid();
		squareSize = 1;
	}
	
	public void setSquareSize(int squareSize) {
		if (!(grid.get().size() > 0)) {
			this.squareSize = (squareSize < 1) ? 1 : squareSize;
		}
	}
	
	public void add(o object, int x, int y) {
		if (!grid.exists(x)) {
			grid.set(x, new HashMap<String, List<o>>());
		}
			
		if (!grid.exists(x, y)) {
			grid.set(x, y, null);
		}
			
		if (grid.get(x, y) == null) {
			List<o> container = new ArrayList<o>();
			int newX = (((x / squareSize) * squareSize) - ((x < 0) ? squareSize : 0));
			int newY = (((y / squareSize) * squareSize) - ((y < 0) ? squareSize : 0));

			for (int tempX = (newX + squareSize); newX < tempX; newX++, newY -= squareSize) {
				if (!grid.exists(newX)) {
					grid.set(newX, new HashMap<String, List<o>>());
				}

				for (int tempY = (newY + squareSize); newY < tempY; newY++) {
					if (!grid.exists(newX, newY)) {
						grid.set(newX, newY, null);
					}

					if (grid.get(newX, newY) == null) {
						grid.set(newX, newY, container);
					}
				}
			}
		}
		if(grid.get(x, y) == null)
			System.out.println("Null container list");

		grid.get(x, y).add(object);
	}
	
	public boolean move(o object, int oldX, int oldY, int newX, int newY) {
		if (remove(object, oldX, oldY)) {
			add(object, newX, newY);
			System.out.println("Successful move");
			return true;
		}
		
		return false;
	}
	
	public List<o> get(int x, int y) {
		if (grid.exists(x, y) && grid.get(x, y) != null) {
			return grid.get(x, y);
		}
		
		return new ArrayList<o>();
	}
	
	public List<o> get(int x, int y, int range) {
		int i, tempX, tempY;
		List<o> objectList = new ArrayList<o>();

		for (tempX = (x - range); tempX < (x + range); tempX += squareSize) {
			for (tempY = (y - range); tempY < (y + range); tempY += squareSize) {
				if (grid.exists(tempX, tempY) && grid.get(tempX, tempY) != null
				&& grid.get(tempX, tempY).size() > 0) {
					for (i = 0; i < grid.get(tempX, tempY).size(); i++) {
						objectList.add(grid.get(tempX, tempY).get(i));
					}
				}
			}
		}
		return objectList;
	}
	
	public boolean remove(o object, int x, int y) {
		if (grid.exists(x, y) && grid.get(x, y) != null) {
			if ((grid.get(x, y)).contains(object)) {
				(grid.get(x, y)).remove(object);
				
				return true;
			}
		}
		
		return false;
	}
	
	public int getXs() {
		return grid.get().size();
	}
	
	public int getYs(int minRange, int maxRange) {
		int size = 0;
		
		for (int i = minRange; i < maxRange; i++) {
			if (grid.exists(i) && !grid.empty(i)) {
				size += grid.get(i).size();
			}
		}
		
		return size;
	}

}
