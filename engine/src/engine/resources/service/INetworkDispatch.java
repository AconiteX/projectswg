package engine.resources.service;

import java.util.Map;

public interface INetworkDispatch {
	
	public void insertOpcodes(Map<Integer,INetworkRemoteEvent> swgOpcodes, Map<Integer,INetworkRemoteEvent> objControllerOpcodes);
	
	public void shutdown();
	
}
