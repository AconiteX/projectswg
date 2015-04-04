package engine.protocol.unitTests;

import java.util.Map;

import engine.protocol.SOEProtocolEncoder;

public abstract class AbstractUnitTests {
	
	public static Map<Integer, AbstractUnitTest> getUnitTests() {
		return SOEProtocolEncoder.getUnitTests();
	}
	
}
