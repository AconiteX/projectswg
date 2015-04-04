/*
 * Crash debugging tool.
 * 
 * Enable debugPackets for this to work.
 * 
 * -Prints all packets sent to and from the client.
 * -Fully timestamped.
 * -This includes pings.  When pings stop, this is the time the client crashed.
 */
package engine.resources.common;

import java.net.SocketAddress;
import java.util.Calendar;
import java.util.Set;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.filterchain.IoFilterChain;
import org.apache.mina.core.future.CloseFuture;
import org.apache.mina.core.future.ReadFuture;
import org.apache.mina.core.future.WriteFuture;
import org.apache.mina.core.service.IoHandler;
import org.apache.mina.core.service.IoService;
import org.apache.mina.core.service.TransportMetadata;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.core.session.IoSessionConfig;
import org.apache.mina.core.write.WriteRequest;
import org.apache.mina.core.write.WriteRequestQueue;

/*
 * Commented what I found useful while debugging. ~Treeku
 */
public class DebugSession implements IoSession {
	
	public static boolean debugPackets = false;
	public static long lastPing = 0;
	
	private IoSession session;
	
	@SuppressWarnings("unused")
	public WriteFuture write(Object arg0) {
		/*
		// Synchronize and delay packets to let them process slowly until they crash.
		synchronized(session) {
			/*
			try {
				Thread.sleep(10000);
			} catch (InterruptedException e) {
				
			}
		}
		*/
		
		// Will cause a cast exception if it's not IoBuffer, alerting us to where
		IoBuffer buffer = ((IoBuffer) arg0);
		
		byte[] bytes = ((IoBuffer) arg0).array();
		
		// Checks for a specific byte, and if true, doesn't send it, to see if it stops a crash.
		if (bytes.length > 2 && bytes[2] == 0x53) {
			//return null;
		}
		
		// View the packet as Ascii so we can easily check if it contains "creo" and such for debugging
		String ascii = new String(bytes, 0, bytes.length);
		
		/*
		if (ascii.contains("OERC")) {
			// Inform us that it was detected.
			Console.println("Contains OERC");
			
			// Or display data for this packet
			System.out.println(Calendar.getInstance().getTime() + ": >" + Utilities.getHexString(bytes));
			
			// Check specific viewType and Delta id
			if (bytes.length > 25 && bytes[18] == 0x01 && bytes[25] == 0x03) {
				Console.println("CREO1 object 3 delta detected.");
				return null;
			}
			
			return null;
		}
		*/
		
		//while ((System.currentTimeMillis() - lastPing) > 1L);
		
		// Display all packets before they get sent out
		System.out.println(Calendar.getInstance().getTime() + ": >" + Utilities.getHexString((((IoBuffer) arg0).array())));
		
		// Send the packet out
		return session.write(arg0);
	}
	
