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
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;
import java.util.Map;
import java.util.Map.Entry;
import java.util.TreeMap;
import java.util.Vector;
import java.util.concurrent.CopyOnWriteArrayList;

import org.apache.commons.lang3.ArrayUtils;
import org.apache.mina.core.buffer.IoBuffer;

import resources.common.Opcodes;
import resources.objects.SWGList;
import resources.objects.SWGMap;
import resources.objects.SWGMultiMap;
import resources.objects.SWGSet;
import engine.resources.common.AString;
import engine.resources.common.StringUtilities;
import engine.resources.common.UString;
import engine.resources.objects.SWGObject;
import engine.resources.scene.Point2D;
import engine.resources.scene.Point3D;

public class Baseline implements List<Object>, Serializable {
	
	private static final long serialVersionUID = 1L;
	private List<Object> list = new CopyOnWriteArrayList<Object>();
	private Map<String, Integer> definition = new TreeMap<String, Integer>();
	private transient SWGObject object;
	private byte viewType;
	protected transient Object objectMutex = new Object();
	private transient boolean hasBuilders = true;
	
	public Baseline() { }
	
	public Baseline(SWGObject object, int viewType) {
		this.viewType = (byte) viewType;
		this.object = object;
	}
	
	public boolean add(Object e) {
		synchronized(objectMutex) {
			if (list.add(checkSet(e, e))) {
				definition.put(Integer.toString(list.size() - 1), list.size() - 1);
				return true;
			} else {
				return false;
			}
		}
	}
	
	public void add(int index, Object element) {
		synchronized(objectMutex) {
			return;
		}
	}
	
	public boolean addAll(Collection<? extends Object> c) {
		synchronized(objectMutex) {
			return false;
		}
	}
	
	public boolean addAll(int index, Collection<? extends Object> c) {
		synchronized(objectMutex) {
			return false;
		}
	}
	
	public static Object checkArray(Object o) {
		if (o != null && o.getClass().isArray()) {
			if (o.getClass().isPrimitive() || o instanceof byte[]) {
				if (o instanceof boolean[]) {
					return ArrayUtils.toObject((boolean[]) o);
				} else if (o instanceof byte[]) {
					return ArrayUtils.toObject((byte[]) o);
				} else if (o instanceof char[]) {
					return ArrayUtils.toObject((char[]) o);
				} else if (o instanceof short[]) {
					return ArrayUtils.toObject((short[]) o);
				} else if (o instanceof int[]) {
					return ArrayUtils.toObject((int[]) o);
				} else if (o instanceof float[]) {
					return ArrayUtils.toObject((float[]) o);
				} else if (o instanceof double[]) {
					return ArrayUtils.toObject((double[]) o);
				} else if (o instanceof long[]) {
					return ArrayUtils.toObject((long[]) o);
				}
			} else {
				if (o instanceof Boolean[]) {
					return ArrayUtils.toPrimitive((Boolean[]) o);
				} else if (o instanceof Byte[]) {
					return ArrayUtils.toPrimitive((Byte[]) o);
				} else if (o instanceof Character[]) {
					return ArrayUtils.toPrimitive((Character[]) o);
				} else if (o instanceof Short[]) {
					return ArrayUtils.toPrimitive((Short[]) o);
				} else if (o instanceof Integer[]) {
					return ArrayUtils.toPrimitive((Integer[]) o);
				} else if (o instanceof Float[]) {
					return ArrayUtils.toPrimitive((Float[]) o);
				} else if (o instanceof Double[]) {
					return ArrayUtils.toPrimitive((Double[]) o);
				} else if (o instanceof Long[]) {
					return ArrayUtils.toPrimitive((Long[]) o);
				}
			}
		}
		
		return o;
	}
	
	private Object checkGet(Object o) {
		return checkArray(o);
	}
	
