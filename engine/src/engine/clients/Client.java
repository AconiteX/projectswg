package engine.clients;

import java.net.SocketAddress;

import org.apache.mina.core.session.IoSession;

import engine.clients.connection.Connection;
import engine.resources.common.DebugSession;
import engine.resources.objects.SWGObject;

public class Client {

	protected IoSession session;
	protected long accountId;
	protected String accountName;
	protected String password;
	protected String accessLevel;
	protected byte[] sessionKey;
	protected long lastPacket;
	protected boolean disconnected = false;
	public boolean ready = false;
	
	public Connection connection;
	public SWGObject parent;
	
	public Client(SocketAddress endPoint) {
		this.connection = new Connection(endPoint);
	}
	
	public IoSession getSession() { return session; }
	public void setSession(IoSession s) { this.session = ((!DebugSession.debugPackets) ? s : new DebugSession(s)); }
	
	public long getAccountId() { return accountId; }
	public void setAccountId(long accountID) { this.accountId = accountID; }
	
	public String getAccountName() { return accountName; }
	public void setAccountName(String accountName) { this.accountName = accountName; }

	public String getAccessLevel() { return accessLevel; }
	public void setAccessLevel(String accessLevel) { this.accessLevel = accessLevel; }
	
	public Connection getConnection() { return connection; }
	public int getConnectionId() { return connection.getConnectionId(); }
	public String getPassword() { return password; }
	
	public byte[] getSessionKey() {
		return sessionKey;
	}
	
	public byte[] setSessionKey(byte[] sessionKey) {
		return this.sessionKey = sessionKey;
	}
	public void setPassword(String password) {
		this.password = password;
	}
	
	public void setDisconnected(boolean disconnected) {
		this.disconnected = disconnected;
	}
	
	public boolean isDisconnected() {
		return disconnected;
	}

	public void makeUnaware(SWGObject object) {
		parent.makeUnaware(object);
	}
	public void makeAware(SWGObject object) {
		parent.makeAware(object);
	}

	public SWGObject getParent() {
		return parent;
	}

	public void setParent(SWGObject parent) {
		this.parent = parent;	
	}
	
}
