package engine.protocol;

import java.util.TreeMap;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoder;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;

import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;
import engine.protocol.soe.DataChannelA;
import engine.protocol.soe.FragmentedChannelA;
import engine.resources.common.Utilities;
import engine.servers.MINAServer;




public class SOEProtocolEncoderOld implements ProtocolEncoder {
	
	private MessageCompression messageCompression;
	private MessageEncryption messageEncryption;
	private MessageCRC messageCRC;
	
	SOEProtocolEncoderOld(MessageCompression messageCompression, MessageEncryption messageEncryption, MessageCRC messageCRC) {
		
		this.messageCompression = messageCompression;
		this.messageEncryption = messageEncryption;
		this.messageCRC = messageCRC;
		
	}
	
	
	@Override
	public void dispose(IoSession arg0) throws Exception {
		// TODO Auto-generated method stub
		
	}
	
	@Override
	public void encode(IoSession session, Object message, ProtocolEncoderOutput encoderOutput) throws Exception {
		
		if(message instanceof IoBuffer) {
			if (MINAServer.debugOutput) {
				System.out.println("OUT: " + Utilities.getHexString(((IoBuffer)message).array()));
			}
			
			if(((IoBuffer) message).array()[1] == 2) {
				encoderOutput.write(message);
				return;
			}
			assembleMessage(session, (IoBuffer) message, encoderOutput);
			
		}
		
	}
	
	
	@SuppressWarnings("unchecked")
	private void assembleMessage(IoSession session, IoBuffer buffer, ProtocolEncoderOutput encoderOutput) {
		
		if(Utilities.IsSOETypeMessage(buffer.array())) {
			
			byte[] data = buffer.array();
			data = messageCRC.append(
					messageEncryption.encrypt(
							data, (Integer) session.getAttribute("crcSeed")), ((Integer) session.getAttribute("crcSeed")));
			buffer.clear();
			buffer.setAutoExpand(true);
			buffer.put(data);
			buffer.flip();
			if(data.length > 0) {
				encoderOutput.write(buffer);
			}
			
		} else {
						
			if(buffer.array().length < 6) return;
			
			if(buffer.array().length > 487) {
				
				

				FragmentedChannelA fragChanA = new FragmentedChannelA();
				for (FragmentedChannelA fragChanASection : fragChanA.create(buffer.array())) {
					fragChanASection.setSequence(((Integer) session.getAttribute("nextServerSequence")).shortValue());
					session.setAttribute("nextServerSequence", (Integer) session.getAttribute("nextServerSequence") + 1);
					byte[] data = fragChanASection.getData();
					data = messageCRC.append(
							messageEncryption.encrypt(
									messageCompression.compress(fragChanASection.serialize().array())
									, (Integer) session.getAttribute("crcSeed"))
									, (Integer) session.getAttribute("crcSeed"));
					buffer.clear();
					buffer.put(data);
					if(data.length > 0) {
						encoderOutput.write(buffer);
					}	
					
					((TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets")).put(((Integer) session.getAttribute("nextServerSequence")).shortValue(), buffer);

				}
			} else {
				

				DataChannelA dataA = new DataChannelA();
				dataA.addMessage(buffer);
				
				dataA.setSequence(((Integer) session.getAttribute("nextServerSequence")).shortValue());
				session.setAttribute("nextServerSequence", (Integer) session.getAttribute("nextServerSequence") + 1);
				dataA.serialize();
				byte[] data = dataA.serialize().array();
				System.out.println(Utilities.getHexString(data));
				
				data = messageCRC.append(
						messageEncryption.encrypt(
								messageCompression.compress(dataA.serialize().array())
								, (Integer) session.getAttribute("crcSeed"))
								, (Integer) session.getAttribute("crcSeed"));
				buffer.clear();
				buffer = IoBuffer.allocate(data.length);
				buffer.put(data);
				buffer.flip();
				if(data.length > 0) {
					encoderOutput.write(buffer);
				}
				//TreeMap<Short, IoBuffer> sent = (TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets");


				((TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets")).put(((Integer) session.getAttribute("nextServerSequence")).shortValue(), buffer);
				
			}
		}
	}
	
	
	
	
	
}
