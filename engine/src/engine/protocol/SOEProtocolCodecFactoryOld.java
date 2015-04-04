package engine.protocol;

import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolCodecFactory;
import org.apache.mina.filter.codec.ProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolEncoder;

import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;

public class SOEProtocolCodecFactoryOld implements ProtocolCodecFactory {
	
	private MessageCompression messageCompression;
	private MessageCRC messageCRC;
	private MessageEncryption messageEncryption;
	
	public SOEProtocolCodecFactoryOld() {
		
		messageCRC = new MessageCRC();
		
		messageEncryption = new MessageEncryption();
		
		messageCompression = new MessageCompression();
		
	}
	
	public ProtocolDecoder getDecoder(IoSession session) throws Exception {
		return new SOEProtocolDecoderOld(messageCompression, messageEncryption, messageCRC);
	}

	
	public ProtocolEncoder getEncoder(IoSession session) throws Exception {
		return new SOEProtocolEncoderOld(messageCompression, messageEncryption, messageCRC);
		
	}
	
	

}
