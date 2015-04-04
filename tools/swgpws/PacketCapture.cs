using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace SWGPacket_Workshop
{
    public class PacketCapture
    {
        # region Variables

        Int32               _crcSeed = 0;                       // Manual crcSeed
        Byte[]              _data;                              // Capture file raw byte data
        int[]               _pcapIndexes;                       // PCap packet indexes
        Byte[][]            _soePackets;                        // Cached SOE packets data
        List<int>           _sessRespIndexes = new List<int>(); // Session response packet Indexes
        String[]            soePacketType = new string[] { "None", "Session Req", "Session Resp", "Multi Pkt", "Not Used", "Disconnect", 
            "Persistant Connect", "Network Stats Cli", "Network Stats Svr", 
            "Data Pkt Ch0", "Data Pkt Ch1", "Data Pkt Ch2", "Data Pkt Ch3", 
            "Frag Data Pkt Ch0", "Frag Data Pkt Ch1", "Frag Data Pkt Ch2", "Frag Data Pkt Ch3", 
            "OOO Pkt Ch0", "OOO Pkt Ch1", "OOO Pkt Ch2", "OOO Pkt Ch3", 
            "Ack Pkt Ch0", "Ack Pkt Ch1", "Ack Pkt Ch2", "Ack Pkt Ch3", 
            "Unk Pkt Ch0", "Unk Pkt Ch1", "Unk Pkt Ch2", "Unk Pkt Ch3", 
            "Fatal Error", "Fatal Error Reply"};
        Int32               _threadCt = 0;
        Int32               _srcIPAddress = 0;
        Mutex               _crcMutex = new Mutex();
        UInt16              _srcPort = 0;

        # endregion

        # region Class constructor

        public PacketCapture()
        {
            _sessRespIndexes = new List<int>();
            _sessRespIndexes.Add(0);
        }

        # endregion
        # region Properties

        public Int32 CRCSeed
        {
            get { return _crcSeed; }
            set
            {
                _crcSeed = value;
                Array.Clear(_soePackets, 0, _soePackets.Length);
                catalogSOE((object)new Int32[] { 0, _pcapIndexes.Length });
            }
        }
        public Int32 Length { get { return _pcapIndexes.Length; } }
        public Int32 IPAddress { get { return _srcIPAddress; } }

        # endregion
        # region Public func

        public Int32 Open(String file, Int32 threadCount = 4)
        {
            System.Threading.Thread[] threads = new System.Threading.Thread[threadCount];
            byte[] buffer = new byte[1024];
            _data = System.IO.File.ReadAllBytes(file);

            // Index UDP packets
            indexUDP();         

            // Start multithreaded cataloging
            DateTime start = DateTime.Now;
            _soePackets = new Byte[_pcapIndexes.Length][];
            if (_soePackets.Length < 2000) threadCount = 1;
            for (Int32 i = 0, sI = 0, c = 0; i < threadCount; i++, sI += c)
            {
                c = (_pcapIndexes.Length - sI) / (threadCount - i);
                Int32[] param = new Int32[] { sI, c };
                threads[i] = new System.Threading.Thread(catalogSOE);
                threads[i].Start(param);
                _threadCt++;
            }
            while (_threadCt > 0) { System.Threading.Thread.Sleep(20); }

            Console.WriteLine("{0} packets indexed in {1:0.000}s", _pcapIndexes.Length, (DateTime.Now - start).TotalSeconds);
            return 0;
        }

        public Boolean IsIncomming(Int32 index) { return _srcIPAddress == _data.GetDestIP(_pcapIndexes[index]) && _srcPort == _data.GetDestPort(_pcapIndexes[index]); }
        public Boolean IsEncrypted(Int32 index) { return GetSOEOpcode(index) > 0x0002 && GetSOEOpcode(index) < 0x0020; }
        public Boolean IsCompressed(Int32 index) { return _soePackets[index][_soePackets[index].Length - 3] == 1; }
        
        public Byte[] GetUDPPDU(Int32 index) { return _data.GetUDPPDU(_pcapIndexes[index]); }
        
        public UInt16 GetSOEOpcode(Int32 index) { return _data.GetSOEOpcode(_pcapIndexes[index]); }
        public Int32 GetSOECRCSeed(Int32 index) 
        {
            UInt16 soeOpCode = GetSOEOpcode(index);
            if (soeOpCode == 0x0001 || soeOpCode == 0x0002)
                return 0;

            _crcMutex.WaitOne();

            Int32 crcSeed = _crcSeed;
            if (crcSeed == 0)
                crcSeed = _data.GetSOECRCSeed(_pcapIndexes[index] > _sessRespIndexes[0] ? _sessRespIndexes.Last<Int32>(x => x < _pcapIndexes[index]) : 0);

            if (crcSeed == 0)
                crcSeed = (_crcSeed = (Int32)RecoverCRC());

            _crcMutex.ReleaseMutex();

            return crcSeed; 
        }
        public String GetSOEMessageType(Int32 index)
        {
            UInt16 opcode = GetSOEOpcode(index);
            if (opcode > soePacketType.Length - 1) return "Unknown";
            return soePacketType[opcode];
        }
        public Byte[] GetSOEMessage(Int32 index)
        {
            if (_soePackets[index] == null)
                _soePackets[index] = _data.GetSOEMessage(_pcapIndexes[index], GetSOECRCSeed(index));
            return _soePackets[index];
        }
        public Byte[] GetSOEFragmentedMessage(Int32 index)
        {
            try
            {

                Int32 seq = getSOESequence(index);
                Int32 curIndex;
                while ((curIndex = getSOEIndex(--seq, index, true)) == -1) ;

                while (GetSOEOpcode(curIndex) == 0x000D && GetSOEMessage(curIndex).Length == 496) curIndex = getSOEIndex(--seq, index, true);
                seq++;

                Int32 startIndex = getSOEIndex(seq, index, true);

                // Assemble data
                Int32 len = BitConverter.ToInt32(BitConverter.GetBytes(BitConverter.ToInt32(GetSOEMessage(getSOEIndex(seq, index, true)), 4)).Reverse<Byte>().ToArray<Byte>(), 0);
                List<Byte> assembledMessage = new List<Byte>(65536);

                curIndex = startIndex;
                assembledMessage.AddRange(GetSOEMessage(curIndex).Take<Byte>(493));

                while (assembledMessage.Count < len)
                {
                    if (len - assembledMessage.Count >= 489)
                    {
                        if ((curIndex = getSOEIndex(++seq, startIndex > 50 ? startIndex - 50 : startIndex)) == -1)
                            return null;
                        assembledMessage.AddRange(GetSOEMessage(curIndex).Skip<Byte>(4).Take<Byte>(489).ToArray<Byte>());
                    }
                    else
                    {
                        UInt32 destPort = _data.GetDestPort(_pcapIndexes[curIndex]);
                        UInt32 srcPort = _data.GetSourcePort(_pcapIndexes[curIndex]);
                        UInt32 opcode = GetSOEOpcode(++curIndex);
                        while (opcode != 0x000D &&
                            opcode != 0x0003 &&
                            destPort != _data.GetDestPort(_pcapIndexes[curIndex]) &&
                            srcPort != _data.GetSourcePort(_pcapIndexes[curIndex]))
                            opcode = GetSOEOpcode(++curIndex);
                        if (opcode == 0x000D)
                            assembledMessage.AddRange(GetSOEMessage(curIndex).Skip<Byte>(4).Take<Byte>(489).ToArray<Byte>());
                        else
                        {
                        }
                    }
                }

                return assembledMessage.ToArray<Byte>();
            }
            catch
            {
                return null;
            }
        }
        public String GetSourceAddr(Int32 index)
        {
            Byte[] addr = BitConverter.GetBytes(_data.GetSourceIP(_pcapIndexes[index]));
            return String.Format("{0}.{1}.{2}.{3}", addr[0], addr[1], addr[2], addr[3]);
        }
        public string GetSourcePort(Int32 index) { return _data.GetSourcePort(_pcapIndexes[index]).ToString(); }
        public String GetDestAddr(Int32 index)
        {
            Byte[] addr = BitConverter.GetBytes(_data.GetDestIP(_pcapIndexes[index]));
            return String.Format("{0}.{1}.{2}.{3}", addr[0], addr[1], addr[2], addr[3]);
        }
        public string GetDestPort(Int32 index) { return _data.GetDestPort(_pcapIndexes[index]).ToString(); }

        public String[] returnHexASCII(int packettype, int selection, bool addlinebreaks)
        {
        	String[] rv = new String[4];
        	
            byte[] packet = null;
            switch (selection)
            {
                case 0:
                    packet = GetUDPPDU(packettype);
                    break;
                case 1:
                    packet = GetSOEMessage(packettype);
                    break;
                case 2:
                    return null;
            }
            string addr = "";
            string hex = "";
            string ascii = "";
            string hexascii = "";
            int index = 0;
            for (int line = 0; ; line++)
            {
            	String thislinehex = "";
            	String thislineascii = "";
            	
                addr += string.Format("{0:X4}:\r\n", index);
                for (int b = 0; b < 16; b++)
                {
                    char asciiChar = (char)packet[index];
                    if (asciiChar > 127 || asciiChar < 32) asciiChar = '.';
                    hex += string.Format("{0:X2}", packet[index]);
                    thislinehex += string.Format("{0:X2}", packet[index]);
                    ascii += asciiChar;
                    thislineascii += asciiChar;
                    index++;
                    if (index == packet.Length) {
                    	hexascii += thislinehex.PadRight(48, ' ') + "   " + thislineascii.PadRight(16, ' ');
                    	goto Done;
                    }
                    switch (b)
                    {
                        case 7:
                            hex += "-";
                            thislinehex += "-";
                            break;
                        default:
                            hex += " ";
                            thislinehex += " ";
                            break;
                    }
                }
                
                hexascii += thislinehex.PadRight(48, ' ') + "   " + thislineascii.PadRight(16, ' ');
                if (addlinebreaks) {
                	hex += Environment.NewLine;
                	ascii += Environment.NewLine;
                	hexascii += Environment.NewLine;
                }
                
            }
        Done:
            rv[0] = addr;
            rv[1] = hex;
            rv[2] = ascii;
            rv[3] = hexascii;
            
            return rv;
        }
        
        public String ReturnDelim(int PacketIndex, int PacketType) {
        	
        	
        	Byte[] packet = ((PacketType == 0) ? GetUDPPDU(PacketIndex) : GetSOEMessage(PacketIndex));
            
 
            return "0x" + BitConverter.ToString(packet).Replace("-", ", 0x") + "\r\n";
        	
        }        
      	

        public String GetAllPackages(int type, int part) {
        	
        	String s = "";
        	
        	if (part <0 || part>4) { return ""; }
        	
        	for (int i = 0 ; i < Length; i++) {
        		
        		if (part == 4) {
        			s += ReturnDelim(i, type);
        		} else {
        			s += returnHexASCII(i, type, true)[part];
        		}
        		s += Environment.NewLine;
        		s += Environment.NewLine;
        		s += "# # # # # # # # # #";
        		s += Environment.NewLine;        		
        		s += Environment.NewLine;
        	}
        	
        	return s;
        }
        
        public SceneObject FindCoords(byte[] ObjectID) {

        	for (int i =0 ; i < Length; i++) {
        		
        		Byte[] CurrentPacket = GetSOEMessage(i);
        		for (int j = 0; j < CurrentPacket.Length; j++) {
        			
	        		if (!FindSceneCreateObjectByCRC(CurrentPacket, j)) {
	        			continue;
	        		}
        			
        			//packet not long enough
        			if ((CurrentPacket.Length - j) < 29) {
        				break;
        			}
        			
        			j+=4;
        			
        			//this is not the packet we're looking for...
        			if (CurrentPacket[j] != ObjectID[0]) { break; }
        			if (CurrentPacket[j+1] != ObjectID[1]) { break; }
        			if (CurrentPacket[j+2] != ObjectID[2]) { break; }
        			if (CurrentPacket[j+3] != ObjectID[3]) { break; }
        			if (CurrentPacket[j+4] != ObjectID[4]) { break; }
        			if (CurrentPacket[j+5] != ObjectID[5]) { break; }
        			if (CurrentPacket[j+6] != ObjectID[6]) { break; }
        			if (CurrentPacket[j+7] != ObjectID[7]) { break; }
        			
        			j+=8;
        			
        			float Q1 = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float Q2 = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float Q3 = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float Q4 = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float X = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float Y = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			float Z = BitConverter.ToSingle(CurrentPacket, j);
        			j+=4;
        			String s = BitConverter.ToString(CurrentPacket, j, 4);
        			return new SceneObject(Q1,Q2,Q3,Q4,X,Y,Z,s);
        			
        		}
        		
        	}
        	return null;
        }
                
        //FIXME: hack
        public Stack<BaselineObject> GetBaselineObjects() {
        	
        	Stack<BaselineObject> Objects = new Stack<BaselineObject>();
        	
        	for (int i =0 ; i < Length; i++) {
        		
        		Byte[] CurrentPacket = GetSOEMessage(i);
        		int j =0;
        		while (j < CurrentPacket.Length) {        			
	        		if (!FindBaseline(CurrentPacket, j)) {
        				j++;
	        			continue;
	        		}
        			
	        		
        			//not enough data left for this packet
        			if ((CurrentPacket.Length-3 - j) < 21) {
        				break;
        			}
        			
        			j+=4;
        			byte[] objectId = GetObjectId(CurrentPacket, j);
        			j+=8;
	        		String objectType = GetObjectType(CurrentPacket, j);
	        		j+=5;
	        		//nothing we can do with this packet if it's missing data
	        		if (objectId == null || objectType == null) {
	        			break;
	        		}
	        		//System.Diagnostics.Debug.WriteLine(BitConverter.ToString(CurrentPacket));
	        		int DataLength = (int) BitConverter.ToUInt32(CurrentPacket, j);
	        		//if (DataLength < 0) {
	        		//	Debug.WriteLine(BitConverter.ToString(CurrentPacket, j, 4));
	        		//}
	        		
	        		j+=4;
	        		if ((CurrentPacket.Length -3 - j) < DataLength) {
	        			break;
	        		}
	        		String data = BitConverter.ToString(CurrentPacket, j, DataLength);
	        		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
	        		String name;
	        		
	        		switch (objectType) {
	        			case "BUIO03":
	        				//6: unk bytes
	        				int k = j + 6;
	        				short sl = (short) BitConverter.ToUInt16(CurrentPacket, k);
	        				k+=2;
	        				String n = enc.GetString(CurrentPacket, k, sl);
	        				k+=sl+4;
	        				short sl2 = (short) BitConverter.ToUInt16(CurrentPacket, k);
	        				k+=2;
	        				String m = enc.GetString(CurrentPacket, k, sl2);
	        				name = n + "\t" + m;
	        				break;
	        			default:
	        				name = data;
	        				break;
	        		}
	        		
	        		Objects.Push(new BaselineObject(objectType, objectId, name));
	        		
	        		j+=DataLength;
        		}
        		
        	}
        	return Objects;
        	
        }

        protected bool FindSceneCreateObjectByCRC(byte[] packet, int index) {
        	// packet not long enough
        	if ((packet.Length - index) < 4) { return false; }
        	if (packet[index] != (byte) 0xEA) { return false; }
        	if (packet[index + 1] != (byte) 0xDD) { return false; }
        	if (packet[index + 2] != (byte) 0x89) { return false; }
        	if (packet[index + 3] != (byte) 0xFE) { return false; }
        	return true;        	
        }

        
        protected bool FindBaseline(byte[] packet, int index) {
        	// packet not long enough
        	if ((packet.Length - index) < 4) { return false; }
        	if (packet[index] != (byte) 0x0C) { return false; }
        	if (packet[index + 1] != (byte) 0x5F) { return false; }
        	if (packet[index + 2] != (byte) 0xA7) { return false; }
        	if (packet[index + 3] != (byte) 0x68) { return false; }
        	return true;
        }

        protected byte[] GetObjectId(byte[] packet, int index) {
        	if ((packet.Length - index) < 8) { return null; }
        	byte[] oid = new Byte[8];
        	Array.Copy(packet, index, oid, 0, 8);
        	return oid;
        }
        
        protected String GetObjectType(byte[] packet, int index) {
        	if ((packet.Length - index) < 5) { return null; }
        	System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        	byte[] ot = new Byte[4];
        	byte[] val = new Byte[1];
        	Array.Copy(packet, index, ot, 0, 4);
        	Array.Reverse(ot);
        	Array.Copy(packet, index+4, val, 0, 1);
        	String s = enc.GetString(ot) + BitConverter.ToString(val);
        	return s;
        }


        # endregion
        # region Search func

        public Int32 SearchSOE(Byte[] array, Int32 start, Boolean caseSensitivity = true, Boolean reverse = false)
        {
            for (Int32 i = start + (reverse ? -1 : 1); i < Length && i >= 0; )
            {
                Byte[] packet = GetSOEMessage(i);
                Int32 result;
                if (caseSensitivity)
                    result = PacketHandler.IndexOf(packet, array);
                else
                {
                    Byte[] lPacket = packet.ToLower(), lArray = array.ToLower();
                    result = PacketHandler.IndexOf(lPacket, lArray);
                }
                if (result != -1) return i;
                if (reverse) i--; else i++;
            }
            return -1;
        }
        public Int32 SearchSOE(String text, Int32 start, Boolean caseSensitivity = true, Boolean reverse = false) { return SearchSOE(ASCIIEncoding.ASCII.GetBytes(text), start, caseSensitivity, reverse); }

        # endregion
        # region Private func

        private void indexUDP()
        {
            _pcapIndexes = _data.GetPCapIndexes();

            foreach (Int32 i in _pcapIndexes)
            {
                UInt16 opcode = _data.GetSOEOpcode(i);
                if (opcode == 0x0002)
                {
                    _sessRespIndexes.Add(i);
                    _srcIPAddress = _data.GetDestIP(i);
                    _srcPort = _data.GetDestPort(i);
                }
            }
        }   // Index UDP/SOE packets
        private void catalogSOE(object param)
        {
            Int32[] vals = (Int32[])param;
            for (Int32 i = vals[0]; i < vals[0] + vals[1]; i++)
                GetSOEMessage(i);
            _threadCt--;
        }   // Precache SOE packets
        private Int32 getSOESequence(Int32 index)
        {
            // if (index < 0) return -1;
            Byte[] message = GetSOEMessage(index);
            return BitConverter.ToUInt16(BitConverter.GetBytes(BitConverter.ToUInt16(GetSOEMessage(index), 2)).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        private Int32 getSOEIndex(Int32 sequence, Int32 start, Boolean reverse = false)
        {
            Int32 destIP = _data.GetDestIP(_pcapIndexes[start]);
            UInt32 destPort = _data.GetDestPort(_pcapIndexes[start]);
            UInt32 srcPort = _data.GetSourcePort(_pcapIndexes[start]);

            while (start < _pcapIndexes.Length && start > 0 && (getSOESequence(start) != sequence ||
                _data.GetDestPort(_pcapIndexes[start]) != destPort ||
                _data.GetDestIP(_pcapIndexes[start]) != destIP ||
                _data.GetSourcePort(_pcapIndexes[start]) != srcPort))
                start += reverse ? -1 : 1;

            if (start < _pcapIndexes.Length && start > 0)
                return getSOESequence(start) == sequence ? start : -1;
            else return -1;
        }

        # endregion
        # region Recover CRC
        /*
         *
         * 17-10
         * 0x04, 0x00, 0x15, 0x??
         * 
         * 35-2
         * 0x??, 0x??, 0x0A, 0x00
         * 
         * 39-2
         * 0x1C, 0x0A, 0x00, 0x08
         * 
         X 41-6
         X 0x06, 0x01, 0x00, 0xAF
         * 
         * 45
         * 0x06, 0x01, 0x00, 0xAF 
         * 
         */
        public UInt32 RecoverCRC()
        {
            UInt32 result = RecoverCRCMethod1();
            if (result == 0) result = RecoverCRCMethod2();
            if (result > 0)
            {
                Console.WriteLine("CRC recovered: {0:X8}", result);
            }
            return result;
        }
        private UInt32 RecoverCRCMethod1()
        {
            UInt32 expectedValue = 0xAF000106;
            UInt32 verificationValue = 0x1CA16CF9;

            for (Int32 i = 50; i < _pcapIndexes.Length; i++)
                if (GetSOEOpcode(i) == 0x0003 && 
                    GetUDPPDU(i).Length == 41 && 
                    (BitConverter.ToUInt32(GetUDPPDU(i),2) ^ BitConverter.ToUInt32(GetUDPPDU(i),6)) == verificationValue)
                    return BitConverter.ToUInt32(GetUDPPDU(i), 2) ^ expectedValue;
            return 0;
        }
        private UInt32 RecoverCRCMethod2()
        {
            UInt32 expectedValue = 0x00150004;
            UInt32 verificationValue = 0xA16CF9AF;
            UInt32 crc;

            for (Int32 i = 0; i < _pcapIndexes.Length; i++)
                if (GetSOEOpcode(i) == 0x0003 &&
                    GetUDPPDU(i).Length == 17 &&
                    (BitConverter.ToUInt32(GetUDPPDU(i), 6) ^ BitConverter.ToUInt32(GetUDPPDU(i), 10)) == verificationValue)
                {
                    Byte[] clientStatsMessage = getFirstClientStatsMessage();
                    if (clientStatsMessage.Length == 0) return 0;
                    crc = (BitConverter.ToUInt32(GetUDPPDU(i), 2) ^ expectedValue) & 0x00FFFFFF;
                    for (UInt32 crcFinalIndex = 0; crcFinalIndex < 256; crcFinalIndex++)
                    {
                        Byte[] result = (Byte[])clientStatsMessage.Clone();
                        SWGLib.Decrypt(ref result, (Int32)(crc | (crcFinalIndex * 0x01000000)));
                        SWGLib.Decompress1(ref result);
                        if (result.Length == 43)
                            return (crc | (crcFinalIndex * 0x01000000));
                    }
                }
            return 0;
        }
        private Byte[] getFirstClientStatsMessage()
        {
            for (Int32 i = 12; i < _pcapIndexes.Length; i++)
                if (GetSOEOpcode(i) == 0x0007)
                    return GetUDPPDU(i);
            return new Byte[] { };
        }

        # endregion
    }
    
    public class SceneObject {
    	
    	public float Q1 {
    		get; private set;
    	}
    	public float Q2 {
    		get; private set;
    	}
    	public float Q3 {
    		get; private set;
    	}
    	public float Q4 {
    		get; private set;
    	}
    	public float X {
    		get; private set;
    	}    	
    	public float Y {
    		get; private set;
    	}
    	public float Z {
    		get; private set;
    	}
    	public String CRC {
    		get; private set;
    	}
    	
    	public SceneObject(float q1, float q2, float q3, float q4, float x, float y, float z, String CRC) {
    		this.Q1 = q1;
    		this.Q2 = q2;
    		this.Q3 = q3;
    		this.Q4 = q4;
    		this.X = x;
    		this.Y = y;
    		this.Z = z;
    		this.CRC = CRC;
    	}
    	
    }
    
    public class BaselineObject {
    	
    	public String Type {
    		get; private set;
    	}
    	
    	public String ID {
    		get; private set;
    	}
    	
    	public String Contents {
    		get; private set;
    	}
    	
    	public byte[] bid {
    		get; private set;
    	}
    	
    	public BaselineObject(String Type, byte[] id, String contents) {
    		this.Type = Type;
    		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
    		this.ID = BitConverter.ToString(id);
    		this.bid = id;
    		this.Contents = contents;
    	}
    	
    }
}

