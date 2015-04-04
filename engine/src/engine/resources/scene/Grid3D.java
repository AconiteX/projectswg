package engine.resources.scene;

import java.util.List;
import java.util.ArrayList;
import java.util.Map;
import java.util.HashMap;
import java.util.concurrent.ConcurrentHashMap;

@SuppressWarnings("unused")

public class Grid3D<o> {
	
	private Grid grid;
	private int squareSize;
	
	private class Grid {
		
		private Map<String, Map<String, Map<String, List<o>>>> grid;
		
		public Grid() {
			grid = new ConcurrentHashMap<String, Map<String, Map<String, List<o>>>>();
		}
		
		public Map<String, Map<String, Map<String, List<o>>>> get() {
			return grid;
		}

		public Map<String, Map<String, List<o>>> get(int x) {
			return grid.get(str(x));
		}
		
		public Map<String, List<o>> get(int x, int y) {
			return grid.get(str(x)).get(str(y));
		}
		
		public List<o> get(int x, int y, int z) {
			return grid.get(str(x)).get(str(y)).get(str(z));
		}
		
		public void set(int x, Map<String, Map<String, List<o>>> o) {
			grid.put(str(x), o);
		}
		
		public void set(int x, int y, Map<String, List<o>> o) {
			grid.get(str(x)).put(str(y), o);
		}
		
		public void set(int x, int y, int z, List<o> container) {
			grid.get(str(x)).get(str(y)).put(str(z), container);
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
		
		public boolean empty(int x, int y, int z) {
			if (grid.get(str(x)).get(str(y)).get(str(z)) != null) {
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
		
		public boolean exists(int x, int y, int z) {
			if (exists(x, y) && grid.get(str(x)).get(str(y)).containsKey(str(y))) {
				return true;
			} else {
				return false;
			}
		}
		
		private String str(int i) {
			return Integer.toString(i);
		}
		
	}

	public Grid3D() {
		grid = new Grid();
		squareSize = 1;
	}
	
	public void setSquareSize(int squareSize) {
		if (!(grid.get().size() > 0)) {
			this.squareSize = (squareSize < 1) ? 1 : squareSize;
		}
	}
	
	public void add(o object, int x, int y, int z) {
		if (!grid.exists(x)) {
			grid.set(x, new HashMap<String, Map<String, List<o>>>());
		}
		
		if (!grid.exists(x, y)) {
			grid.set(x, y, new HashMap<String, List<o>>());
		}
			
		if (!grid.exists(x, y, z)) {
			grid.set(x, y, null);
		}
			
		if (grid.get(x, y, z) == null) {
			List<o> container = new ArrayList<o>();
			int newX = (((x / squareSize) * squareSize) - ((x < 0) ? squareSize : 0));
			int newY = (((y / squareSize) * squareSize) - ((y < 0) ? squareSize : 0));
			int newZ = (((z / squareSize) * squareSize) - ((z < 0) ? squareSize : 0));

			for (int tempX = (newX + squareSize); newX < tempX; newX++, newY -= squareSize) {
				if (!grid.exists(newX)) {
					grid.set(newX, new HashMap<String, Map<String, List<o>>>());
				}

				for (int tempY = (newY + squareSize); newY < tempY; newY++, newZ -= squareSize) {
					if (!grid.exists(newX, newY)) {
						grid.set(newX, newY, new HashMap<String, List<o>>());
					}

					for (int tempZ = (newZ + squareSize); newZ < tempZ; newZ++) {
						if (!grid.exists(newX, newY, newZ)) {
							grid.set(newX, newY, null);
						}
						
						if (grid.get(newX, newY, newZ) == null) {
							grid.set(newX, newY, newZ, container);
						}
					}
				}
			}
		}

		(grid.get(x, y, z)).add(object);
	}
	
	public boolean move(o object, int oldX, int oldY, int oldZ, int newX, int newY, int newZ) {
		if (remove(object, oldX, oldY, oldZ)) {
			add(object, newX, newY, newZ);
			
			return true;
		}
		
		return false;
	}
	
	public List<o> get(int x, int y, int z) {
		if (grid.exists(x, y, z) && grid.get(x, y, z) != null) {
			return grid.get(x, y, z);
		}
		
		return new ArrayList<o>();
	}
	
	public List<o> get(int x, int y, int z, int range) {
		int i, tempX, tempY, tempZ;
		List<o> objectList = new ArrayList<o>();

		for (tempX = (x - range); tempX < (x + range); tempX += squareSize) {
			for (tempY = (y - range); tempY < (y + range); tempY += squareSize) {
				for (tempZ = (z - range); tempZ < (z + range); tempZ += squareSize) {
					if (grid.exists(tempX, tempY, tempZ) && grid.get(tempX, tempY, tempZ) != null
					&& grid.get(tempX, tempY, tempZ).size() > 0) {
						for (i = 0; i < grid.get(tempX, tempY, tempZ).size(); i++) {
							objectList.add(grid.get(tempX, tempY, tempZ).get(i));
						}
					}
				}
			}
		}
		
		return objectList;
	}
	
	public boolean remove(o object, int x, int y, int z) {
		if (grid.exists(x, y, z) && grid.get(x, y, z) != null) {
			if ((grid.get(x, y, z)).contains(object)) {
				(grid.get(x, y, z)).remove(object);
				
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
		
		for (int x = minRange; x < maxRange; x++) {
			if (grid.exists(x) && !grid.empty(x)) {
				size += grid.get(x).size();
			}
		}
		
		return size;
	}
	
	public int getZs(int minRange, int maxRange) {
		int size = 0;
		int y;
		
		for (int x = minRange; x < maxRange; x++) {
			if (grid.exists(x) && !grid.empty(x)) {
				for (y = minRange; y < maxRange; y++) {
					if (grid.exists(y) && !grid.empty(y)) {
						size += grid.get(x).get(y).size();
					}
				}
			}
		}
		
		return size;
	}

}
