package engine.protocol;

import java.nio.ByteOrder;
import java.nio.charset.Charset;

import org.apache.mina.core.buffer.IoBuffer;

public class SOEPacket {
	
	public SOEPacket(short opcode, int sizePrediction) {
		this.opcode = opcode;
		
		data = IoBuffer.allocate(sizePrediction);
		data.setAutoExpand(true);
		
		putNetShort(opcode);
		if(isSequenced()) {
			putNetShort((short)0xCAFE);
		}
	}
	
	public short getOpcode() {
		return opcode;
	}
	
	public void setOpcode(short opcode) {
		this.opcode = opcode;
	}
	
	public void setSequence(short sequence) {
		data.putShort(2, sequence);
	}
	
	public boolean isSequenced() {
		return opcode == 9 || opcode == 13;
	}
	
	public int position() {
		return data.position();
	}
	
	public IoBuffer getData() {
		return data;
	}
	
	public void setData(IoBuffer data) {
		this.data = data;
	}
	
	public void put(byte value) {
		data.put(value);
	}
	
	public void put(byte[] array, int index, int count) {
		data.put(array, index, count);
	}
	
	public void put(byte[] array) {
		data.put(array);
	}
	
	public void putShort(short value) {
		data.putShort(Short.reverseBytes(value));
	}
	
	public void putInt(int value) {
		data.putInt(Integer.reverseBytes(value));
	}
	
	public void putInt(int index, int value) {
		data.putInt(index, Integer.reverseBytes(value));
	}
	
	public void putLong(long value) {
		data.putLong(Long.reverseBytes(value));
	}
	
	public void putNetShort(short value) {
		data.putShort(value);
	}
	
	public void putNetInt(int value) {
		data.putInt(value);
	}
	
	public void putNetInt(int index, int value) {
		data.putInt(index, value);
	}
	
	public void putNetLong(long value) {
		data.putLong(value);
	}
	
	public void putFloat(float value) {
		data.order(ByteOrder.LITTLE_ENDIAN);
		data.putFloat(value);
		data.order(ByteOrder.BIG_ENDIAN);
	}
	
	public void putNetFloat(float value) {
		data.putFloat(value);
	}
	
	public void putASCII(String s) throws Exception {
		putShort((short)s.length());
		data.putString(s, Charset.forName("US-ASCII").newEncoder());
	}
	
	public void putUTF(String s) throws Exception {
		putInt(s.length());
		data.putString(s, Charset.forName("UTF-16LE").newEncoder());
	}
	
	public void putRawString(String s) throws Exception {
		data.putString(s, Charset.forName("US-ASCII").newEncoder());
	}
	
	public SOEPacket flip() {
		data.flip();
		return this;
	}
	
	private short opcode;
	private IoBuffer data;
}
