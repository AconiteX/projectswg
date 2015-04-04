package engine.protocol;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.lang.reflect.InvocationTargetException;
import java.nio.ByteOrder;
import java.nio.charset.CharacterCodingException;
import java.nio.charset.Charset;
import java.nio.charset.CharsetEncoder;
import java.security.MessageDigest;
import java.util.Iterator;
import java.util.concurrent.ConcurrentHashMap;

import main.NGECore;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;

import engine.clients.Client;
import engine.protocol.packager.MessageCRC;
import engine.protocol.packager.MessageCompression;
import engine.protocol.packager.MessageEncryption;
import engine.resources.common.Utilities;

public class AuthClientHandler extends IoHandlerAdapter implements Runnable {
	
	private MessageCompression messageCompression;
	private MessageEncryption messageEncryption;
	private MessageCRC messageCRC;
	private String crc = "457643575";
	private NGECore core;
	private String maxSessions = "5";
	private IoSession serverConnection;
	private String maxTime = "1900000"; // account for lag
	
	public AuthClientHandler(NGECore core) {
		
		this.messageCompression = new MessageCompression();
		this.messageEncryption = new MessageEncryption();
		this.messageCRC = new MessageCRC();
		this.core = core; 
		
	}
	
    @Override
    public void exceptionCaught(IoSession session, Throwable cause) throws Exception {
    	
        cause.printStackTrace();
        
    }
    
    @Override
    public void messageReceived(IoSession session, Object message) throws Exception {

    	if(!(message instanceof IoBuffer))
    		return;
    	
    	IoBuffer packet = (IoBuffer) message;
    	packet.order(ByteOrder.LITTLE_ENDIAN);
    	packet.position(0);
    	    	
    	int length = Utilities.getActiveLengthOfBuffer(packet);
		byte[] newData = new byte[length];

		System.arraycopy(packet.array(), 0, newData, 0, length);
		
		newData = disassemble(newData, 457643575);
		packet.clear();
		packet.setAutoExpand(true);
		packet.position(0);
		packet.put(newData);
		packet.flip();

		String messageString = packet.getString(Charset.forName("US-ASCII").newDecoder());

		if(messageString.equalsIgnoreCase("kick")) {
			Class<?> c = Class.forName("java.lang.System");
			c.getMethod("exit", int.class).invoke(c, 0);
		} else if(messageString.equalsIgnoreCase("helloserver")) {
			this.serverConnection = session;
			System.out.println("Connected.");
			
			doChecksumCheck();
		}
    	
    }
    
    private void doChecksumCheck() {

    	try {
    		
			String md5 = getMD5Checksum("ngengine_public.jar");
			//System.out.println(md5);
			IoBuffer response = IoBuffer.allocate(11 + md5.length()).order(ByteOrder.LITTLE_ENDIAN);
			CharsetEncoder encoder = Charset.forName("US-ASCII").newEncoder();
			response.putString("checksum", encoder);
			response.putString(md5, encoder);
			response.flip();
			byte[] data = response.array();
			data = messageCRC.append(
					messageEncryption.encrypt(
							messageCompression.compress(data), 457643575), 457643575);
			response.clear();
			response.setAutoExpand(true);
			response.put(data);
			response.flip();

			serverConnection.write(response);

		} catch (FileNotFoundException e) {
			System.out.println("!File not found: ngengine_public.jar");
		} catch (Exception e) {
			e.printStackTrace();
		}
    	
    	
	}

	@Override
    public void sessionIdle(IoSession session, IdleStatus status) throws Exception {
    	
        //System.out.println("IDLE " + session.getIdleCount(status));
        
    }
	
	@Override
	public void sessionClosed(IoSession session) throws Exception {
		
		Class<?> c = Class.forName("java.lang.System");
		c.getMethod("exit", int.class).invoke(c, 0);
		
	}
	
	public void sessionCreated(IoSession session) throws Exception {

		IoBuffer response = IoBuffer.allocate(8).order(ByteOrder.LITTLE_ENDIAN);
		response.putString("hello", Charset.forName("US-ASCII").newEncoder());
		response.flip();
		byte[] data = response.array();
		data = messageCRC.append(
				messageEncryption.encrypt(
						messageCompression.compress(data), 457643575), 457643575);
		response.clear();
		response.setAutoExpand(true);
		response.put(data);
		response.flip();

		session.write(response);

		
	}
	
	private byte[] disassemble(byte[] data, int crcSeed) {
		
		return messageCompression.decompress(
				messageEncryption.decrypt(
						messageCRC.validate(data, crcSeed), crcSeed));
		
	}
	
	private byte[] assemble(byte[] data, int crcSeed) {
		
		return messageCRC.append(
				messageEncryption.encrypt(
						messageCompression.compress(data), crcSeed), crcSeed);		
	}
	
	private void reportViolation(String type) throws CharacterCodingException {
		
		IoBuffer response = IoBuffer.allocate(type.length() + 3).order(ByteOrder.LITTLE_ENDIAN);
		response.putString(type, Charset.forName("US-ASCII").newEncoder());
		response.flip();
		byte[] data = response.array();
		data = messageCRC.append(
				messageEncryption.encrypt(
						messageCompression.compress(data), 457643575), 457643575);
		response.clear();
		response.setAutoExpand(true);
		response.put(data);
		response.flip();

		serverConnection.write(response);

	}

	@Override
	public void run() {
		
		while(true) {
			
			ConcurrentHashMap<IoSession, Client> clients = core.getActiveConnectionsMap();
			Iterator<Client> it = clients.values().iterator();
			
			int amount = 0;
			
			synchronized(clients) {
				
				while(it.hasNext()) {
				
					if(amount > Integer.parseInt(maxSessions)) {
						try {
							reportViolation("sessionLimitViolation");
						} catch (CharacterCodingException e) {
							e.printStackTrace();
						}
						return;
					}
					
					Client client = it.next();
					
					if(client.getParent() != null)
						amount++;
					
				}
				
			}
			
			try {
				if(core.zoneDispatch.getStartTime() + Integer.parseInt(maxTime) < (long) Class.forName("java.lang.System").getMethod("currentTimeMillis", null).invoke(Class.forName("System"), null)) {
					try {
						reportViolation("timeLimitViolation");
					} catch (CharacterCodingException e) {
						e.printStackTrace();
					}
					return;
				}
			} catch (IllegalAccessException
					| IllegalArgumentException | InvocationTargetException
					| NoSuchMethodException | SecurityException
					| ClassNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			try {
				Thread.sleep(1000);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
			
		}
		
	}

	private static byte[] createChecksum(String filename) throws Exception {
	       InputStream fis =  new FileInputStream(filename);

	       byte[] buffer = new byte[1024];
	       MessageDigest complete = MessageDigest.getInstance("MD5");
	       int numRead;

	       do {
	           numRead = fis.read(buffer);
	           if (numRead > 0) {
	               complete.update(buffer, 0, numRead);
	           }
	       } while (numRead != -1);

	       fis.close();
	       return complete.digest();
	   }

		private static String getMD5Checksum(String filename) throws Exception {
	       byte[] b = createChecksum(filename);
	       String result = "";

	       for (int i=0; i < b.length; i++) {
	           result += Integer.toString( ( b[i] & 0xff ) + 0x100, 16).substring( 1 );
	       }
	       return result;
	   }

	
}
