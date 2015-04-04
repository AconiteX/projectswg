package engine.clients;

import java.net.SocketAddress;


public class GameClient extends Client {
	
	private long characterId;


	public GameClient(SocketAddress endPoint) {
		super(endPoint);
	}
	
	public long getCharacterId() { return this.characterId; };
	public void setCharacterId(long characterId) { this.characterId = characterId; }
	

}
