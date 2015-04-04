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

import org.apache.mina.core.buffer.IoBuffer;

import engine.clientdata.StfTable;
import engine.resources.objects.Delta;

public class Stf extends Delta implements Serializable {
	
	private static final long serialVersionUID = 1L;
	
	private AString stfFilename = new AString("");
	private int spacer = 0;
	private AString stfName = new AString("");
	
	public Stf(String stfFilename, int spacer, String stfName) {
		this.stfFilename = new AString(stfFilename);
		this.spacer = spacer;
		this.stfName = new AString(stfName);
	}
	
	public Stf(String stf) {
		setString(stf);
	}
	
	public Stf() {
		
	}
	
	public synchronized String getStfFilename() {
		return stfFilename.get();
	}
	
	public synchronized void setStfFilename(String stfFilename) {
		this.stfFilename = new AString(stfFilename);
	}
	
	public synchronized int getSpacer() {
		return spacer;
	}
	
	public synchronized void setSpacer(int spacer) {
		this.spacer = spacer;
	}
	
	public synchronized String getStfName() {
		return stfName.get();
	}
	
	public synchronized void setStfName(String stfName) {
		this.stfName = new AString(stfName);
	}
	
	public synchronized String getStfValue() {
		try {
			StfTable stf = new StfTable("clientdata/string/en/" + stfFilename.get() + ".stf");
			
			for (int s = 1; s < stf.getRowCount(); s++) {
				if (stf.getStringById(s).getKey() != null && stf.getStringById(s).getKey().equals(stfName.get())) {
					if (stf.getStringById(s).getValue() != null) {
						return stf.getStringById(s).getValue();
					}
				}
			}
			
			return "";
        } catch (Exception e) {
        	return "";
        }
	}
	
	public synchronized String getString() {
		return ("@" + stfFilename.get() + ":" + stfName.get());
	}
	
	public synchronized void setString(String stf) {
		if (stf == null || stf.equals("")) {
			stfFilename.set("");
			stfName.set("");
		} else if (stf.contains(":")) {
			stf = stf.replace("@", "");
			stfFilename.set(stf.split(":")[0]);
			stfName.set(stf.split(":")[1]);
		}
	}
	
	public synchronized byte [] getBytes() {
		int size = stfFilename.getBytes().length + 4 + stfName.getBytes().length;
		
		IoBuffer buffer = createBuffer(size);
		buffer.put(stfFilename.getBytes());
		buffer.putInt(spacer);
		buffer.put(stfName.getBytes());
		buffer.flip();
		
		return buffer.array();
	}
	
}