	private Object checkSet(Object o1, Object o2) {
		if (o2 instanceof UString && o1 instanceof String) {
			o1 = new UString((String) o1);
		}
		
		return ((o1.getClass().isPrimitive() || o1 instanceof byte[]) ? checkArray(o1) : o1);
	}
	
	public void clear() {
		synchronized(objectMutex) {
			list.clear();
			definition.clear();
		}
	}
	
	public boolean contains(Object o) {
		synchronized(objectMutex) {
			return list.contains(o);
		}
	}
	
	public boolean containsAll(Collection<?> c) {
		synchronized(objectMutex) {
			return list.containsAll(c);
		}
	}
	
	private boolean compareTypes(Object o1, Object o2) {
		return o1.getClass().getSimpleName().equals(o2.getClass().getSimpleName());
	}
	
	public IoBuffer createBaseline() {
		byte[] objects = { };
		int size = 0;
		
		for (Object o : list) {
			byte[] object;
			
			if (hasBuilders && getBaselineBuilders().containsKey(o)) {
				object = getBaselineBuilders().get(o).build(this.object);
			} else {
				object = toBytes(o);
			}
			
			size += object.length;
			
			IoBuffer buffer = createBuffer(size);
			buffer.put(objects);
			buffer.put(object);
			buffer.flip();
			
			objects = buffer.array();
		}
		
		IoBuffer buffer = createBuffer(25 + size);
		buffer.putShort((short) 5);
		buffer.putInt(Opcodes.BaselinesMessage);
		buffer.putLong(object.getObjectID());
		try {
			buffer.put(reverse(getShortTemplate()).getBytes("US-ASCII"));
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
		}
		buffer.put(viewType);
		buffer.putInt(2 + size);
		buffer.putShort((short) list.size());
		buffer.put(objects);
		buffer.flip();
		try {
			if (getShortTemplate().equals("CREO")) StringUtilities.printBytes(buffer.array());
		} catch (Exception e) {
			e.printStackTrace();
		}
		return buffer;
	}
	
	public static IoBuffer createBuffer(int size) {
		return IoBuffer.allocate(size, false).order(ByteOrder.LITTLE_ENDIAN);
	}
	
	public IoBuffer createDelta() {
		List<Integer> objectQueue = new ArrayList<Integer>();
		
		synchronized(objectMutex) {
			for (int i = 0; i < list.size(); i++) {
				objectQueue.add(i);
			}
		}
		
		return createDelta(objectQueue, null);
	}
	
	public IoBuffer createDelta(int object) {
		List<Integer> objectQueue = new ArrayList<Integer>();
		objectQueue.add(object);
		return createDelta(objectQueue, null);
	}
	
	public IoBuffer createDelta(int object, byte[] data) {
		List<Integer> objectQueue = new ArrayList<Integer>();
		objectQueue.add(object);
		return createDelta(objectQueue, data);
	}
	
	public IoBuffer createDelta(List<Integer> objectQueue, byte[] data) {
		byte[] objects = { };
		int size = 2;
		
		for (Integer o : objectQueue) {
			byte[] object;
			
			if (hasBuilders && getDeltaBuilders().containsKey(o)) {
				object = getDeltaBuilders().get(o).build(this.object);
			} else if (data != null) {
				object = data;
			} else {
				object = toBytes(get(o));
			}
			
			size += 2 + object.length;
			
			IoBuffer buffer = createBuffer(size);
			buffer.put(objects);
			buffer.putShort(o.shortValue());
			buffer.put(object);
			buffer.flip();
			
			objects = buffer.array();
		}
		
		IoBuffer buffer = createBuffer(27 + size);
		buffer.putShort((short) 5);
		buffer.putInt(Opcodes.DeltasMessage);
		buffer.putLong(object.getObjectID());
		try {
			buffer.put(reverse(getShortTemplate()).getBytes("US-ASCII"));
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
 		} catch (Exception e) {
			e.printStackTrace();
		}
		buffer.put(viewType);
		buffer.putInt(size);
		buffer.putShort((short) objectQueue.size());
		buffer.put(objects);
		buffer.flip();
		//StringUtilities.printBytes(buffer.array());
		return buffer;
	}
	
