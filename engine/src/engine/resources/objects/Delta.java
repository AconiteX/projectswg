/*******************************************************************************
 * Copyright (c) 2013 <Project SWG>
 * 
 * This File is part of NGECore2.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * Using NGEngine to work with NGECore2 is making a combined work based on NGEngine. 
 * Therefore all terms and conditions of the GNU Lesser General Public License cover the combination.
 ******************************************************************************/
package engine.resources.objects;

import java.io.Serializable;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import org.apache.mina.core.buffer.IoBuffer;

import engine.resources.common.StringUtilities;

public abstract class Delta implements IDelta, Serializable {
	
	private static final long serialVersionUID = 1L;
	
	protected transient Object objectMutex = new Object();
	
	public Delta() {
		
	}
	
	public void init(SWGObject object) {
		objectMutex = new Object();
	}
	
	protected String getAsciiString(ByteBuffer buffer) {
		return StringUtilities.getAsciiString(buffer);
	}
	
	protected String getUnicodeString(ByteBuffer buffer) {
		return StringUtilities.getUnicodeString(buffer);
	}
	
	protected byte[] getAsciiString(String string) {
		return StringUtilities.getAsciiString(string);
	}
	
	protected byte[] getUnicodeString(String string) {
		return StringUtilities.getUnicodeString(string);
	}
	
	protected byte getBoolean(boolean variable) {
		return ((byte) ((variable) ? 1 : 0));
	}
	
	public static IoBuffer createBuffer(int size) {
		return IoBuffer.allocate(size, false).order(ByteOrder.LITTLE_ENDIAN);
	}
	
	public static IoBuffer resizeBuffer(IoBuffer buffer) {
		int size = buffer.position();
		return buffer == null ? createBuffer(0) : createBuffer(size).put(buffer.flip().array(), 0, size).flip();
	}
	
}
