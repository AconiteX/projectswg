package engine.protocol;

import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;
import java.util.Map;

import main.NGECore;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.buffer.SimpleBufferAllocator;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;
import org.apache.mina.transport.socket.nio.NioDatagramAcceptor;

import protocol.swg.LoginServerId;
import protocol.swg.LoginServerString;

import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;
import engine.protocol.soe.DataChannelA;
import engine.protocol.soe.MultiProtocol;
import engine.protocol.soe.NetStatsClient;
import engine.protocol.soe.NetStatsServer;
import engine.resources.common.Utilities;
import engine.resources.config.Config;
import engine.resources.config.DefaultConfig;
import engine.resources.service.NetworkDispatch;
import engine.servers.MINAServer;

public class SOEProtocolDecoder implements ProtocolDecoder {
	
	private MessageCompression messageCompression;
	private MessageEncryption messageEncryption;
	private MessageCRC messageCRC;
	private SimpleBufferAllocator bufferPool;
	private String maxSessions = "5";
	private static final boolean enable = true;
	
	SOEProtocolDecoder() {
		this.messageCompression = new MessageCompression();
		this.messageEncryption = new MessageEncryption();
		this.messageCRC = new MessageCRC();
		bufferPool = new SimpleBufferAllocator();
		if(enable) {
			maxSessions = "100000";
		}

	}
	
	private void sendAck(IoSession session, short sequence) {
		SOEPacket packet = new SOEPacket((short)21, 7);
		packet.putNetShort(sequence);
		session.write(packet.flip());
	}
	
	private void sendOutofOrder(IoSession session, short sequence) {
		SOEPacket packet = new SOEPacket((short)21, 7);
		packet.putNetShort(sequence);
		session.write(packet.flip());
	}
	
