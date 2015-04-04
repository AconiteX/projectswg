package engine.resources.container;

import java.io.Serializable;

import com.sleepycat.persist.model.Persistent;

import engine.resources.objects.SWGObject;
import main.NGECore;
import resources.datatables.Posture;
import resources.objects.creature.CreatureObject;
import resources.objects.group.GroupObject;
import services.ai.AIActor;

@Persistent
public class CreatureContainerPermissions implements ContainerPermissions, Serializable {
	
	private static final long serialVersionUID = 1L;
	public final static CreatureContainerPermissions CREATURE_CONTAINER_PERMISSIONS = new CreatureContainerPermissions();
	
	public CreatureContainerPermissions() {}
	
	@Override
	public boolean canInsert(SWGObject requester, SWGObject container) {
		if(requester instanceof CreatureObject && ((CreatureObject) requester).getPosture() == Posture.Dead)
			return false;
		if(container.getContainer() == requester || container.getGrandparent() == requester || requester == container) 
			return true;
		if(container.getGrandparent() != null && container.getGrandparent().getContainer() == requester)
			return true;
		if(container.getAttachment("AI") != null) {
			try {
				if (((AIActor) container.getAttachment("AI")).getHighestDamageDealer() == requester)
					return true;
				
				if (requester instanceof CreatureObject && ((CreatureObject) requester).getGroupId() != 0) {
					CreatureObject highestDamageDealer = (((AIActor) container.getAttachment("AI")).getHighestDamageDealer());
					
					for (SWGObject member : ((GroupObject) NGECore.getInstance().objectService.getObject(((CreatureObject) requester).getGroupId())).getMemberList()) {
						if (member == highestDamageDealer) {
							return true;
						}
					}
				}
			} catch (Exception e) {
				System.err.println("canInsert::Error with checking loot container permissions.");
			}
		}
		return false;
	}

	@Override
	public boolean canRemove(SWGObject requester, SWGObject container) {
		if(requester instanceof CreatureObject && ((CreatureObject) requester).getPosture() == Posture.Dead)
			return false;
		if(container.getContainer() == requester || container.getGrandparent() == requester || requester == container) 
			return true;
		if(container.getGrandparent() != null && container.getGrandparent().getContainer() == requester)
			return true;
		if(container.getAttachment("AI") != null) {
			try {
				if (((AIActor) container.getAttachment("AI")).getHighestDamageDealer() == requester)
					return true;
				
				if (requester instanceof CreatureObject && ((CreatureObject) requester).getGroupId() != 0) {
					CreatureObject highestDamageDealer = (((AIActor) container.getAttachment("AI")).getHighestDamageDealer());
					
					for (SWGObject member : ((GroupObject) NGECore.getInstance().objectService.getObject(((CreatureObject) requester).getGroupId())).getMemberList()) {
						if (member == highestDamageDealer) {
							return true;
						}
					}
				}
			} catch (Exception e) {
				System.err.println("canRemove::Error with checking loot container permissions.");
			}
		}
		return false;
	}

	@Override
	public boolean canView(SWGObject requester, SWGObject container) {
		if(requester instanceof CreatureObject && ((CreatureObject) requester).getPosture() == Posture.Dead)
			return false;
		if(container.getContainer() == requester || container.getGrandparent() == requester || requester == container) 
			return true;
		if(container.getGrandparent() != null && container.getGrandparent().getContainer() == requester)
			return true;
		if(container.getAttachment("AI") != null) {
			try {
				if (requester == ((AIActor) container.getAttachment("AI")).getHighestDamageDealer())
					return true;
				
				if (requester instanceof CreatureObject && ((CreatureObject) requester).getGroupId() != 0) {
					CreatureObject highestDamageDealer = (((AIActor) container.getAttachment("AI")).getHighestDamageDealer());
					
					for (SWGObject member : ((GroupObject) NGECore.getInstance().objectService.getObject(((CreatureObject) requester).getGroupId())).getMemberList()) {
						if (member == highestDamageDealer) {
							return true;
						}
					}
				}
			} catch (Exception e) {
				System.err.println("canView::Error with checking loot container permissions.");
			}
		}
		return false;
	}


}
