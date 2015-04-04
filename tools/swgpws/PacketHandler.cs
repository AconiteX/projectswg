using System;
using System.Collections.Generic;
using System.Linq;

namespace SWGPacket_Workshop
{
    static class PacketHandler
    {
        # region Constants
        const Int32 PCAP_FILE_HEADER_LEN = 24;
        const Int32 PCAP_HEADER_LEN = 16;
        const Int32 ETH_HEADER_LEN = 14;
        const Int32 ETH_PACKET_TYPE = 12;
        const Int32 IP_SEGMENT_TYPE = 12;
        const Int32 IP_SOURCE_POS = 12;
        const Int32 IP_DEST_POS = 16;
        const Int32 UDP_HEADER_LEN = 8;
        # endregion

        # region PCap func

        // Returns array of starting offsets of each PCap packet
        public static unsafe Int32[] GetPCapIndexes(this Byte[] data)
        {
            Boolean netOrder = data[0] == 0xa1;
            // Refactor to include only UDP packets from SWG server
            DateTime start = DateTime.Now;

            Int32[] indexes = new Int32[data.Length];
            fixed (Byte* p = data) fixed (Int32* i = indexes)       // Fix managed types
            {
                Byte* pI = p + PCAP_FILE_HEADER_LEN;                // First PCap packet appears after PCap header
                Int32* iI = i;
                for (; (pI - p) < data.Length;
                    pI += netOrder ? (*(Int32*)(pI + 8)).ToNetOrder() + PCAP_HEADER_LEN : *(Int32*)(pI + 8) + PCAP_HEADER_LEN)
                    if (getSegmentType(ref data, (Int32)(pI - p)) == 0x11)
                        *(iI++) = (Int32)(pI - p);
                Array.Resize<Int32>(ref indexes, (Int32)(iI - i));
            }

            if (Properties.Settings.Default.profiling)
                Console.WriteLine("{0} PCap packets indexed in {1:0.000}s", indexes.Length, (DateTime.Now - start).TotalSeconds);

            return indexes;
        }
        
        # endregion
        # region ETH func

        private static unsafe UInt16 getPacketType(this Byte[] data, Int32 offset)
        {
            fixed (Byte* p = data)
                return *((UInt16*)p + offset + PCAP_HEADER_LEN + ETH_PACKET_TYPE);
        }

        # endregion
        # region IP func

        private static unsafe Byte getSegmentType(ref Byte[] data, Int32 offset)
        {   // UDP = 0x11, TCP = 0x06
            switch (getIPVersion(ref data, offset))
            {
                case 0x06:
                    return data[offset + getIPHeaderOffset(data, offset) + 6];
                default:
                    return data[offset + getIPHeaderOffset(data, offset) + 9];
            }
        }
        private static Int32 getIPHeaderOffset(Byte[] data, Int32 offset)
        {
            if (data[offset + PCAP_HEADER_LEN] >> 4 == 4 || data[offset + PCAP_HEADER_LEN] >> 4 == 6)
                return PCAP_HEADER_LEN;
            else
                return PCAP_HEADER_LEN + ETH_HEADER_LEN;
        }
        private static unsafe Byte getIPVersion(ref Byte[] data, Int32 offset)
        {
            Int32 ipHeaderOffset = getIPHeaderOffset(data, offset);

            return (Byte)(data[offset + PCAP_HEADER_LEN + ipHeaderOffset] >> 4);
        }
        private static UInt32 getIPHeaderLen(ref Byte[] data, Int32 offset) 
        {   // IPv6 = 40, IPv4 arbitrary
            return getIPVersion(ref data, offset) == 0x06 ? (UInt32)40 : (UInt32)((data[offset + getIPHeaderOffset(data, offset)] & 0x1f) << 2); 
        }
        public static unsafe Int32 GetSourceIP(this Byte[] data, Int32 offset) { return GetIP(data, offset); }
        public static unsafe Int32 GetDestIP(this Byte[] data, Int32 offset) { return GetIP(data, offset, false); }
        private static unsafe Int32 GetIP(this Byte[] data, Int32 offset, Boolean source = true)
        {
            Int32 ipOffset = getIPHeaderOffset(data, offset) + (source ? IP_SOURCE_POS : IP_DEST_POS);
            fixed (Byte* p = data)
            {
                Byte* pI = p + offset + ipOffset;
                return *(Int32*)pI;
            }
            
        }
        public static UInt16 GetSourcePort(this Byte[] data, Int32 offset) { return GetPort(data, offset).ToNetOrder(); }
        public static UInt16 GetDestPort(this Byte[] data, Int32 offset) { return GetPort(data, offset, false).ToNetOrder(); }
        private static unsafe UInt16 GetPort(this Byte[] data, Int32 offset, Boolean source = true)
        {
            Int32 udpOffset = getIPHeaderOffset(data, offset) + (Int32)getIPHeaderLen(ref data, offset);
            if (!source) udpOffset += 2;
            fixed (Byte* p = data)
            {
                Byte* pI = p + offset + udpOffset;
                return *(UInt16*)pI;
            }
        }
        
