package engine.resources.scene;

import java.util.Map;
import java.util.HashMap;

/*
 *  Committed Timm's zone partioning class
 *  too incase mine is inadequet.  It still
 *  needs some modifications to work though.
 *  
 *  ~Treeku
 */

public class ZonePartition {

	private int minX, maxX, minY, maxY;
	private int partitionW, partitionH;
	private int partitionCols;
	
	protected Map<Long, Integer> entities = new HashMap<Long, Integer>();
	
	private ZonePartition(int w, int h, int partitionCount) {
		
		minX = -(w / 2);
		maxX = minX * -1;
		minY = -(h / 2);
		maxY = minY * -1;
		
		partitionCols = (int) Math.sqrt(partitionCount);
		
		partitionW = w / partitionCols;
		partitionH = h / partitionCols;
		
	}
	
	public static ZonePartition create(int width, int height, int partitionCount) {
		
		if (Math.sqrt(partitionCount) % 1 != 0)
			throw new IllegalArgumentException("partitionCount must be a perfect square");
		
		if ((width / Math.sqrt(partitionCount) % 1) != 0)
			throw new IllegalArgumentException("width must be evenly divisable by the square root of partitionCount");
				
		if ((height / Math.sqrt(partitionCount) % 1) != 0)
			throw new IllegalArgumentException("height must be evenly divisable by the square root of partitionCount");
		
		return new ZonePartition(width, height, partitionCount);
		
	}
	
	public int getPartition(long objectId) {
		
		if (entities.containsKey(objectId))
			return entities.get(objectId);
		
		return -1;
		
	}
	
	/*
	// "Point" doesn't exist yet ~Treeku
	public int getPartition(Point point) {
		
		return getPartition(point.x, point.y);
	
	}
	*/
	
	public int getPartition(float x, float y) {
	
		if (x < minX || x > maxX || y < minY || y > maxY)		
			return -1;
		
		int col = getColumn(x);
		int row = getRow(y, col % 2 == 1);

		return  row * partitionCols + col;
		
	}
	
	protected int getColumn(float x) {
		
		return (int) ((x - minX) / partitionW);
	
	}
	
	protected int getRow(float y, boolean offset) {
		
		int offsetValue = partitionH / 2;
		return (int)((y - minY + (offset ? offsetValue : 0)) / partitionH);
		
	}
	
	public void update(long objectId, float x, float y) {
		
		
		
	}
	
	public int[] getAdjacent(int sourcePartition, int iteration) {
		
		int[] partitions = new int[getAdjacentPartitionCount(iteration)];
		
		int currentPartitionIndex = 0;
		partitions[currentPartitionIndex++] = sourcePartition + partitionCols * iteration;
		try {
			
			for (int direction = 0; direction < 6; direction++)
				for (int i = 1; i <= iteration; i++)
					partitions[currentPartitionIndex] = advanceSequence(partitions[currentPartitionIndex++ - 1], direction);
			
		} catch (ArrayIndexOutOfBoundsException e) {
			
		}

		return partitions;
		
	}
	
	protected int getAdjacentPartitionCount(int iteration) {

		return iteration * 6;
	
	}
	
	protected int advanceSequence(int sourcePartition, int direction) {
		
		switch (direction) {
			case 0:
				return sourcePartition + 1 - (sourcePartition % 2 == 1 ? partitionCols : 0);
			case 1:
				return sourcePartition - partitionCols;
			case 2:
				return sourcePartition - 1 - (sourcePartition % 2 == 1 ? partitionCols : 0);
			case 3:
				return sourcePartition - 1 + (sourcePartition % 2 == 0 ? partitionCols : 0);
			case 4:
				return sourcePartition + partitionCols;
			case 5:
				return sourcePartition + 1 + (sourcePartition % 2 == 0 ? partitionCols : 0);
			default:
				return -1;
		}
		
	}
	
	public void insert(long objectId, int partition) {
		entities.put(objectId, partition);
	}
	
	public void remove(long objectId) {
		entities.remove(objectId);
	}
	
}
