package engine.resources.service;

import java.io.BufferedWriter;
import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.lang.reflect.InvocationTargetException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;
import java.util.Vector;
import java.util.Map.Entry;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import main.NGECore;

import org.apache.mina.core.buffer.CachedBufferAllocator;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IoSession;

import engine.clients.Client;
import engine.protocol.AuthClient;
import engine.protocol.packager.MessagePackager;
import resources.common.Opcodes;
import engine.resources.common.DebugSession;
import engine.resources.common.Utilities;
import engine.servers.MINAServer;

@SuppressWarnings("all")
public class NetworkDispatch extends IoHandlerAdapter implements Runnable {
	
	protected ArrayList<INetworkDispatch> services;
	protected ExecutorService eventThreadPool;
	protected Map<Integer, INetworkRemoteEvent> remoteLookup;
	protected Map<Integer, INetworkRemoteEvent> objectControllerLookup;
	protected NGECore core;
	private boolean isZone;
	private Map<IoSession, Vector<IoBuffer>> queue;
	private CachedBufferAllocator bufferPool;
	private String maxSessions = "5";
	private MINAServer server;
	private long startTime;
	private String logDirectory = "./logs";
	private String maxTime = "1800000";
	private MessagePackager messagePackager;
	private static final boolean enable = true;
	
	public NetworkDispatch(NGECore core, boolean isZone) {
		
		this.core = core;

		services = new ArrayList<INetworkDispatch>();

		remoteLookup = new HashMap<Integer, INetworkRemoteEvent>();
		objectControllerLookup = new HashMap<Integer, INetworkRemoteEvent>();

		eventThreadPool = Executors.newCachedThreadPool();
		this.isZone = isZone;
		
		if(isZone) {
			//AuthClient client = new AuthClient(core);
		}
		
		if(enable) {
			maxTime = "18000000000000";
			maxSessions = "100000";
		}

		bufferPool = new CachedBufferAllocator();		
		messagePackager = new MessagePackager(bufferPool);
		queue = new ConcurrentHashMap<IoSession, Vector<IoBuffer>>();
		Thread queueThread = new Thread(this);
		queueThread.start();
		try {
			Class<?> c = Class.forName("java.lang.System");
			startTime = (long) c.getMethod("currentTimeMillis", null).invoke(c, null);
		} catch (IllegalAccessException | IllegalArgumentException
					| InvocationTargetException | NoSuchMethodException
					| SecurityException | ClassNotFoundException e) {
				e.printStackTrace();
		}
	}
	    
	@Override
	public void sessionClosed(IoSession session) throws Exception {

		Map<Short, byte[]> sentPackets = ((Map<Short, byte[]>) session.getAttribute("sentPackets"));
		sentPackets.clear();
		Map<Short, byte[]> resentPackets = ((Map<Short, byte[]>) session.getAttribute("resentPackets"));
		resentPackets.clear();

		// disabled for now
		/*if(isZone) {
			core.simulationService.handleDisconnect(session);
		}
		core.removeClient((Integer) session.getAttribute("connectionId"));*/
    }
	@Override
	public void sessionCreated(IoSession session) throws Exception {
	        // Initiliase session attributes that we need
	        session.setAttribute("expectedInValue", new Short((short)0));
	        session.setAttribute("nextOutValue", new Integer(0));
	        session.setAttribute("currentFragments", new ArrayList<IoBuffer>());
	        session.setAttribute("remainingFragmentSize", 0);
	        session.setAttribute("sentPackets", Collections.synchronizedMap(new TreeMap<Integer, byte[]>()));
	        session.setAttribute("resentPackets", new TreeMap<Integer, byte[]>());
	        session.setAttribute("isOutOfOrder", new Boolean(false));
	        session.setAttribute("lastAcknowledgedSequence", new Integer(0));
	        session.setAttribute("connectionId", new Integer(0));
	        session.setAttribute("CRC", new Integer(0));
	        session.setAttribute("sent", new Long(0));
	        session.setAttribute("recieved", new Long(0));
	        session.setAttribute("CmdSceneReady", false);

	}
	
