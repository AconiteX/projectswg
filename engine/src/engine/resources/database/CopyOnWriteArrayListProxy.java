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
package engine.resources.database;

import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

import com.sleepycat.persist.model.Persistent;
import com.sleepycat.persist.model.PersistentProxy;

@Persistent(proxyFor=CopyOnWriteArrayList.class)
public class CopyOnWriteArrayListProxy<E> implements PersistentProxy<CopyOnWriteArrayList<E>> {
	
	private int size = 0;
	private E[] elements;
	
	private CopyOnWriteArrayListProxy() { }
	
	@SuppressWarnings("unchecked")
	public void initializeProxy(CopyOnWriteArrayList<E> object) {
		List<E> elementList = new CopyOnWriteArrayList<E>();
		
		for (E element : object) {
			elementList.add(element);
		}
		
		size = object.size();
		
		elements = (E[]) elementList.toArray();
	}
	
	public CopyOnWriteArrayList<E> convertProxy() {
		CopyOnWriteArrayList<E> vector = new CopyOnWriteArrayList<E>();
		
		for (int i = 0; i < size; i++) {
			vector.add(elements[i]);
		}
		
		return vector;
	}
	
}