	public Object get(int index) {
		synchronized(objectMutex) {
			return checkGet(list.get(index));
		}
	}
	
	public Object get(String name) {
		if (objectMutex == null) {
			System.err.println("Baseline::get(): objectMutex is null for " + object.getTemplate());
		}
		
		synchronized(objectMutex) {
			if (!definition.containsKey(name))
				return null;
			return list.get(definition.get(name));
		}
	}
	
	public IoBuffer getBaseline() {
		return createBaseline();
	}
	
	public Map<Integer, Builder> getBaselineBuilders() {
		Map<Integer, Builder> baselineBuilders = new HashMap<Integer, Builder>();
		Map<Integer, Builder> deltaBuilders = new HashMap<Integer, Builder>();
		
		try {
			if (object.getMessageBuilder() != null) {
				object.getMessageBuilder().getClass().getMethod("buildBaseline" + viewType, new Class[] { Map.class, Map.class }).invoke(object.getMessageBuilder(), new Object[] { baselineBuilders, deltaBuilders });
			}
		} catch (Exception e) {
			
		}
		
		if (baselineBuilders.size() == 0 && deltaBuilders.size() == 0) {
			hasBuilders = false;
		}
		
		return baselineBuilders;
	}
	
	public static byte getBoolean(boolean condition) {
		return ((byte) ((condition) ? 1 : 0));
	}
	
	public static boolean getBoolean(byte condition) {
		return ((condition == 1) ? true : false);
	}
	
	public byte[] getBytes() {
		synchronized(objectMutex) {
			int size = 0;
			
			for (Object o : list) {
				size += toBytes(o).length;
			}
			
			IoBuffer buffer = createBuffer(size);
			
			for (Object o : list) {
				buffer.put(toBytes(o));
			}
			
			buffer.flip();
			
			return buffer.array();
		}
	}
	
	public byte[] getBytes(int index) {
		synchronized(objectMutex) {
			return toBytes(list.get(index));
		}
	}
	
	public byte[] getBytes(String name) {
		synchronized(objectMutex) {
			return toBytes(list.get(definition.get(name)));
		}
	}
	
	private String getDefinition(int index) {
		for (Entry<String, Integer> entry : definition.entrySet()) {
			if (entry.getValue() == index) {
				return entry.getKey();
			}
		}
			
		return null;
	}
	
	public Map<String, Integer> getDefinitions() {
		synchronized(objectMutex) {
			return definition;
		}
	}
	
	public IoBuffer getDelta(int index) {
		return createDelta(index);
	}
	
	public IoBuffer getDelta(String name) {
		return createDelta(definition.get(name));
	}
	
	public Map<Integer, Builder> getDeltaBuilders() {
		Map<Integer, Builder> baselineBuilders = new HashMap<Integer, Builder>();
		Map<Integer, Builder> deltaBuilders = new HashMap<Integer, Builder>();
		
		try {
			if (object.getMessageBuilder() != null) {
				object.getMessageBuilder().getClass().getMethod("buildBaseline" + viewType, new Class[] { Map.class, Map.class }).invoke(object.getMessageBuilder(), new Object[] { baselineBuilders, deltaBuilders });
			}
		} catch (Exception e) {
			
		}
		
		if (baselineBuilders.size() == 0 && deltaBuilders.size() == 0) {
			hasBuilders = false;
		}
		
		return deltaBuilders;
	}
	
	public List<Object> getList() {
		synchronized(objectMutex) {
			return list;
		}
	}
	
	public Object getMutex() {
		return objectMutex;
	}
	