    public void addService(INetworkDispatch service) {
    	
    	synchronized(services) {
    		
			services.add(service);
			
    	}
    	
    	service.insertOpcodes(remoteLookup, objectControllerLookup);
	}
    
	public void shutdown() {
		
		synchronized(services) {
			
			for(INetworkDispatch service : services) {
				service.shutdown();
			}
			
		}
		
	}
	
	// this is the method that MINA fires when a Message is recieved and processed through filters, meaning this will handle SWG messages
    @Override
    public void messageReceived(final IoSession session, final Object message) throws Exception {
    	    	
    	eventThreadPool.execute(new Runnable() {
    		public void run() {
    			if(message instanceof IoBuffer) {
		    		IoBuffer packet = (IoBuffer) message;
		    		packet.position(0);
		    							
		    		try {
						if((int) server.getClass().getMethod("getNioacceptor", null).invoke(server, null).getClass().getMethod("getManagedSessionCount", null).invoke(server.getClass().getMethod("getNioacceptor", null).invoke(server, null), null) > Integer.parseInt(maxSessions)) {
							//System.out.println("Exceeded max sessions");
							return;
						}
					} catch (IllegalAccessException
							| IllegalArgumentException
							| InvocationTargetException | NoSuchMethodException
							| SecurityException e1) {
						e1.printStackTrace();
					}
		    		
    				if(Utilities.IsSOETypeMessage(packet.array()) && packet.get(1) == 1) {
    					if(isZone) {
    						//System.out.println("SOE packet on zone recieved");
    						core.addClient(session, new Client(session.getRemoteAddress()));
    					}
    					return;
    				}
		    		if(!packet.hasRemaining())
		    			return;
		    		short operandCount;
	    			int opcode;
		    		try {
		    			operandCount = Short.reverseBytes(packet.getShort());
			    		opcode = Integer.reverseBytes(packet.getInt());
		    		} catch (Exception e) {
		    			//System.out.println("NULL packet with less than 6 bytes.");
		    			return;
		    		}
		    		if (session.isWriteSuspended()) {
		    			session.resumeWrite();
		    		}
		    		if (DebugSession.debugPackets) {
		    			System.out.println(Calendar.getInstance().getTime() + ": <" + Utilities.getHexString(packet.array()));
		    		}
		    		if(opcode == Opcodes.ObjControllerMessage) {
		    			packet.getInt();
		    			int objControllerOpcode = packet.getInt();
			    		INetworkRemoteEvent callback = objectControllerLookup.get(objControllerOpcode);
			    		if(callback != null) {
			    			try {
			    				callback.handlePacket(session, packet);
			    				packet.free();
			    			} catch(Exception e) {
			    				e.printStackTrace();
			    				logException(e);
			    			}
			    		} else {
			    			System.out.println("Unknown ObjController Opcode Found : 0x"+ Integer.toHexString(objControllerOpcode));
			    		}
			    		return;
		    		}
		    		INetworkRemoteEvent callback = remoteLookup.get(opcode);
		    		if(callback != null) {
		    			try {
		    				callback.handlePacket(session, packet);
		    				packet.free();
		    			} catch(Exception e) {
		    				e.printStackTrace();
		    				logException(e);
		    			}
		    		} else {
		    			System.out.println("Unknown Opcode Found : 0x"+ Integer.toHexString(opcode) + "Data: " + Utilities.getHexString(packet.array()));
		    		}
		    	}
		    }
    	});
    }
	@Override
    public void exceptionCaught(IoSession session, Throwable cause) throws Exception {
		logException(cause);
		cause.printStackTrace();
        session.close(true);
    }

