package engine.protocol.packager;

import java.util.Map;
import java.util.Vector;

import org.apache.mina.core.buffer.CachedBufferAllocator;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;

import engine.protocol.soe.DataChannelA;
import engine.protocol.soe.FragmentedChannelA;
import engine.protocol.soe.MultiProtocol;
import engine.resources.common.Utilities;

@SuppressWarnings("unused")
public class MessagePackager {
	
	private MessageCRC messageCRC;
	private MessageEncryption messageEncryption;
	private MessageCompression messageCompression;
	private CachedBufferAllocator bufferPool;
	private Vector<IoBuffer> soeMessages = new Vector<IoBuffer>();
	private Vector<IoBuffer> swgMessages = new Vector<IoBuffer>();
	private Vector<byte[]> outgoingMessages = new Vector<byte[]>();
	
	public MessagePackager(CachedBufferAllocator bufferPool) {
		this.bufferPool = bufferPool;
	}
	
	public Vector<byte[]> assemble(IoBuffer[] messages, IoSession connection, int crcSeed)
	{
		{
			createDependencies();
			
			
			for (int i = 0; i < messages.length; i++) {
				if (!Utilities.IsSOETypeMessage(messages[i].array())) 
					swgMessages.add(messages[i]);
				else
					soeMessages.add(messages[i]);
			}
			if(!swgMessages.isEmpty()) {

				DataChannelA dataChannelA = new DataChannelA();
				for (IoBuffer message : swgMessages) {
					if (message == null) continue;
					if (message.array().length < 6) continue;
					int opcode = message.getInt(2);
					if(opcode == 0x1B24F808 || opcode == 0xC867AB5A)
						continue;
					if (!dataChannelA.addMessage(message) && message.array().length <= 487) {
						soeMessages.add(dataChannelA.serialize());
						dataChannelA = new DataChannelA();
						dataChannelA.addMessage(message);
					} else if(message.array().length > 487) {
						if (dataChannelA.hasMessages()) {
							soeMessages.add(dataChannelA.serialize());
							dataChannelA = new DataChannelA();
						}
						FragmentedChannelA fragChanA = new FragmentedChannelA();
						for (FragmentedChannelA fragChanASection : fragChanA.create(message.array())) {
							soeMessages.add(fragChanASection.serialize());
						}
					}
					
				}
				if (dataChannelA.hasMessages()) 
					soeMessages.add(dataChannelA.serialize());
				/*for (IoBuffer message : swgMessages) {
					int opcode = message.getInt(2);
					if (message.array().length > 487 && opcode != 0x1B24F808 && opcode != 0xC867AB5A) {
						FragmentedChannelA fragChanA = new FragmentedChannelA();
						for (FragmentedChannelA fragChanASection : fragChanA.create(message.array())) {
							soeMessages.add(fragChanASection.serialize());
						}
					}
				}*/
				MultiProtocol multiProtocol = new MultiProtocol();
				for (IoBuffer message : swgMessages) {
					if (message == null) continue;
					if (message.array().length < 6) continue;
					int opcode = message.getInt(2);
					if(opcode != 0x1B24F808 && opcode != 0xC867AB5A)
						continue;
					if(message.array().length > 255)
						continue;
					if(!multiProtocol.addSWGMessage(message)) {
						 soeMessages.add(multiProtocol.serialize());
						 multiProtocol = new MultiProtocol();
						 multiProtocol.addSWGMessage(message); 
					}
					
				}
				 if (multiProtocol.hasMessages())
					 soeMessages.add(multiProtocol.serialize());
			}
			
			/* MultiProtocol multiProtocol = new MultiProtocol(); 
			 for (int i = 0; i < soeMessages.size(); i++) { 
				 if (!(soeMessages.get(i) instanceof ICombinable) || soeMessages.get(i).getSize() > 255 || soeMessages.get(i) instanceof FragmentedChannelA) continue; 
				 if(!multiProtocol.addMessage(soeMessages.get(i))) {
					 soeMessages.add(multiProtocol); 
					 multiProtocol = new MultiProtocol();
					 multiProtocol.addMessage(soeMessages.get(i)); 
				 }
				 soeMessages.remove(i--); 
			 } 
			 if (multiProtocol.hasMessages())
				 soeMessages.add(multiProtocol);*/
			 
			
			for (int i = 0; i < soeMessages.size(); i++) {
				
				/*if (soeMessages.get(i) instanceof DataChannelA || soeMessages.get(i) instanceof FragmentedChannelA)
				{
					if(connection.getSequence() > 0 && !(connection.getSequence() == 1)) {
						connection.setSequence((short) (connection.getSequence() + 1));
					}
					((ISequenced) soeMessages.get(i)).setSequence((connection.getSequence()));
					connection.addPacket(soeMessages.get(i));
					
				}*/
				soeMessages.get(i).position(0);
				if (soeMessages.get(i).get(1) == 9 || soeMessages.get(i).get(1) == 13) {
					short sequence = ((Integer) connection.getAttribute("nextOutValue")).shortValue();
					soeMessages.get(i).putShort(2, sequence);
					connection.setAttribute("nextOutValue", (Integer) connection.getAttribute("nextOutValue") + 1);
					soeMessages.get(i).flip();
					byte[] packet = messageCRC.append(
										messageEncryption.encrypt(
												messageCompression.compress(soeMessages.get(i).array()), crcSeed), crcSeed);
					outgoingMessages.add(packet);
					((Map<Short, byte[]>) connection.getAttribute("sentPackets")).put(new Short(sequence), packet);
				} else {
					soeMessages.get(i).flip();
					byte[] packet = messageCRC.append(
										messageEncryption.encrypt(
												messageCompression.compress(soeMessages.get(i).array()), crcSeed), crcSeed);
					outgoingMessages.add(packet);
				}
				
					//if (soeMessages.get(i) instanceof FragmentedChannelA) System.out.println("FragA Sequence" + soeMessages.get(i).getSequence() + "should be: " + connection.getSequence());
					/*
					 * if(soeMessages.get(i) instanceof DataChannelA)
					 * System.out.println("DataA Sequence" +
					 * soeMessages.get(i).getSequence());
					 */
				}
				
				/*if (soeMessages.get(i) instanceof MultiProtocol && ((MultiProtocol) soeMessages.get(i)).getSequencedmessages() != null) {
					
					Vector<SOEMessage> Sequencedmessages = ((MultiProtocol) soeMessages.get(i)).getSequencedmessages();
					for (int j = 0; j < Sequencedmessages.size(); j++) {
						if (Sequencedmessages.get(j) instanceof ISequenced) {
							byte[] packet2 = messageCRC.append(messageEncryption.encrypt(messageCompression.compress(Sequencedmessages.get(j).serialize(connection.getSequence())), crcSeed), crcSeed);
							connection.addPacket(packet2, connection.getSequence());
							((ISequenced) Sequencedmessages.get(j)).setSequence((connection.getSequence()));
							connection.setSequence((short) (connection.getSequence() + 1));
						}
					}
					Sequencedmessages.clear();
					Sequencedmessages = null;
					((MultiProtocol) soeMessages.get(i)).Sequencedmessages.clear();
					((MultiProtocol) soeMessages.get(i)).Sequencedmessages = null;
				}
				
			}*/
			soeMessages.clear();
			swgMessages.clear();
			return outgoingMessages;
		}
		
	}
	
	public byte[] disassemble(byte[] data, int crcSeed) {
		createDependencies();
		
		if (crcSeed == 0 && data[0] == 0x00 && data[1] == 0x01){
			return data;
		}
		
		else if (crcSeed == 0 && data[0] != 0x00 && data[1] != 0x01){
			return data = new byte[] { 0x00 };
		}
		
		else {
			return messageCompression.decompress(messageEncryption.decrypt(messageCRC.validate(data, crcSeed), crcSeed));
			
		}
	}
	
	private void createDependencies() {
		
		if (messageCRC == null) messageCRC = new MessageCRC();
		
		if (messageEncryption == null) messageEncryption = new MessageEncryption();
		
		if (messageCompression == null) messageCompression = new MessageCompression();
				
	}
	
}
