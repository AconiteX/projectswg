package engine.protocol.soe;

import java.nio.ByteBuffer;

import protocol.Message;


public abstract class SOEMessage extends Message {

	public SOEMessage(byte[] data) {
		
		super(data);
		opcode = ByteBuffer.wrap(data).getShort();
		
	}
	public SOEMessage() { 
		
//		super(new byte[] { }); 
		
	}
	
	public short getSequence() {
		
		return ByteBuffer.wrap(data).getShort(2);
		
	}
	
	
}