        # endregion
        # region UDP func

        // Returns UDP packet from packet capture data and offset marking start of PCap packet
        public static Byte[] GetUDPPDU(this Byte[] data, Int32 offset)
        {
            Byte[] packet = new Byte[getUDPDataLen(data, offset)];
            Buffer.BlockCopy(data, offset + (Int32)getIPHeaderLen(ref data, offset) + getIPHeaderOffset(data, offset) + UDP_HEADER_LEN, packet, 0, packet.Length);
            return packet;
        }

        // Returns length of UDP data from packet capture data and offset marking start of PCap packet
        private static unsafe Int32 getUDPDataLen(this Byte[] data, Int32 offset)
        {
            fixed (Byte* p = data)
            {
                Byte* pI = p + offset + getIPHeaderOffset(data, offset) + getIPHeaderLen(ref data, offset) + 0x04;
                return System.Net.IPAddress.HostToNetworkOrder((Int16)(*((Int16*)pI))) - UDP_HEADER_LEN;
            }
        }

        # endregion
        # region SOE func

        // Returns SOE opcode from packet capture data and offset marking start of PCap packet
        public static unsafe UInt16 GetSOEOpcode(this Byte[] data, Int32 offset)
        {
            fixed (Byte* p = data)
            {
                Byte* pI = p + offset + getIPHeaderOffset(data, offset) + getIPHeaderLen(ref data, offset) + UDP_HEADER_LEN;
                return (UInt16)System.Net.IPAddress.HostToNetworkOrder(((Int16)(*((Int16*)pI))));
            }
        }

        // Return SOE packet from packet capture data and offset marking start of PCap packet
        public static Byte[] GetSOEMessage(this Byte[] data, Int32 offset, Int32 crcSeed)
        {
            Byte[] packet = GetUDPPDU(data, offset);
            if (packet[0] == 0 && packet[1] > 2 && packet[1] < 0x1F && packet.Length > 2)
            {
                SWGLib.Decrypt(ref packet, crcSeed);        // If this fails, packet will not decompress!
                if (packet[packet.Length - 3] == 1 && crcSeed != 0x00000000)
                    SWGLib.Decompress(ref packet);
            }
            return packet;
        }

        // Return SOE CRC seed from packet capture data and offset marking start of PCap packet
        // offset marks packet containing SOE Session Response packet containing CRC seed
        public static unsafe Int32 GetSOECRCSeed(this Byte[] data, Int32 offset)
        {
            if (offset == 0) return 0;
            fixed (Byte* p = data)
            {
                Byte* pI = p + offset + getIPHeaderOffset(data, offset) + getIPHeaderLen(ref data, offset) + UDP_HEADER_LEN + 6;
                return (Int32)System.Net.IPAddress.HostToNetworkOrder(*((Int32*)pI));
            }
        }

        # endregion
        # region Search func

        public static Int32 IndexOf(this Byte[] source, String criteria, Int32 offset = 0) { return IndexOf(source, System.Text.ASCIIEncoding.ASCII.GetBytes(criteria), offset); }
        public static unsafe Int32 IndexOf(this Byte[] source, Byte[] criteria, Int32 offset = 0)
        {
            if (source == null || criteria == null)
                throw new Exception("Param must not be null");
            fixed (Byte* s = source, c = criteria)
            {
                Int32 i = 0;
                for (Byte* sN = s + offset, sE = s + source.Length, sI, cI, cE; sN < sE; i++, sN++)
                {
                    Boolean match = true;
                    for (sI = sN, cI = c, cE = c + criteria.Length; match && cI < cE; match = *cI == *sI, sI++, cI++) ;
                    if (match) return i;
                }
            }
            return -1;
        }
        public static Byte[] ToLower(this Byte[] source) {return System.Text.ASCIIEncoding.ASCII.GetBytes(System.Text.ASCIIEncoding.ASCII.GetString(source).ToLower()); }
        public static Byte[] GetBytesFromHex(this String source)
        {
            try
            {
                Byte[] b = new byte[source.Length / 2];
                for (Int32 i = 0; i < source.Length; i += 2)
                    b[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
                return b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        } 

        # endregion
        # region Misc func

        public static UInt16 ToNetOrder(this UInt16 val)
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        public static Int32 ToNetOrder(this Int32 val)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }

        # endregion
    }
}
