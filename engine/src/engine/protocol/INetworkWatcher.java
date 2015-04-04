package engine.protocol;

import java.net.SocketAddress;

public interface INetworkWatcher {
	public void OnReceive(SocketAddress endPoint, byte[] message);
}
