package engine.protocol.soe;

import java.lang.reflect.Array;
import org.apache.mina.core.buffer.IoBuffer;

public class OutOfOrder extends SOEMessage{

	private short sequence;


	public OutOfOrder(short sequence) {
		
		this.sequence = sequence;
		
	}

	public OutOfOrder(byte[] data) { super(data); }
	
	public short getSequence() { return Array.getShort(data, 2); }
	
	public void deserialize(IoBuffer data) {
		
	}
	
	public IoBuffer serialize() {
		IoBuffer result = IoBuffer.allocate(4);
		
		result.putShort((short)17);
		result.putShort((short)sequence);
		
		result.flip();
		return result;
	}
	
	public int getOpcode() { return (short) 17; }
}
