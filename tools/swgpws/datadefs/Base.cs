using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacketData
{
    public static class Extensions
    {
        public static Byte[] SubArray(this Byte[] data, ref Int32 pos, Int32 len)
        {
            Byte[] newData = new Byte[len];
            Buffer.BlockCopy(data, pos, newData, 0, len);
            pos += len;
            return newData;
        }
        public static Byte ToByte(this Byte[] data, ref Int32 pos) {  return data[pos++]; }
        public static UInt16 ToUInt16(this Byte[] data, ref Int32 pos)
        {
            UInt16 val = BitConverter.ToUInt16(data, pos);
            pos += 2;
            return val;
        }
        public static Int32 ToInt32(this Byte[] data, ref Int32 pos)
        {
            Int32 val = BitConverter.ToInt32(data, pos);
            pos += 4;
            return val;
        }
        public static UInt32 ToUInt32(this Byte[] data, ref Int32 pos)
        {
            UInt32 val = BitConverter.ToUInt32(data, pos);
            pos += 4;
            return val;
        }
        public static UInt64 ToUInt64(this Byte[] data, ref Int32 pos)
        {
            UInt64 val = BitConverter.ToUInt64(data, pos);
            pos += 8;
            return val;
        }
        public static Single ToSingle(this Byte[] data, ref Int32 pos)
        {
            Single val = BitConverter.ToSingle(data, pos);
            pos += 4;
            return val;
        }
        public static Double ToDouble(this Byte[] data, ref Int32 pos)
        {
            Double val = BitConverter.ToDouble(data, pos);
            pos += 8;
            return val;
        }
        public static String ToString(this Byte[] data, ref Int32 pos)
        {
            UInt16 len = ToUInt16(data, ref pos);
            if (len > data.Length - pos || len == 0) return null;

            String val = System.Text.Encoding.ASCII.GetString(data, pos, len);
            pos += len;
            return val;
        }
        public static String ToUString(this Byte[] data, ref Int32 pos)
        {
            UInt32 len = ToUInt32(data, ref pos) * 2;
            if (len > data.Length - pos) return null;
            String val = System.Text.Encoding.Unicode.GetString(data, pos, (Int32)len);
            pos += (Int32)len;
            return val;
        }
        public static UInt16 ToNetOrder(this UInt16 val)
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        public static UInt32 ToNetOrder(this UInt32 val)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        public static UInt64 ToNetOrder(this UInt64 val)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        public static Int32 ToNetOrder(this Int32 val)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(val).Reverse<Byte>().ToArray<Byte>(), 0);
        }
        public static Byte[] Reverse(this Byte[] data)
        {
            Array.Reverse(data);
            return data;            
        }
        public static String GetString(this Byte[] data)
        {
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }
    public class Base
    {
        protected Int32 pos = 0;
        
        public override String ToString() { return GetType().Name; }
    }

    namespace SOE
    {
        public class SOEBase : Base
        {
            public 
                static UInt16 OpCode = 0x0000;
            public SOEBase()
                : base()
            {
                pos += 2;
            }
        }
        public class SessionRequest : SOEBase
        {
            public
                static new UInt16   OpCode = 0x0100;
            public UInt32           CRC_Length;
            public UInt32           Connection_ID;
            public UInt32           Client_UDP_Size;

            public SessionRequest(Byte[] data)
                : base()
            {
                CRC_Length =        data.ToUInt32(ref pos).ToNetOrder();
                Connection_ID =     data.ToUInt32(ref pos).ToNetOrder();
                Client_UDP_Size =   data.ToUInt32(ref pos).ToNetOrder();
            }
        }
        public class SessionResponse : SOEBase
        {
            public
                static new UInt16   OpCode = 0x0200;
            public UInt32           Connection_ID;
            public UInt32           CRC_Seed;
            public Byte             CRC_Length;
            public UInt16           Encryption;
            public UInt32           Server_UDP_Size;

            public SessionResponse(Byte[] data)
                : base()
            {
                Connection_ID =     data.ToUInt32(ref pos).ToNetOrder();
                CRC_Seed =          data.ToUInt32(ref pos).ToNetOrder();
                CRC_Length =        data.ToByte(ref pos);
                Encryption =        data.ToUInt16(ref pos);
                Server_UDP_Size =   data.ToUInt32(ref pos).ToNetOrder();
            }
        }
        public class MultiPacket : SOEBase
        {
            public
                static new UInt16   OpCode = 0x0300;
            public Byte[][]         Messages;
            private List<Byte[]>    _messages = new List<Byte[]>();

            public MultiPacket(Byte[] data)
                : base()
            {
                Byte len =          1;
                while (len > 0 && pos < data.Length - 3)
                {
                    len =           data.ToByte(ref pos);
                    _messages.Add(data.SubArray(ref pos, len));
                }
                Messages =          _messages.ToArray();
            }
        }
        public class Disconnect : SOEBase
        {
            public
                static new UInt16   OpCode = 0x0500;
            public UInt32           Connection_ID;
            public UInt16           Reason_ID;

            public Disconnect(Byte[] data)
                : base()
            {
                Connection_ID =     data.ToUInt32(ref pos).ToNetOrder();
                Reason_ID =         data.ToUInt16(ref pos).ToNetOrder();
            }
        }
        public class Ping : SOEBase
        {
            public
                static new UInt16 OpCode = 0x0600;

            public Ping(Byte[] data)
                : base()
            {
            }
        }
        public class NetworkStatsClient : SOEBase
        {
            public
                static new UInt16               OpCode = 0x0700;
            public UInt16                       Client_Tick_Count;
            public UInt32                       Last_Client_Update_Time;
            public UInt32                       Average_Client_Update_Time;
            public UInt32                       Shortest_Client_Update_Time;
            public UInt32                       Longest_Client_Update_Time;
            public UInt32                       Last_Server_Update_Time;
            public UInt64                       Packets_Sent;
            public UInt64                       Packets_Received;

            public NetworkStatsClient(Byte[] data)
                : base()
            {
                Client_Tick_Count =             data.ToUInt16(ref pos).ToNetOrder();
                Last_Client_Update_Time =       data.ToUInt32(ref pos).ToNetOrder();
                Average_Client_Update_Time =    data.ToUInt32(ref pos).ToNetOrder();
                Shortest_Client_Update_Time =   data.ToUInt32(ref pos).ToNetOrder();
                Longest_Client_Update_Time =    data.ToUInt32(ref pos).ToNetOrder();
                Last_Server_Update_Time =       data.ToUInt32(ref pos).ToNetOrder();
                Packets_Sent =                  data.ToUInt64(ref pos).ToNetOrder();
                Packets_Received =              data.ToUInt64(ref pos).ToNetOrder();
            }
        }
        public class NetworkStatsServer : SOEBase
        {
            public
                static new UInt16 OpCode = 0x0800;
            public UInt16                   Client_Tick_Count;
            public UInt32                   Unknown;
            public UInt64                   Client_Packets_Sent;
            public UInt64                   Client_Packets_Received;
            public UInt64                   Server_Packets_Sent;
            public UInt64                   Server_Packets_Received;

            public NetworkStatsServer(Byte[] data)
                : base()
            {
                Client_Tick_Count =         data.ToUInt16(ref pos).ToNetOrder();
                Unknown =                   data.ToUInt32(ref pos).ToNetOrder();
                Client_Packets_Sent =       data.ToUInt64(ref pos).ToNetOrder();
                Client_Packets_Received =   data.ToUInt64(ref pos).ToNetOrder();
                Server_Packets_Sent =       data.ToUInt64(ref pos).ToNetOrder();
                Server_Packets_Received =   data.ToUInt64(ref pos).ToNetOrder();
            }
        }
        public class DataPacketA : SOEBase
        {
            public
                static new UInt16   OpCode = 0x0900;
            public UInt16           Sequence;
            public UInt16           Priority;
            public Byte[][]         Messages;

            private List<Byte[]>    _messages = new List<Byte[]>();

            public DataPacketA(Byte[] data) 
                : base()
            {
                // DataPacket will need to host array of objects
                UInt16 len = 1;
                Sequence =          data.ToUInt16(ref pos).ToNetOrder();
                Priority =          data.ToUInt16(ref pos).ToNetOrder();

                if (Priority != 0x0019)
                {
                    pos -= 2;
                    _messages.Add(data.SubArray(ref pos, data.Length - pos));
                }
                else
                    while (len > 0 && pos < data.Length - 3)
                    {
                        len =       (Byte)data.ToByte(ref pos);

                        if (len == 0xFF)
                            len = data.ToUInt16(ref pos).ToNetOrder();

                        _messages.Add(data.SubArray(ref pos, len));
                    }
                Messages =          _messages.ToArray();
            }
        }
        public class FragPacketA : SOEBase
        {
            public
                static new UInt16 OpCode = 0x0D00;
            public UInt16 Sequence;
            public UInt32 Data_Length;
            public UInt16 Priority;
            public Byte[] Message;

            public FragPacketA(Byte[] data) 
                : base()
            {
                Sequence = data.ToUInt16(ref pos).ToNetOrder();
                Data_Length = data.ToUInt32(ref pos).ToNetOrder();
                Priority = BitConverter.ToUInt16(data, pos);
                Message = data.SubArray(ref pos, data.Length - pos);
            }
        }
        public class OutOfOrderA : SOEBase
        {
            public
                static new UInt16 OpCode = 0x1100;
            public UInt16 Sequence;

            public OutOfOrderA(Byte[] data)
                : base()
            {
                Sequence = data.ToUInt16(ref pos).ToNetOrder();
            }
        }
        public class AcknowledgementA : SOEBase
        {
            public
                static new UInt16   OpCode = 0x1500;
            public UInt16           Sequence;

            public AcknowledgementA(Byte[] data)
                : base()
            {
                Sequence =          data.ToUInt16(ref pos).ToNetOrder();
            }
        }
        public class UnknownA : SOEBase
        {
            public
                static new UInt16   OpCode = 0x1900;
            public Byte[]           Blob;

            public UnknownA(Byte[] data)
                : base()
            {
                Blob =              data.SubArray(ref pos, data.Length);
            }
        }
        public class FatalError : SOEBase
        {
            public
                static new UInt16 OpCode = 0x1D00;

            public FatalError(Byte[] data)
                : base()
            {
            }
        }
        public class FatalErrorReply : SOEBase
        {
            public
                static new UInt16 OpCode = 0x1E00;

            public FatalErrorReply(Byte[] data)
                : base()
            {
            }
        }

        namespace SWG
        {
            public class SWGBase : Base
            {
                public
                    static UInt32   OpCode = 0x00000000;

                public SWGBase() 
                    : base()
                {
                    pos += 4; 
                }
            }
        }
    }
}

