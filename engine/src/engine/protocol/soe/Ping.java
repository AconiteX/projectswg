package engine.protocol.soe;

import org.apache.mina.core.buffer.IoBuffer;

public class Ping extends SOEMessage {

	public Ping() {
		
		
	}
	
	public byte[] serialize(short sequence) {
		
		return data.clone();
	}
	
	public void deserialize(IoBuffer data) {
		
	}
	
	public IoBuffer serialize() {
		IoBuffer result = IoBuffer.allocate(2);
		
		result.putShort((short)6);
		result.flip();
		return result;
	}
}
