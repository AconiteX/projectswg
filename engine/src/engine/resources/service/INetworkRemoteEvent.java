package engine.resources.service;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;

public interface INetworkRemoteEvent {
	
	public void handlePacket(IoSession session, IoBuffer data) throws Exception;


}
