package engine.protocol.swg;

import java.nio.ByteBuffer;

import protocol.Message;



public abstract class SWGMessage extends Message {

	protected short	operandCount;
	
	public SWGMessage() {
		
	}
	
	public SWGMessage(byte[] data) { 
		super(data);
		opcode = ByteBuffer.wrap(data).getInt(2);
	}
}
