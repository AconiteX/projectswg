package engine.resources.container;

import engine.resources.objects.SWGObject;

public interface ContainerPermissions {
	
	public boolean canInsert(SWGObject requester, SWGObject container);
	public boolean canRemove(SWGObject requester, SWGObject container);
	public boolean canView(SWGObject requester, SWGObject container);
	
}