	private void handlePacketBody(IoSession session, IoBuffer packetBody, ProtocolDecoderOutput output) throws Exception {
		if(packetBody.get(0) != 0x00) {
			output.write(packetBody);
			return;
		}
		packetBody.position(0);

		switch(packetBody.getShort()) 
		{
			case 3:
			{
				/*while((packetBody.remaining()) > 0) {
					int size = packetBody.get();
					handlePacketBody(session, packetBody.getSlice(size), output);
				}*/	
				
				MultiProtocol multi = new MultiProtocol(packetBody.array());
				for (IoBuffer data : multi.getMessages()) {
					if (data != null && data.hasArray() && data.array().length >= 2) {
						// SOE Packet
						if ((data.get(0) == 0x00 && data.get(1) > 0x00 && data.get(1) < 0x1E)) {
							handlePacketBody(session, data, output);
						}
						// SWG Packet
						else {
							output.write(data);
						}
					}
				}	
				break;
			}
			
			case 9:
			{
				short sequence = packetBody.getShort();
				Short expectedIn = (Short)session.getAttribute("expectedInValue");
				//if(sequence == expectedIn) {
					sendAck(session, sequence);
					++expectedIn;
					session.setAttribute("expectedInValue", expectedIn);
					DataChannelA dataA = new DataChannelA(packetBody.array());
					for (IoBuffer messageData : dataA.getMessages()) {
						if (messageData != null) {
							messageData.flip();
							output.write(messageData);
						}
					}		
				//} else {
				//	sendOutofOrder(session, sequence);
				//}
				break;
			}
		
			case 1:
			{
				NioDatagramAcceptor nio = ((NetworkDispatch) session.getHandler()).getServer().getNioacceptor();
				NetworkDispatch dispatch = (NetworkDispatch) session.getHandler();
				try {
					if((int) nio.getClass().getMethod("getManagedSessionCount", null).invoke(nio, null) > Integer.parseInt(maxSessions)) {
						//System.out.println("Exceeded max sessions");
						return;
					}
				} catch (IllegalAccessException
						| IllegalArgumentException | InvocationTargetException
						| NoSuchMethodException | SecurityException e) {
					e.printStackTrace();
				}
				int crcLength = packetBody.getInt();
				int connectionId = packetBody.getInt();
				int clientUDPSize = packetBody.getInt();
				session.setAttribute("crcLength", crcLength);
				session.setAttribute("connectionId", connectionId);
				session.setAttribute("clientUDPSize", clientUDPSize);
				session.setAttribute("CRC", 0xDEADBABE);

				SOEPacket packet = new SOEPacket((short)2, 15);
				packet.putNetInt(connectionId);
				packet.putNetInt(0xDEADBABE);
				packet.put((byte)2);
				packet.put((byte)1);
				packet.put((byte)4);
				packet.putNetInt(496);
					
				session.write(packet.flip());
				packetBody.position(0);
				output.write(packetBody);
				Config config = new Config();
				config.setFilePath("nge.cfg");
				if(!(config.loadConfigFile()))
				{
					config = DefaultConfig.getConfig();
				}
					
				int galaxyId = config.getInt("GALAXY_ID");
					
					

				if(dispatch.isZone()) {
					LoginServerId loginServerId = new LoginServerId(galaxyId);
					LoginServerString loginServerString = new LoginServerString("ConnectionServer:" + galaxyId);
					session.write(loginServerString.serialize());
					session.write(loginServerId.serialize());
				} else {
					LoginServerId loginServerId = new LoginServerId(galaxyId);
					LoginServerString loginServerString = new LoginServerString("LoginServer:" + galaxyId);
					session.write(loginServerString.serialize());
					session.write(loginServerId.serialize());
				}

				break;
			}
		
			case 5:
			{
				NetworkDispatch dispatch = (NetworkDispatch) session.getHandler();
				NGECore core = dispatch.getCore();
				
				if(dispatch.isZone()) {
					core.simulationService.handleDisconnect(session);
					session.close(true);
					return;
				}
				
				core.removeClient(session);
				session.close(true);
				
				break;
			}
			
			case 6:
			{
				//SOEPacket packet = new SOEPacket((short)6, 2);
				//session.write(packet.flip());
				break;
			}
			
			case 7:
			{
				/*SOEPacket packet = new SOEPacket((short)8, 41);
				packet.getData().fill(38);
				packet.put((byte) 1);
				packet.putNetShort((short) 0);
				session.write(packet.flip());*/
				packetBody.position(0);
				NetStatsClient stats = new NetStatsClient();
				stats.deserialize(packetBody);
				
				NetStatsServer response = new NetStatsServer(stats.getClientTickCount(), stats.getClientTickCount(), stats.getClientPacketsSent(), stats.getClientPacketsReceived(), (Long) session.getAttribute("sent") + 1, (Long) session.getAttribute("recieved") + 1);
				session.write(response.serialize());

				break;
			}
			
			case 13:
			{
				short sequence = packetBody.getShort();
				Short expectedIn = (Short)session.getAttribute("expectedInValue");
				//if(sequence == expectedIn) {
					sendAck(session, sequence);
					
					@SuppressWarnings("unchecked")
					ArrayList<IoBuffer> fragments = (ArrayList<IoBuffer>) session.getAttribute("currentFragments");
					
					int remainingFragmentSize = (Integer) session.getAttribute("remainingFragmentSize");
					
					if(fragments.size() == 0) {
						remainingFragmentSize = packetBody.getInt();
						session.setAttribute("totalFragmentSize", remainingFragmentSize);
					}
					
					remainingFragmentSize -= packetBody.remaining();
					fragments.add(packetBody.getSlice(packetBody.remaining()));
					
					if(remainingFragmentSize <= 0) {
						
						int totalFragmentSize = (Integer) session.getAttribute("totalFragmentSize");
						IoBuffer addedData = bufferPool.allocate(totalFragmentSize, false);
						//IoBuffer addedData = IoBuffer.allocate(totalFragmentSize, false);
						
						for(IoBuffer fragment : fragments) {
							addedData.put(fragment);
						}
						
						//handlePacketBody(session, addedData, output);
						output.write(addedData);
						fragments.clear();
						remainingFragmentSize = 0;
					}
					
					session.setAttribute("remainingFragmentSize", remainingFragmentSize);
				//} else {
				//	sendOutofOrder(session, sequence);
				//}
				
				
				
				break;
			}
			
			case 17:
			{
				short sequence = packetBody.getShort();
				Integer lastAcknowledgedSequence = (Integer) session.getAttribute("lastAcknowledgedSequence");

				System.out.println("OOO incoming for sequence : " + sequence + " last ACK sequence: " + lastAcknowledgedSequence.shortValue());

				if((Boolean) session.getAttribute("isOutOfOrder") == false) {
					session.setAttribute("isOutOfOrder", new Boolean(true));
					session.setAttribute("oooTimestamp", new Date());
				}
					
				//if(sequence < (lastAcknowledgedSequence - 1))
				//	return;
				@SuppressWarnings("unchecked")
				Map<Short, byte[]> resentPackets = ((Map<Short, byte[]>) session.getAttribute("resentPackets"));

				@SuppressWarnings("unchecked")
				Map<Short, byte[]> sentPackets = ((Map<Short, byte[]>) session.getAttribute("sentPackets"));
				
				Date oooTimestamp = (Date) session.getAttribute("oooTimestamp");
				
				if(oooTimestamp != null && new Date().getTime() - oooTimestamp.getTime() > 5000)
					resentPackets.clear();

			/*	if((sequence - lastAcknowledgedSequence) > 300) {
					System.out.println("Sequence difference too big, disconnecting client, ooo sequence: " + sequence + " last ACK sequence: " + lastAcknowledgedSequence);
					session.close(true);
					return;
				}
				*/
				
				synchronized(sentPackets) {
					Iterator<Short> it = sentPackets.keySet().iterator();
					while(it.hasNext()) {
						Short e = it.next();
						if(e == sequence) {
							it.remove();
							continue;
						}
						if(e < lastAcknowledgedSequence)
							continue;
						if(resentPackets.containsKey(e))
							continue;
						if(e <= sequence) {
							session.write(sentPackets.get(e));
							resentPackets.put(e, sentPackets.get(e));
						}

				}

					/*for(Entry<Short, byte[]> e : sentPackets.entrySet()) {
						if(e.getKey() == sequence) {
							sentPackets.remove(e.getKey());
							continue;
						}
						if(e.getKey() < lastAcknowledgedSequence)
							continue;
						if(resentPackets.containsKey(e.getKey()))
							continue;
						if(e.getKey() <= sequence) {
							session.write(e.getValue());
							resentPackets.put(e.getKey(), e.getValue());
						}
					}*/
					
				}
				
				break;
			}
			
			case 21:
			{
				short sequence = packetBody.getShort();
				session.setAttribute("lastAcknowledgedSequence", new Integer(sequence));
				session.setAttribute("isOutOfOrder", new Boolean(false));
				session.setAttribute("oooTimestamp", null);
				packetBody.position(0);
				@SuppressWarnings("unchecked")
				Map<Short, byte[]> resentPackets = ((Map<Short, byte[]>) session.getAttribute("resentPackets"));
				resentPackets.clear();
				
				@SuppressWarnings("unchecked")
				Map<Short, byte[]> sentPackets = ((Map<Short, byte[]>) session.getAttribute("sentPackets"));

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
				
				
				//System.out.println("Send Packets stored: " + sentPackets.size());
				
				break;
			}
			
			default:
			{
				System.out.println("Test!!!");
			}
		}
	}
	
