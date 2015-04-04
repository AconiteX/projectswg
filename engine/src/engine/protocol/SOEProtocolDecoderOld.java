package engine.protocol;

import java.nio.ByteOrder;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Random;
import java.util.TreeMap;
import java.util.Map.Entry;


import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;

import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;
import engine.protocol.soe.DataChannelA;
import engine.protocol.soe.FragmentedChannelA;
import engine.protocol.soe.MultiProtocol;
import engine.protocol.soe.Ping;
import engine.protocol.soe.SessionRequest;
import engine.protocol.soe.SessionResponse;
import engine.resources.common.Utilities;
import engine.resources.config.Config;
import engine.resources.config.DefaultConfig;
import engine.servers.MINAServer;

public class SOEProtocolDecoderOld implements ProtocolDecoder {
	
	private MessageCompression messageCompression;
	private MessageEncryption messageEncryption;
	private MessageCRC messageCRC;
	private Random crcGenerator = new Random();
	private Map<IoSession, FragmentedChannelA> fragmentedMessages;
	
	SOEProtocolDecoderOld(MessageCompression messageCompression, MessageEncryption messageEncryption, MessageCRC messageCRC) {
		
		this.messageCompression = messageCompression;
		this.messageEncryption = messageEncryption;
		this.messageCRC = messageCRC;
		
		
	}
	
	@Override
	public void decode(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) throws Exception {

		IoBuffer workingData;
		if (MINAServer.debugOutput) {
			int length = Utilities.getActiveLengthOfBuffer(buffer);
			byte [] newData = new byte[length];
			System.arraycopy(buffer.array(), 0, newData, 0, length);
			System.out.println("IN:  " + Utilities.getHexString(newData));
		}
		
		int crcSeed = ((Integer) session.getAttribute("crcSeed")).intValue();
		
		short opcode = buffer.order(ByteOrder.BIG_ENDIAN).getShort(0);
		
		if (crcSeed == 0 && opcode == 1) {
			workingData = buffer;
			handleSOEPacket(session, workingData, decoderOutput);
		} else if(crcSeed != 0) {
			int length = Utilities.getActiveLengthOfBuffer(buffer);
			byte [] newData = new byte[length];
			System.arraycopy(buffer.array(), 0, newData, 0, length);
			
			byte [] packet = disassemble(newData, crcSeed);
			workingData = IoBuffer.allocate(packet.length);
			workingData.clear();
			workingData.setAutoExpand(true);
			workingData.put(packet);
			
			workingData.flip();

			handleSOEPacket(session, workingData, decoderOutput);
			
		}
		
		buffer.skip(buffer.remaining());
		
	}
	
	
	
	public byte[] disassemble(byte[] data, int crcSeed) {
		
		return messageCompression.decompress(
				messageEncryption.decrypt(
						messageCRC.validate(data, crcSeed), crcSeed));
		
	}
	
	public void handleSOEPacket(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		switch(buffer.array()[1]) {
			
			case 1: handleSessionRequest(session, buffer); break;
			case 3: handleMultiPacket(session, buffer, decoderOutput); break;
			case 5: handleDisconnect(session); break;
			case 6: handlePing(session); break;
			case 7: handleNetStats(session, buffer); break;
			case 9: handleDataA(session, buffer, decoderOutput); break;
			case 13: handleFragA(session, buffer, decoderOutput); break;
			//case 17: handleOutOfOrderA(session, buffer, decoderOutput); break;
			case 21: handleAckA(session, buffer, decoderOutput); break;
			
		}
		
		
		
	}
	
