package engine.protocol;

import java.nio.ByteBuffer;

import org.apache.mina.core.buffer.IoBuffer;

import protocol.Message;

public class UnknownMessage extends Message {

	private byte[] data;
	
	public UnknownMessage(byte[] data) {
		
		String dataString = "";
		for (byte currentByte: data) dataString += String.format("%02X", currentByte)  + " ";
		
		//System.out.println("UNKNOWN: Message data: " + dataString);
		
		try {
			
			this.data = data;
			if (data[0] == 0 && data[1] > 0 && data[1] < 0x1f)
				opcode = data[1];
			else
				opcode = ByteBuffer.wrap(data).getInt(2);
		
		} catch (java.lang.IndexOutOfBoundsException e) {
			
		}
		
	}
	
	public byte[] getData() { return data; }
	public int getOpCode() { return opcode; }
	public byte[] serialize(short[] sequence) { return new byte[] { }; }

	public void deserialize(IoBuffer data) {
		
	}
	
	public IoBuffer serialize() {
		return IoBuffer.allocate(0);
	}



}
