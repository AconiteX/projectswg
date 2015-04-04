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
package engine.resources.common;

import java.io.Serializable;
import java.nio.ByteOrder;

import org.apache.mina.core.buffer.IoBuffer;

import engine.resources.objects.Delta;

public final class AString extends Delta implements Serializable {
	
	private static final long serialVersionUID = 1L;
	private String string;
	
	public AString() {
		string = "";
	}
	
	public AString(String string) {
		this.string = ((string == null) ? "" : string);
	}
	
	public synchronized String get() {
		return string;
	}
	
	public synchronized void set(String string) {
		this.string = string;
	}
	
	public synchronized short length() {
		return (short) string.length();
	}
	
	public synchronized byte [] getBytes() {
		return IoBuffer.allocate(2 + string.length(), false).order(ByteOrder.LITTLE_ENDIAN).putShort((short) string.length()).put(string.getBytes()).flip().array();
	}
	
}
