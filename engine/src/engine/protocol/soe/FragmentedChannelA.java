package engine.protocol.soe;

import java.nio.ByteBuffer;
import java.util.Vector;

import org.apache.mina.core.buffer.IoBuffer;

import engine.protocol.swg.SWGMessage;


public class FragmentedChannelA extends SOEMessage implements ICombinable, ISequenced {

	private short sequence;
	private int length;
	private IoBuffer swgBuffer;
	
	public FragmentedChannelA() { }
	public FragmentedChannelA(byte[] data) {
		
		super(data);
		if(data.length < 8)
			return;
		ByteBuffer buffer = ByteBuffer.wrap(data);
		sequence = buffer.getShort(2);
		length = buffer.getInt(4);
		
	}
	
	public FragmentedChannelA[] create(SWGMessage message) {
		return create(message.serialize().array());
	}
	
	public FragmentedChannelA [] create(byte [] message) {
		ByteBuffer buffer = ByteBuffer.wrap(message);
		Vector<FragmentedChannelA> fragChannelAs = new Vector<FragmentedChannelA>();
		
		while (buffer.remaining() > 0)
			fragChannelAs.add(createSegment(buffer));
		
		return fragChannelAs.toArray(new FragmentedChannelA[fragChannelAs.size()]);
	}
	
	private FragmentedChannelA createSegment(ByteBuffer buffer) {
		ByteBuffer message = ByteBuffer.allocate(Math.min(buffer.remaining() + 4, 493));
		
		message.putShort((short)13);
		message.putShort((short)0);
		if (buffer.position() == 0)
			message.putInt(buffer.capacity());
		byte[] messageData = new byte[message.remaining()];
		buffer.get(messageData, 0, message.remaining());
		
		message.put(messageData);
		
		return new FragmentedChannelA(message.array());
	}
	
	public void deserialize(IoBuffer data) {
		
	}
	
	public byte[] serialize(short sequence) { 
		
		ByteBuffer message = ByteBuffer.wrap(data);
		
		message.putShort(2, sequence++);
		data = message.array();
		return message.array();
		
	}

	public int getSize() { return data.length; }
	
	public short getSequence() { return sequence; }
	
	public FragmentedChannelA addData(byte[] data) {
		
		ByteBuffer result = ByteBuffer.wrap(data);
		int start = 4;
		int length;
		
		if (this.data == null)
		{
			
			this.data = new byte[] { };
			this.sequence = result.getShort(2);
			this.length = result.getInt(4);
			start = 8;
			
		}
		
		length = data.length - start;
		result = ByteBuffer.allocate(this.data.length + length);
		result.put(this.data);
		result.put(data, start, length);
		
		this.data = result.array();
		sequence++;
		
		return this;
		
	}
	public boolean isComplete() { return length == data.length; }
	public int getOpcode() { return 0x0D; }
	
	public void setSequence(short sequence) {
		this.sequence = sequence;		
	}
	
	public IoBuffer serialize() {
		IoBuffer message = IoBuffer.wrap(data);
		if(data.length < 2)
			return message;
		message.putShort(2, sequence);
		data = message.array();
		return message;
	}
	
	public IoBuffer getSWGData() {
		return swgBuffer = IoBuffer.wrap(data);
	}
}
