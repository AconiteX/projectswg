package engine.clients.connection;

import java.net.SocketAddress;
import java.util.Iterator;
import java.util.TreeMap;
import java.util.Vector;

import org.jboss.netty.buffer.ChannelBuffer;

import engine.protocol.soe.ISequenced;

public class Connection {
		
	protected SocketAddress endPoint;
	
	protected long sent = 0;
	protected long received = 0;
	
	protected int connectionId;
	protected int crc = 0;
	
	protected short nextClientSequence = 0;
	protected short nextServerSequence = 0;

	public TreeMap<Short, ChannelBuffer> sentPackets;

	protected Vector<ISequenced> unacknowledgedMessages;
	
	public Connection(SocketAddress endPoint) {
		this.endPoint = endPoint;
		sentPackets = new TreeMap<Short, ChannelBuffer>();
	}
	
	public SocketAddress getEndPoint() { return endPoint; }
	public long getSent() { return sent; }
	public void incSent() { sent++; }
	public long getReceived() { return received; }
	public void incReceived() { received++; }
	public int getConnectionId() { return connectionId; }
	public void setConnectionId(int connectionId) { this.connectionId = connectionId; }
	public int getCRC() { return crc; };
	public void setCRC(int crc) { this.crc = crc; }
	public short getSequence() { return (short) (nextServerSequence - 1); }
	public short getNextServerSequence() { return nextServerSequence; }
	public short getNextClientSequence() { return nextClientSequence; }


	public TreeMap<Short, ChannelBuffer> getSentPackets() {
		synchronized(sentPackets) {
			return sentPackets;
		}
	}

	public void addPacket(ChannelBuffer message, short sequence) {
		synchronized(sentPackets) {
			sentPackets.put(sequence, message);
		}
	}
	
	public void removeSentPacket(short sequence) {
		
		synchronized(sentPackets) {
			Iterator<Short> it = sentPackets.keySet().iterator();
			while(it.hasNext()) {
				Short e = it.next();
				if(e <= sequence) {
					it.remove();
				} else {
					continue;
				}
			}
			
		}
		
	}

	public void setNextServerSequence(short sequence) {

		if(nextServerSequence == 0) {
			nextServerSequence = 1;
		}
		else {
			nextServerSequence =  sequence;
		}
		
	}
	public void setNextClientSequence(short sequence) {

		if(nextClientSequence == 0) {
			nextClientSequence = 1;
		}
		else {
			nextClientSequence =  sequence;
		}
		
	}
}