	public String getShortTemplate() throws Exception {
		String Template = object.getTemplate();
		
		if (Template.startsWith("object/battlefield_marker")) {
			return "BMRK";
		} else if(Template.startsWith("object/building")){
			return "BUIO";
		} else if(Template.startsWith("object/cell")) {
			return "SCLT";
		} else if (Template.startsWith("object/construction_contract")) {
			throw new Exception();
		} else if (Template.startsWith("object/counting")) {
			throw new Exception();
		} else if (Template.startsWith("object/creature")) {
			return "CREO";
		} else if (Template.startsWith("object/draft_schematic")) {
			return "DSCO";
		} else if (Template.startsWith("object/factory")) {
			return "FCYT";
		} else if(Template.startsWith("object/group")) {
			return "GRUP";
		} else if(Template.startsWith("object/guild")) {
			return "GILD";
		} else if (Template.startsWith("object/installation/battlefield")) {
			throw new Exception();
		} else if (Template.startsWith("object/installation/faction_perk/covert_detector")) {
			throw new Exception();
		} else if (Template.startsWith("object/installation/faction_perk/minefield")) {
			throw new Exception();
		} else if (Template.startsWith("object/installation/faction_perk/turret")) {
			throw new Exception();
		} else if (Template.startsWith("object/installation/generators")) {
			return "HINO";
		} else if (Template.startsWith("object/installation/manufacture")) {
			return "MINO";
		} else if (Template.startsWith("object/installation")) {
			return "INSO";
		} else if (Template.startsWith("object/intangible")) {
			return "ITNO";
		} else if (Template.startsWith("object/jedi_manager")) {
			throw new Exception();
		} else if (Template.startsWith("object/manufacture_schematic")) {
			return "MSCO";
		} else if (Template.startsWith("object/mission")) {
			return "MISO";
		} else if (Template.startsWith("object/mobile")) {
			return "CREO";
		} else if (Template.startsWith("object/object")) {
			throw new Exception();
		} else if (Template.startsWith("object/path_waypoint")) {
			throw new Exception();
		} else if (Template.startsWith("object/player")) {
			return "PLAY";
		} else if (Template.startsWith("object/player_quest")) {
			return "PQOS";
		} else if (Template.startsWith("object/resource_container")) {
			return "RCNO";
		} else if (Template.startsWith("object/ship")) {
			return "SHIP";
		} else if (Template.startsWith("object/soundobject")) {
			throw new Exception();
		} else if(Template.startsWith("object/static")) {
			return "STAO";
		} else if (Template.startsWith("object/tangible")) {
			return "TANO";
		} else if (Template.startsWith("object/token")) {
			throw new Exception();
		} else if (Template.startsWith("object/universe")) {
			throw new Exception();
		} else if(Template.startsWith("object/waypoint")) {
			return "WAYP";
		} else if (Template.startsWith("object/weapon")) {
			return "WEAO";
		} else {
			throw new Exception();
		}
	}
	
	public int indexOf(Object o) {
		synchronized(objectMutex) {
			return list.indexOf(o);
		}
	}
	
	public boolean isEmpty() {
		synchronized(objectMutex) {
			return list.isEmpty();
		}
	}
	
	public Iterator<Object> iterator() {
		synchronized(objectMutex) {
			return list.iterator();
		}
	}
	
	public int lastIndexOf(Object o) {
		synchronized(objectMutex) {
			return lastIndexOf(o);
		}
	}
	
	public ListIterator<Object> listIterator() {
		synchronized(objectMutex) {
			return list.listIterator();
		}
	}
	
	public ListIterator<Object> listIterator(int index) {
		synchronized(objectMutex) {
			return listIterator(index);
		}
	}
	
	public void put(String name, Object o) {
		synchronized(objectMutex) {
			if (!definition.containsKey(name)) {
				definition.put(name, list.size());
				list.add(o);
			} else {
				list.set(definition.get(name), o);
			}
		}
	}
	
	public boolean remove(Object o) {
		return false;
	}
	
	public Object remove(int index) {
		return null;
	}
	
	public boolean removeAll(Collection<?> c) {
		return false;
	}
	
	public boolean retainAll(Collection<?> c) {
		return false;
	}
	
