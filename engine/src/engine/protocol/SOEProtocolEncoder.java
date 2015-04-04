package engine.protocol;

import java.util.HashMap;
import java.util.Map;
import java.util.TreeMap;
import java.util.Vector;

import org.apache.mina.core.buffer.CachedBufferAllocator;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoder;
import org.apache.mina.filter.codec.ProtocolEncoderException;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;

import resources.common.Opcodes;
import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;
import engine.protocol.soe.FragmentedChannelA;
import engine.protocol.unitTests.AbstractUnitTest;
import engine.resources.common.Utilities;
import engine.resources.objects.Delta;
import engine.resources.service.*;

@SuppressWarnings("unused")
public class SOEProtocolEncoder implements ProtocolEncoder {

	private MessageCompression messageCompression;
	private MessageEncryption messageEncryption;
	private MessageCRC messageCRC;
	private Map<IoSession, Vector<IoBuffer>> queue;
	private CachedBufferAllocator bufferPool;
	private static Map<Integer, AbstractUnitTest> unitTests = null;
	private static boolean enableUnitTesting = false;

	SOEProtocolEncoder() {
		this.messageCompression = new MessageCompression();
		this.messageEncryption = new MessageEncryption();
		this.messageCRC = new MessageCRC();
		bufferPool = new CachedBufferAllocator();
	}
	
	@Override
	public void dispose(IoSession arg0) throws Exception {}

	@Override
	public void encode(IoSession session, Object input, ProtocolEncoderOutput output) throws Exception {
		if(input instanceof IoBuffer) {
			if (((IoBuffer) input).position() > 0) {
				System.err.println("Buffer isn't flipped with buffer.flip().");
				throw new ProtocolEncoderException();
			}
			
			IoBuffer buffer = Delta.createBuffer(((IoBuffer) input).array().length).put(((IoBuffer) input).array()).flip();
			
			if (enableUnitTesting && buffer != null && buffer.array().length >= 6) {
				int opcode = buffer.skip(2).getInt();
				
				if (getUnitTests() != null && getUnitTests().containsKey(opcode) && getUnitTests().get(opcode) != null) {
					if (!getUnitTests().get(opcode).validate(buffer)) {
						String additional = Integer.toHexString(opcode);
						buffer.flip();
						
						try {
							switch (opcode) {
								case Opcodes.BaselinesMessage:
									additional = "Baseline " + new String(buffer.array(), 14, 4, "US-ASCII") + buffer.skip(18).get();
									break;
								case Opcodes.DeltasMessage:
									additional = "Delta " + new String(buffer.array(), 14, 4, "US-ASCII") + buffer.skip(18).get();
									break;
								case Opcodes.ObjControllerMessage:
									additional = "ObjController " + Integer.toHexString(buffer.skip(10).getInt());
									break;
							}
						} catch (Exception e) {
							System.err.println("Packet size invalid (couldn't reach sub-opcode for baseline or objcontroller).");
						}
						
						System.err.println("Failed packet validation: " + additional);
						System.err.println("Stack Trace:");
						try { throw new Exception(); } catch (Exception e) { e.printStackTrace(); }
						throw new ProtocolEncoderException();
					}
				}
			}
			
			((NetworkDispatch) session.getHandler()).queueMessage(session, (IoBuffer) input);
		}
		if(input instanceof SOEPacket) {
			if(((SOEPacket) input).getData().array()[1] == 2 || ((SOEPacket) input).getData().array()[1] == 8) {
				
				if(((SOEPacket) input).getData().array()[1] == 8) {
					byte[] data = ((SOEPacket) input).getData().array();
					data = messageCompression.compress(data);
					IoBuffer buffer = ((SOEPacket) input).getData();
					buffer.clear();
					buffer.setAutoExpand(true);
					buffer.put(data);
					buffer.flip();
				}
				output.write(((SOEPacket) input).getData());
				return;
			}
			assembleMessage(session, ((SOEPacket) input).getData(), output);
		}
		if(input instanceof byte[]) {
			IoBuffer packet = IoBuffer.allocate(((byte[])input).length).put((byte[]) input);
			packet.flip();
			session.setAttribute("sent", (Long) session.getAttribute("sent") + 1);
			output.write(packet);
		}
	}
	
	@SuppressWarnings("unchecked")
	private void assembleMessage(IoSession session, IoBuffer buffer, ProtocolEncoderOutput encoderOutput) {
		
		if(Utilities.IsSOETypeMessage(buffer.array())) {

			byte[] data = buffer.array();
			if(buffer.get(1) == 8)
				data = messageCRC.append(
						messageEncryption.encrypt(messageCompression.compress(data), 0xDEADBABE), 0xDEADBABE);
			else
				data = messageCRC.append(
						messageEncryption.encrypt(data, 0xDEADBABE), 0xDEADBABE);
			buffer.clear();
			buffer.setAutoExpand(true);
			buffer.put(data);
			buffer.flip();
			if(data.length > 0) {
				session.setAttribute("sent", (Long) session.getAttribute("sent") + 1);
				encoderOutput.write(buffer);
			}
			
		} else {
						
			if(buffer.array().length < 6) return;
			
			if(buffer.array().length > 487) {
				
				
				FragmentedChannelA fragChanA = new FragmentedChannelA();
				for (FragmentedChannelA fragChanASection : fragChanA.create(buffer.array())) {
					fragChanASection.setSequence(((Integer) session.getAttribute("nextOutValue")).shortValue());
					session.setAttribute("nextOutValue", (Integer) session.getAttribute("nextOutValue") + 1);
					byte[] data = messageCRC.append(
							messageEncryption.encrypt(messageCompression.compress(fragChanASection.serialize().array()), 0xDEADBABE), 0xDEADBABE);
					buffer.clear();
					buffer.setAutoExpand(true);
					//buffer = IoBuffer.allocate(data.length);
					buffer.put(data);
					buffer.flip();
					if(data.length > 0) {
						encoderOutput.write(buffer);
					}	
					
					((TreeMap<Short, byte[]>) session.getAttribute("sentPackets")).put(((Integer) session.getAttribute("nextOutValue")).shortValue(), buffer.array());

				}
			} else {
				

				/*DataChannelA dataA = new DataChannelA();
				dataA.addMessage(buffer);
				
				dataA.setSequence(((Integer) session.getAttribute("nextOutValue")).shortValue());
				session.setAttribute("nextOutValue", (Integer) session.getAttribute("nextOutValue") + 1);
				byte[] data = messageCRC.append(
						messageEncryption.encrypt(messageCompression.compress(dataA.serialize().array()), 0xDEADBABE), 0xDEADBABE);
				buffer.clear();
				buffer = IoBuffer.allocate(data.length);
				buffer.put(data);
				buffer.flip();
				if(data.length > 0) {
					encoderOutput.write(buffer);
				}
				//TreeMap<Short, IoBuffer> sent = (TreeMap<Short, IoBuffer>) session.getAttribute("sentPackets");


				((TreeMap<Short, byte[]>) session.getAttribute("sentPackets")).put(((Integer) session.getAttribute("nextOutValue")).shortValue(), buffer.array());*/
				
			}
		}
	}
	
	public static Map<Integer, AbstractUnitTest> getUnitTests() {
		if (unitTests == null) {
			unitTests = new HashMap<Integer, AbstractUnitTest>();
		}
		
		return unitTests;
	}
	
}