	public DebugSession(IoSession session) { this.session = session; }
	public DebugSession() {}
	@SuppressWarnings("deprecation") public CloseFuture close() { return session.close(); }
	public CloseFuture close(boolean arg0) { return session.close(arg0); }
	public boolean containsAttribute(Object arg0) { return session.containsAttribute(arg0); }
	@SuppressWarnings("deprecation") public Object getAttachment() { return session.getAttachment(); }
	public Object getAttribute(Object arg0) { return session.getAttribute(arg0); }
	public Object getAttribute(Object arg0, Object arg1) { return session.getAttribute(arg0, arg1); }
	public Set<Object> getAttributeKeys() { return session.getAttributeKeys(); }
	public int getBothIdleCount() { return session.getBothIdleCount(); }
	public CloseFuture getCloseFuture() { return session.getCloseFuture(); }
	public IoSessionConfig getConfig() { return session.getConfig(); }
	public long getCreationTime() { return session.getCreationTime(); }
	public Object getCurrentWriteMessage() { return session.getCurrentWriteMessage(); }
	public WriteRequest getCurrentWriteRequest() { return session.getCurrentWriteRequest(); }
	public IoFilterChain getFilterChain() { return session.getFilterChain(); }
	public IoHandler getHandler() { return session.getHandler(); }
	public long getId() { return session.getId(); }
	public int getIdleCount(IdleStatus arg0) { return session.getIdleCount(arg0); }
	public long getLastBothIdleTime() { return session.getLastBothIdleTime(); }
	public long getLastIdleTime(IdleStatus arg0) { return session.getLastIdleTime(arg0); }
	public long getLastIoTime() { return session.getLastIoTime(); }
	public long getLastReadTime() { return session.getLastReadTime(); }
	public long getLastReaderIdleTime() { return session.getLastReaderIdleTime(); }
	public long getLastWriteTime() { return session.getLastWriteTime(); }
	public long getLastWriterIdleTime() { return session.getLastWriterIdleTime(); }
	public SocketAddress getLocalAddress() { return session.getLocalAddress(); }
	public long getReadBytes() { return session.getReadBytes(); }
	public double getReadBytesThroughput() { return session.getReadBytesThroughput(); }
	public long getReadMessages() { return session.getReadMessages(); }
	public double getReadMessagesThroughput() { return session.getReadMessagesThroughput(); }
	public int getReaderIdleCount() { return session.getReaderIdleCount(); }
	public SocketAddress getRemoteAddress() { return session.getRemoteAddress(); }
	public long getScheduledWriteBytes() { return session.getScheduledWriteBytes(); }
	public int getScheduledWriteMessages() { return session.getScheduledWriteMessages(); }
	public IoService getService() { return session.getService(); }
	public SocketAddress getServiceAddress() { return session.getServiceAddress(); }
	public TransportMetadata getTransportMetadata() { return session.getTransportMetadata(); }
	public WriteRequestQueue getWriteRequestQueue() { return session.getWriteRequestQueue(); }
	public int getWriterIdleCount() { return session.getWriterIdleCount(); }
	public long getWrittenBytes() { return session.getWrittenBytes(); }
	public double getWrittenBytesThroughput() { return session.getWrittenBytesThroughput(); }
	public long getWrittenMessages() { return session.getWrittenMessages(); }
	public double getWrittenMessagesThroughput() { return session.getWrittenMessagesThroughput(); }
	public boolean isBothIdle() { return session.isBothIdle(); }
	public boolean isClosing() { return session.isClosing(); }
	public boolean isConnected() { return session.isConnected(); }
	public boolean isIdle(IdleStatus arg0) { return session.isIdle(arg0); }
	public boolean isReadSuspended() { return session.isReadSuspended(); }
	public boolean isReaderIdle() { return session.isReaderIdle(); }
	public boolean isWriteSuspended() { return session.isWriteSuspended(); }
	public boolean isWriterIdle() { return session.isWriterIdle(); }
	public ReadFuture read() { return session.read(); }
	public Object removeAttribute(Object arg0) { return session.removeAttribute(arg0); }
	public boolean removeAttribute(Object arg0, Object arg1) { return session.removeAttribute(arg0, arg1); }
	public boolean replaceAttribute(Object arg0, Object arg1, Object arg2) { return session.replaceAttribute(arg0, arg1, arg2); }
	public void resumeRead() { session.resumeRead(); }
	public void resumeWrite() { session.resumeWrite(); }
	@SuppressWarnings("deprecation") public Object setAttachment(Object arg0) { return session.getAttachment(); }
	public Object setAttribute(Object arg0) { return session.setAttribute(arg0); }
	public Object setAttribute(Object arg0, Object arg1) { return session.setAttribute(arg0, arg1); }
	public Object setAttributeIfAbsent(Object arg0) {return session.setAttributeIfAbsent(arg0); }
	public Object setAttributeIfAbsent(Object arg0, Object arg1) { return session.setAttributeIfAbsent(arg0, arg1); }
	public void setCurrentWriteRequest(WriteRequest arg0) { session.setCurrentWriteRequest(arg0); }
	public void suspendRead() { session.suspendRead(); }
	public void suspendWrite() { session.suspendWrite(); }
	public void updateThroughput(long arg0, boolean arg1) { session.updateThroughput(arg0, arg1); }
	public WriteFuture write(Object arg0, SocketAddress arg1) { return session.write(arg0, arg1); }
	
}