	private String reverse(String reverseString) {
	    if (reverseString.length() <= 1) {
	    	return reverseString;
	    }
	    
	    return reverse(reverseString.substring(1, reverseString.length())) + reverseString.charAt(0);
	}
	
	public Object set(int index, Object element) {
		return null;
	}
	
	public IoBuffer set(String name, Object o) {
		synchronized(objectMutex) {
			Integer index = definition.get(name);
			if (index == null) {
				index = definition.size();
				put(name, o);
			}
			
			if (compareTypes(checkSet(o, list.get(index)), list.get(index)) && list.set(index, checkSet(o, list.get(index))) != null) {
				return createDelta(index);
			} else {
				return null;
			}
		}
	}
	
	public int size() {
		synchronized(objectMutex) {
			return list.size();
		}
	}
	
	public List<Object> subList(int fromIndex, int toIndex) {
		synchronized(objectMutex) {
			return list.subList(fromIndex, toIndex);
		}
	}
	
	public Object[] toArray() {
		synchronized(objectMutex) {
			return list.toArray();
		}
	}
	
	public <T> T[] toArray(T[] a) {
		synchronized(objectMutex) {
			return list.toArray(a);
		}
	}
	
	public static byte[] toBytes(Object o) {
		try {
			if (o != null) {
				if (o instanceof IDelta) {
					return ((IDelta) o).getBytes();
				} else if (o instanceof String) {
					return (new AString((String) o)).getBytes();
				} else if (o instanceof AString) {
					return ((AString) o).getBytes();
				} else if (o instanceof UString) {
					return ((UString) o).getBytes();
				} else if (o instanceof Byte[]) {
					IoBuffer buffer = createBuffer(2 + ((Byte[]) o).length);
					buffer.putShort((short) ((Byte[]) o).length);
					for (Byte b : (Byte[]) o) buffer.put(b);
					return buffer.array();
				} else if (o instanceof byte[]) {
					return createBuffer(2 + ((byte[]) o).length).putShort((short) ((byte[]) o).length).put((byte[]) o).array();
				} else if (o instanceof Byte) {
					return createBuffer(1).put((Byte) o).array();
				} else if (o instanceof Boolean) {
					return createBuffer(1).put(((((Boolean) o)) ? (byte) 1 : (byte) 0)).array();
				} else if (o instanceof Short) {
					return createBuffer(2).putShort((Short) o).array();
				} else if (o instanceof Integer) {
					return createBuffer(4).putInt((Integer) o).array();
				} else if (o instanceof Float) {
					return createBuffer(4).putFloat((Float) o).array();
				} else if (o instanceof Long) {
					return createBuffer(8).putLong((Long) o).array();
				/*
				} else if (o instanceof SWGObject) {
					long objectId = ((((SWGObject) o) == null) ? (long) 0 : ((SWGObject) o).getObjectID());
					return createBuffer(8).putLong(objectId).array();
				*/
				} else if (o instanceof SWGList) {
					return ((SWGList<?>) o).getBytes();
				} else if (o instanceof SWGSet) {
					return ((SWGSet<?>) o).getBytes();
				} else if (o instanceof SWGMap) {
					return ((SWGMap<?, ?>) o).getBytes();
				} else if (o instanceof SWGMultiMap) {
					return ((SWGMultiMap<?, ?>) o).getBytes();
				} else if (o instanceof List) {
					List<?> list = ((List<?>) o);
					int size = 0;
					byte[] objects = { };
					
					for (int i = 0; i < list.size(); i++) {
						byte[] object = toBytes(list.get(i));
						size += object.length;
						
						IoBuffer buffer = createBuffer(size);
						buffer.put(objects);
						buffer.put(object);
						buffer.flip();
						
						objects = buffer.array();
					}
					
					return createBuffer(4 + size).putInt(list.size()).put(objects).array();
				} else if (o instanceof Vector) {
					Vector<?> list = ((Vector<?>) o);
					int size = 0;
					byte[] objects = { };
					
					for (int i = 0; i < list.size(); i++) {
						byte[] object = toBytes(list.get(i));
						size += object.length;
						
						IoBuffer buffer = createBuffer(size);
						buffer.put(objects);
						buffer.put(object);
						buffer.flip();
						
						objects = buffer.array();
					}
					
					return createBuffer(size + 4).putInt(list.size()).array();
				} else if (o instanceof Map) {
					Map<?, ?> map = ((Map<?, ?>) o);
					int size = 0;
					byte[] objects = { };
					
					for (Entry<?, ?> entry : map.entrySet()) {
						byte[] keyBytes = toBytes(entry.getKey());
						byte[] valueBytes = toBytes(entry.getValue());
						byte[] object = createBuffer(keyBytes.length + valueBytes.length).put(keyBytes).put(valueBytes).flip().array();
						size += object.length;
						
						IoBuffer buffer = createBuffer(size);
						buffer.put(objects);
						buffer.put(object);
						buffer.flip();
						
						objects = buffer.array();
					}
					
					return createBuffer(size + 4).putInt(map.size()).array();
				} else if (o instanceof Point2D) {
					Point2D p = (Point2D) o;
					return createBuffer(8).putFloat(p.x).putFloat(p.z).array();
				} else if (o instanceof Point3D) {
					Point3D p = (Point3D) o;
					return createBuffer(12).putFloat(p.x).putFloat(p.y).putFloat(p.z).array();
				} else if (o instanceof BitSet) {
					BitSet b = ((BitSet) o);
					byte[] bA = b.toByteArray();
					return createBuffer(8 + bA.length).putInt(bA.length).putInt(b.length()).put(bA).array();
				} else {
					System.err.println("ERROR: Unsupported type used in Baseline: " + (o.getClass()).getSimpleName());
					throw new Exception();
				}
			} else {
				if (o instanceof IDelta) {
					System.err.println("ERROR: a baseline object that implements IDelta is null.  This could cause crashes!  Make it at least a new instance with all fields set to 0.");
					
					try {
						throw new Exception();
					} catch (Exception e) {
						e.printStackTrace();
					}
					
					return new byte[] { };
				} else if (o instanceof String) {
					return new byte[] { 0x00, 0x00 };
				} else if (o instanceof AString) {
					return new byte[] { 0x00, 0x00 };
				} else if (o instanceof UString) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Byte[]) {
					return new byte[] { 0x00, 0x00 };
				} else if (o instanceof Byte) {
					return new byte[] { 0x00 };
				} else if (o instanceof Boolean) {
					return new byte[] { 0x00 };
				} else if (o instanceof Short) {
					return new byte[] { 0x00, 0x00 };
				} else if (o instanceof Integer) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Float) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Long) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof SWGObject) {
					return null;
				} else if (o instanceof SWGList) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof SWGSet) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof SWGMap) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof SWGMultiMap) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof List) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Vector) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Map) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Point2D) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof Point3D) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else if (o instanceof BitSet) {
					return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
				} else {
					System.err.println("ERROR: Unsupported type used in Baseline.");
					System.err.println("~additionally, the value was null, which is dangerous.");
					return new byte[] { };
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
			return new byte[] { };
		}
	}
	
	public void transformStructure(SWGObject object, Baseline defaults) {
		objectMutex = new Object();
		this.object = object;
		hasBuilders = true;
		
		synchronized(objectMutex) {
			for (Object delta : list) {				
				try {
					initializeChildren(delta);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		}
		
		synchronized(objectMutex) {
			List<Object> oldStruct = list;
			Baseline newStruct = defaults;
			
			for (int i = 0; i < newStruct.size(); i++) {
				if (definition.containsKey(newStruct.getDefinition(i))) {
					int oldIndex = definition.get(newStruct.getDefinition(i));
					Object newObject = checkArray(newStruct.get(i));
					Object oldObject = oldStruct.get(oldIndex);
					
					if (compareTypes(newObject, oldObject)) {
						newStruct.list.set(i, oldObject);
					} else {
						if (newObject instanceof String) {
							if (oldObject instanceof AString) {
								newStruct.list.set(i, ((AString) oldObject).get());
							} else if (oldObject instanceof UString) {
								newStruct.list.set(i, ((UString) oldObject).get());
							}
						} else if (newObject instanceof AString) {
							if (oldObject instanceof String) {
								newStruct.list.set(i, new AString((String) oldObject));
							} else if (oldObject instanceof UString) {
								newStruct.list.set(i, new AString(((UString) oldObject).get()));
							}
						} else if (newObject instanceof UString) {
							if (oldObject instanceof String) {
								newStruct.list.set(i, new UString((String) oldObject));
							} else if (oldObject instanceof AString) {
								newStruct.list.set(i, new UString(((AString) oldObject).get()));
							}
						} else if (newObject instanceof Byte) {
							if (oldObject instanceof Short && (Short) oldObject < 0xFF) {
								newStruct.list.set(i, ((Short) oldObject).byteValue());
							} else if (oldObject instanceof Integer && (Integer) oldObject < 0xFF) {
								newStruct.list.set(i, ((Integer) oldObject).byteValue());
							} else if (oldObject instanceof Long && (Long) oldObject < 0xFF) {
								newStruct.list.set(i, ((Long) oldObject).byteValue());
							}
						} else if (newObject instanceof Short) {
							if (oldObject instanceof Byte) {
								newStruct.list.set(i, ((Byte) oldObject).shortValue());
							} else if (oldObject instanceof Integer && (Integer) oldObject < 0xFFFF) {
								newStruct.list.set(i, ((Integer) oldObject).shortValue());
							} else if (oldObject instanceof Long && (Long) oldObject < 0xFFFF) {
								newStruct.list.set(i, ((Long) oldObject).shortValue());
							}
						} else if (newObject instanceof Integer) {
							if (oldObject instanceof Byte) {
								newStruct.list.set(i, ((Byte) oldObject).intValue());
							} else if (oldObject instanceof Short) {
								newStruct.list.set(i, ((Short) oldObject).intValue());
							} else if (oldObject instanceof Long && (Long) oldObject < 0xFFFFFFFF) {
								newStruct.list.set(i, ((Long) oldObject).intValue());
							}
						} else if (newObject instanceof Long) {
							if (oldObject instanceof Byte) {
								newStruct.list.set(i, ((Byte) oldObject).longValue());
							} else if (oldObject instanceof Short) {
								newStruct.list.set(i, ((Short) oldObject).longValue());
							} else if (oldObject instanceof Integer) {
								newStruct.list.set(i, ((Integer) oldObject).longValue());
							} else if (oldObject instanceof SWGObject) {
								newStruct.list.set(i, ((SWGObject) oldObject).getObjectID());
							}
						}
					}
				}
			}
			
			list = newStruct.list;
		}
		
	}
	
	/*
	 * This will initialize all children of an IDelta that inherit IDelta.
	 * This way we don't get issues with AString's objectMutex being null etc.
	 */
	public void initializeChildren(Object delta) throws IllegalArgumentException, IllegalAccessException, InvocationTargetException, NoSuchMethodException, SecurityException {
		if (delta instanceof IDelta || delta instanceof SWGList || delta instanceof SWGMap || delta instanceof SWGMultiMap || delta instanceof SWGSet) {
			delta.getClass().getMethod("init", new Class[] { SWGObject.class }).invoke(delta, new Object[] { object });
			
			Class<?> iDelta = null;
			
			try {
				iDelta = Class.forName("engine.resources.objects.IDelta");
			} catch (ClassNotFoundException e) {
				e.printStackTrace();
				return;
			}
			
			for (Field field : delta.getClass().getFields()) {
				if (iDelta.isInstance(field.get(delta))) {
					initializeChildren(field.get(delta));
				}
			}
		}
	}
	
}
