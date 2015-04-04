package main;

import java.io.IOException;
import java.nio.ByteOrder;
import java.nio.charset.Charset;
import java.nio.charset.CharsetDecoder;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;

import protocol.packager.MessageCRC;
import protocol.packager.MessageCompression;
import protocol.packager.MessageEncryption;
import resources.config.Config;

public class AuthServerHandler extends IoHandlerAdapter {
	@SuppressWarnings("unused")
	private String[] validChecksums;
	private Config config = new Config("config.cfg");
	private DatabaseConnection databaseConnection = new DatabaseConnection();
	private MessageCRC messageCRC = new MessageCRC();
	private MessageEncryption messageEncryption = new MessageEncryption();
	private MessageCompression messageCompression = new MessageCompression();
	
	public AuthServerHandler() {
		this.databaseConnection.connect(this.config.getString("DB.URL"),
				this.config.getString("DB.NAME"),
				this.config.getString("DB.USER"),
				this.config.getString("DB.PASS"), "postgresql");

		try {
			List<String> lines = Files.readAllLines(Paths.get("checksums.txt"),
					Charset.forName("UTF-8"));
			validChecksums = lines.toArray(new String[] {});
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public void exceptionCaught(IoSession session, Throwable cause)
			throws Exception {
	}

	public void messageReceived(IoSession session, Object message)
			throws Exception {
		if (!(message instanceof IoBuffer)) {
			return;
		}
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
		CharsetDecoder decoder = Charset.forName("US-ASCII").newDecoder();
		String messageString = packet.getString(decoder);
		String IP = session.getRemoteAddress().toString().split("/", 2)[1]
				.split(", ", 2)[0].split(":", 2)[0];
		if (messageString.equalsIgnoreCase("hello")) {
			System.out.println(IP + " attempting to connect...");
			if (!checkIP(session, IP)) {
				System.out
						.println(IP
								+ " attempting to connect but is in banlist, sending disconnect.");

				IoBuffer response = IoBuffer.allocate(7).order(
						ByteOrder.LITTLE_ENDIAN);
				response.putString("kick", Charset.forName("US-ASCII")
						.newEncoder());
				response.flip();
				byte[] data = response.array();
				data = this.messageCRC.append(this.messageEncryption.encrypt(
						this.messageCompression.compress(data), 457643575),
						457643575);
				response.clear();
				response.setAutoExpand(true);
				response.put(data);
				response.flip();

				session.write(response);
				return;
			}
			IoBuffer response = IoBuffer.allocate(14).order(
					ByteOrder.LITTLE_ENDIAN);
			response.putString("helloserver", Charset.forName("US-ASCII")
					.newEncoder());
			response.flip();

			byte[] data = response.array();
			data = this.messageCRC.append(
					this.messageEncryption.encrypt(
							this.messageCompression.compress(data), 457643575),
					457643575);
			response.clear();
			response.setAutoExpand(true);
			response.put(data);
			response.flip();

			session.write(response);

			System.out.println(IP + " connected.");
		} else if (messageString.equalsIgnoreCase("bye")) {
			System.out.println(IP + " sent disconnect to server.");

			session.close(false);
		} else if (messageString.equalsIgnoreCase("sessionLimitViolation")) {
			System.out.println(IP + " violated session limit!");

			session.close(false);
		} else if (messageString.equalsIgnoreCase("timeLimitViolation")) {
			System.out.println(IP + " violated uptime limit!");

			session.close(false);
		} else if (messageString.contains("checksum")) {
			String md5 = messageString.split("checksum")[1].toLowerCase();
			System.out.println(md5);
			try {
				List<String> checksums = Files.readAllLines(
						Paths.get("checksums.txt"), Charset.forName("UTF-8"));
				if (!checksums.contains(md5)) {
					System.out
							.println(IP + " has invalid engine md5 checksum!");

					session.close(false);
				}
			} catch (IOException e) {
				e.printStackTrace();
				session.close(false);
			}
		}
	}

	public void sessionIdle(IoSession session, IdleStatus status)
			throws Exception {
		System.out.println("IDLE " + session.getIdleCount(status));
	}

	public void sessionClosed(IoSession session) throws Exception {
	}

	public void sessionCreated(IoSession session) throws Exception {
	}

	public byte[] disassemble(byte[] data, int crcSeed) {
		return this.messageCompression.decompress(this.messageEncryption
				.decrypt(this.messageCRC.validate(data, crcSeed), crcSeed));
	}

	public byte[] assemble(byte[] data, int crcSeed) {
		return this.messageCRC.append(
				this.messageEncryption.encrypt(
						this.messageCompression.compress(data), crcSeed),
				crcSeed);
	}

	public boolean checkIP(IoSession session, String IP) throws SQLException {
		PreparedStatement preparedStatement = this.databaseConnection
				.preparedStatement("SELECT ip FROM ipbans WHERE ip=?");
		preparedStatement.setString(1, IP);
		ResultSet resultSet = preparedStatement.executeQuery();
		if (resultSet.next()) {
			return false;
		}
		return true;
	}
}