	@Override
	public void decode(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) throws Exception {

		if (MINAServer.debugOutput) {
			int length = Utilities.getActiveLengthOfBuffer(buffer);
			byte [] newData = new byte[length];
			System.arraycopy(buffer.array(), 0, newData, 0, length);
			System.out.println("IN:  " + Utilities.getHexString(newData));
		}
		buffer.position(0);
		//int bodySize = buffer.remaining()-3;
		//byte[] calculatedCrc = Utilities.intToByteArray(CRC.memcrc(buffer.array(), 0, bodySize+1, 0xDEADBABE));
		
		//int crcSeed = ((Integer) session.getAttribute("crcSeed")).intValue();
		buffer.mark();
		short opcode = buffer.getShort();
		buffer.reset();
		if ((Integer) session.getAttribute("CRC") == 0 && opcode == 1) {
			handlePacketBody(session, buffer, decoderOutput);
		} else if((Integer) session.getAttribute("CRC") == 0 && opcode != 1) {
			//System.out.println("Skipped packet!!!");
			buffer.skip(buffer.remaining());
			return;
		} else {
			buffer.position(0);
			int length = Utilities.getActiveLengthOfBuffer(buffer);

			byte [] newData = new byte[length];
			System.arraycopy(buffer.array(), 0, newData, 0, length);
			
			newData = disassemble(newData, 0xDEADBABE);
			buffer.clear();
			buffer.setAutoExpand(true);
			buffer.position(0);
			buffer.put(newData);
			buffer.flip();
			
			buffer.position(0);
			//System.out.println("IN: " + Utilities.getHexString(buffer.array()));

			if(!buffer.hasRemaining())
				return;

			if(buffer.array().length >= 4)
				handlePacketBody(session, buffer, decoderOutput);
			
		}
		session.setAttribute("recieved", (Long) session.getAttribute("recieved") + 1);
		buffer.skip(buffer.remaining());
		
	}
	
	
	
	public byte[] disassemble(byte[] data, int crcSeed) {
		
		return messageCompression.decompress(
				messageEncryption.decrypt(
						messageCRC.validate(data, crcSeed), crcSeed));
		
	}
	
	@Override
	public void dispose(IoSession arg0) throws Exception {
		
	}

	@Override
	public void finishDecode(IoSession arg0, ProtocolDecoderOutput arg1) throws Exception {
		
	}

}
