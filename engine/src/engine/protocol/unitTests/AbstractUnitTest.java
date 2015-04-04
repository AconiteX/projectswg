package engine.protocol.unitTests;

import org.apache.mina.core.buffer.IoBuffer;

import engine.resources.common.StringUtilities;

public abstract class AbstractUnitTest {
	
	protected IoBuffer buffer = null;
	
	public AbstractUnitTest() {
		
	}
	
	public abstract boolean isValid(IoBuffer buffer);
	
	public boolean validate(IoBuffer buffer) {
		try {
			return isValid(buffer);
		} catch (Exception e) {
			return false;
		}
	}
	
	protected String getAsciiString(IoBuffer buffer) {
		return StringUtilities.getAsciiString(buffer);
	}
	
	protected String getUnicodeString(IoBuffer buffer) {
		return StringUtilities.getUnicodeString(buffer);
	}
	
}
