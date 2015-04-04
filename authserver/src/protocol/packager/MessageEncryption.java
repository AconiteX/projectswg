package protocol.packager;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

public class MessageEncryption {
	public byte[] encrypt(byte[] data, int crcSeed) {
		return encryptWithCRC(data, crcSeed, true);
	}

	public byte[] decrypt(byte[] data, int crcSeed) {
		return encryptWithCRC(data, crcSeed, false);
	}

	private byte[] encryptWithCRC(byte[] data, int crcSeed, boolean out) {
		int startingIndex = 2;
		if (data.length < 4) {
			return new byte[0];
		}
		if ((data[0] > 0) && (data[0] < 16)) {
			startingIndex = 1;
		}
		ByteBuffer sourceByteBuffer = ByteBuffer.wrap(data).order(
				ByteOrder.LITTLE_ENDIAN);
		ByteBuffer resultByteBuffer = ByteBuffer.allocate(data.length).order(
				ByteOrder.LITTLE_ENDIAN);
		try {
			resultByteBuffer.put(data[0]);
			if (startingIndex > 1) {
				resultByteBuffer.put(data[1]);
			}
			if (data.length < 6) {
				for (int i = startingIndex; i < data.length; i++) {
					resultByteBuffer.put((byte) (data[i] ^ crcSeed));
				}
			} else {
				resultByteBuffer.putInt(sourceByteBuffer.getInt(startingIndex)
						^ crcSeed);

				int i = startingIndex + 4;
				for (int j = startingIndex; i <= data.length - 4; j += 4) {
					resultByteBuffer.putInt(sourceByteBuffer.getInt(i)
							^ (out ? resultByteBuffer.getInt(j)
									: sourceByteBuffer.getInt(j)));
					i += 4;
				}
				for (int j = i - 4; i < data.length; i++) {
					resultByteBuffer
							.put((byte) (data[i] ^ (out ? resultByteBuffer
									.get(j) : sourceByteBuffer.get(j))));
				}
			}
			return resultByteBuffer.array();
		} catch (ArrayIndexOutOfBoundsException e) {
			return new byte[0];
		} catch (IndexOutOfBoundsException e) {
		}
		return new byte[0];
	}
}
