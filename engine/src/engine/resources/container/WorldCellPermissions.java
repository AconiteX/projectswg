package engine.resources.container;

import java.io.Serializable;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;

@Persistent
public class WorldCellPermissions implements ContainerPermissions , Serializable {
	
	private static final long serialVersionUID = 1L;
	public final static WorldCellPermissions WORLD_CELL_PERMISSIONS = new WorldCellPermissions();
	
	public WorldCellPermissions() {}

	@Override
	public boolean canInsert(SWGObject requester, SWGObject container) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean canRemove(SWGObject requester, SWGObject container) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public boolean canView(SWGObject requester, SWGObject container) {
		// TODO Auto-generated method stub
		return true;
	}

}