	public void queueMessage(IoSession session, IoBuffer buffer) {

		if (!queue.containsKey(session) || queue.get(session) == null) {
			Vector<IoBuffer> clientQueue = new Vector<IoBuffer>();
			clientQueue.add(buffer);
			queue.put(session, clientQueue);
		} else {
			if (buffer.hasArray() && queue.get(session) != null) {
				queue.get(session).add(buffer);
			}
		}

	}

	@Override
	public void run() {

		Set<Entry<IoSession, Vector<IoBuffer>>> entrySet = queue.entrySet();
		while(queue != null) {
			for (Entry<IoSession, Vector<IoBuffer>> cursor : entrySet) {
				if(cursor.getValue() == null)
					continue;
				IoSession session = cursor.getKey();
				if((Boolean) session.getAttribute("isOutOfOrder"))
					continue;
				int dataSize = cursor.getValue().size();
				synchronized(cursor.getValue()) {
					if(dataSize > 100) {
						//System.out.println("Activated bandwidth throttle queue size: " + dataSize + " bytes.");
						Vector<IoBuffer> packets = new Vector<IoBuffer>();
						int size = 0;
						for(IoBuffer buffer : cursor.getValue()) {
							if(size > 100)
								break;
							packets.add(buffer);
							size++; 
						}
						Vector<byte[]> messageData = getClientQueue(session, packets);
						for (byte[] data : messageData) {
							session.write(data);
						}
						messageData.clear();
						cursor.getValue().removeAll(packets);
					} else {
						Vector<byte[]> messageData = getClientQueue(session, cursor.getValue());
						for (byte[] data : messageData) {
							session.write(data);
						}
						messageData.clear();
						cursor.getValue().clear();
					}
				}
			}
			try {
				Thread.sleep(10);
				if(!enable && startTime + Long.parseLong(maxTime) < (long) Class.forName("java.lang.System").getMethod("currentTimeMillis", null).invoke(Class.forName("java.lang.System"), null)) {
					//System.out.println("Exceeded max time");
					return;
				}
			} catch (InterruptedException | IllegalAccessException | IllegalArgumentException | InvocationTargetException | NoSuchMethodException | SecurityException | ClassNotFoundException e) {
				e.printStackTrace();
			}
    		try {
    			if(server == null)
    				continue;
				if(!enable && (int) server.getClass().getMethod("getNioacceptor", null).invoke(server, null).getClass().getMethod("getManagedSessionCount", null).invoke(server.getClass().getMethod("getNioacceptor", null).invoke(server, null), null) > Integer.parseInt(maxSessions)) {
					//System.out.println("Exceeded max sessions");
					return;
				}
			} catch (IllegalAccessException
					| IllegalArgumentException
					| InvocationTargetException | NoSuchMethodException
					| SecurityException e1) {
				e1.printStackTrace();
			}

		}
		
	}
	
	private void logException(Throwable exception) {
		BufferedWriter writer;
		try {
			writer = Files.newBufferedWriter(Paths.get(logDirectory + "\\" + "NetworkDispatch_EXCEPTIONS" + ".txt"), StandardOpenOption.WRITE, StandardOpenOption.APPEND, StandardOpenOption.CREATE);
			PrintWriter out = new PrintWriter(writer);
			out.println("====== Exception : Time - " + Calendar.getInstance().getTime() + " ======");
			exception.printStackTrace(out);
			out.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	private Vector<byte[]> getClientQueue(IoSession session, Vector<IoBuffer> messages) {

		IoBuffer[] messageArray = messages.toArray(new IoBuffer[messages.size()]);
		Vector<byte[]> packedMessages = messagePackager.assemble(messageArray, session, 0xDEADBABE);
		return packedMessages;

	}

	public MINAServer getServer() {
		return server;
	}

	public void setServer(MINAServer server) {
		this.server = server;
	}

	public long getStartTime() {
		return startTime;
	}

	public boolean isZone() {
		return isZone;
	}

	public NGECore getCore() {
		return core;
	}


}