	@SuppressWarnings("unchecked")
	private void handleAckA(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		if(!buffer.hasRemaining())
			return;
		
		short sequence = buffer.getShort(2);
		
		TreeMap<Short, IoBuffer> sentPackets = (TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets");
		synchronized(sentPackets) {
			
			Iterator<Short> iterator = sentPackets.keySet().iterator();
			
			while(iterator.hasNext()) {
				
				Short sequence2 = iterator.next();
				
				if(sequence2 <= sequence) {
					
					iterator.remove();
					
				}
				
				else {
					
					break;
					
					
				}
			}
		
		}
		
		decoderOutput.write(buffer);	// write this to decoder output otherwise server thinks its getting spammed with this packet and overflows
		
	}
	
	
	@SuppressWarnings({ "unused", "unchecked" })
	private void handleOutOfOrderA(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		System.out.println("test");
		
		synchronized((TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets")) {
			
			TreeMap<Short, IoBuffer> sentPackets = (TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets");

			for(Entry<Short, IoBuffer> sequence2 : sentPackets.entrySet()) {
				
				session.write(sequence2.getValue());
				
				
			}
		
		}
		
		decoderOutput.write(buffer); // write this to decoder output otherwise server thinks its getting spammed with this packet and overflows
		
	}
	
	
	private void handleFragA(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		
		
		FragmentedChannelA fragA = new FragmentedChannelA(buffer.array());
		/*if(fragA.getSequence() > (Short) session.getAttribute("nextClientSequence")) {
			OutOfOrder ooo = new OutOfOrder(fragA.getSequence());
			session.write(ooo.serialize());
			return;
		}*/
		acknowledgedMessage(session, fragA.getSequence());
		
		
		if (fragmentedMessages == null)
			fragmentedMessages = new HashMap<IoSession, FragmentedChannelA>();
		
		if (!fragmentedMessages.containsKey(session))
			fragmentedMessages.put(session, new FragmentedChannelA().addData(fragA.getData()));
		else {
			
			FragmentedChannelA fragMessage = fragmentedMessages.get(session);
			fragMessage.addData(fragA.getData());
			if (fragMessage.isComplete())
			{
				decoderOutput.write(fragMessage.getSWGData());
				fragmentedMessages.remove(session);
				
			}
			
		}
		
		
	}
	
	
	private void handleDataA(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		DataChannelA dataA = new DataChannelA(buffer.array());
		/*if(dataA.getSequence() > (Short) session.getAttribute("nextClientSequence")) {
			System.out.println("test");
			OutOfOrder ooo = new OutOfOrder(dataA.getSequence());
			session.write(ooo.serialize());
			return;
		}*/
		acknowledgedMessage(session, dataA.getSequence());
		for (IoBuffer messageData : dataA.getMessages()) {
			if (messageData != null) {
				if (messageData.array().length == 0) return;
				decoderOutput.write(messageData);
			}
		}		
	}
	
	
	private void acknowledgedMessage(IoSession session, short sequence) {
		
		//AcknowledgementA ack = new AcknowledgementA(sequence);
		//session.write(ack.serialize());
		
		
	}
	
	
	private void handleNetStats(IoSession session, IoBuffer buffer) {
		
		/*NetStatsClient stats = new NetStatsClient(buffer.array());
		stats.deserialize();
		
		NetStatsServer response = new NetStatsServer(stats.getClientTickCount(), stats.getClientTickCount(), stats.getClientPacketsSent(), stats.getClientPacketsReceived(), (Long) session.getAttribute("sent"), (Long) session.getAttribute("recieved"));
		session.write(response.serialize());*/
		
	}
	
	
	private void handlePing(IoSession session) {
		
		Ping pong = new Ping();
		session.write(pong.serialize());
		
	}
	
	
	private void handleDisconnect(IoSession session) {
		
		session.close(true);
	}
	
	
	private void handleMultiPacket(IoSession session, IoBuffer buffer, ProtocolDecoderOutput decoderOutput) {
		
		MultiProtocol multi = new MultiProtocol(buffer.array());
		
		
		for (IoBuffer data : multi.getMessages()) {
			if (data != null && data.hasRemaining()) {
				// SOE Packet
				if ((data.array()[0] == 0x00 && data.array()[1] > 0x00 && data.array()[1] < 0x1E)) {
					handleSOEPacket(session, data, decoderOutput);
				}
				// SWG Packet
				else {
					decoderOutput.write(data);
				}
				
			}
		}	
		
		
	}
	
	
	private void handleSessionRequest(IoSession session, IoBuffer buffer) {
		
		if(buffer.hasRemaining()) {
			SessionRequest request = new SessionRequest();
			request.deserialize(buffer);
			session.setAttribute("crcSeed", (Integer) crcGenerator.nextInt());
			
			session.setAttribute("connectionId", request.getConnectionId());
			SessionResponse response = new SessionResponse(
					((Integer)session.getAttribute("connectionId")).intValue(),
					(Integer) session.getAttribute("crcSeed"),
					2,
					0x4010,
					496);
			
			session.write(response.serialize());
			
			Config config = new Config();
			config.setFilePath("nge.cfg");
			if(!(config.loadConfigFile()))
			{
				config = DefaultConfig.getConfig();
			}
			
			/*
			int galaxyId = config.getInt("GALAXY_ID");
			
			LoginServerId loginServerId = new LoginServerId(galaxyId);
			LoginServerString loginServerString = new LoginServerString("LoginServer:" + galaxyId);
			
			session.write(loginServerString);
			session.write(loginServerId);*/
			

		}
	}
	
	@Override
	public void dispose(IoSession arg0) throws Exception {
		
	}
	@Override
	public void finishDecode(IoSession arg0, ProtocolDecoderOutput arg1) throws Exception {
		
		
	}
	
}
