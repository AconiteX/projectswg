package engine.protocol.soe;

import java.nio.ByteOrder;

import org.apache.mina.core.buffer.IoBuffer;

public class Disconnect extends SOEMessage {

	private int connectionId;

	public Disconnect(int connectionId, int reason) {
		this.connectionId = connectionId;
	}
	
	public byte[] serialize(short sequence) {
		return data.clone();
	}

	public int getSize() {
		return 0;
	}
	
	public void deserialize(IoBuffer data) {
		
	}
	
	public IoBuffer serialize() {

		IoBuffer result = IoBuffer.allocate(8).order(ByteOrder.LITTLE_ENDIAN);
		
		result.putShort((short)5);
		result.putInt(connectionId);
		result.putShort((short)6);
		result.flip();
		return result;

	}

}
