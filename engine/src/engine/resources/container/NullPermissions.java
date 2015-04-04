package engine.resources.container;

import java.io.Serializable;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;

@Persistent
public class NullPermissions implements ContainerPermissions , Serializable {
	
	private static final long serialVersionUID = 1L;
	public final static NullPermissions NULL_PERMISSIONS = new NullPermissions();
	
	public NullPermissions() {}
	
	@Override
	public boolean canInsert(SWGObject actor, SWGObject actee) {
		return false;
	}

	@Override
	public boolean canRemove(SWGObject actor, SWGObject actee) {
		return false;
	}

	@Override
	public boolean canView(SWGObject actor, SWGObject actee) {
		return false;
	}

}
