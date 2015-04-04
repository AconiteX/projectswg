using System;
using System.Collections.Generic;

namespace PacketData.SOE.SWG
{
    class LoginClusterStatus : SWGBase
    {        
        public
            static new UInt32       OpCode = 0x3436AEB6;

        public UInt32               Server_Count;
        public Server[]             Server_List;

        private List<Server>        _servers = new List<Server>();

        public LoginClusterStatus(Byte[] data) 
            : base()
        {
            Server_Count =          data.ToUInt32(ref pos);
            
            for (Int32 i = 0; i < Server_Count; i++)
                _servers.Add(new Server(data, ref pos));

            Server_List =           _servers.ToArray();
        }

        public class Server
        {
            public UInt32           Galaxy_ID;
            public String           Server_Name;
            public UInt16           Server_Port;
            public UInt16           Ping_Port;
            public UInt32           Server_Population;
            public UInt32           Max_Capacity;
            public UInt32           Max_Characters;
            public Int32            Distance;
            public UInt32           Status;
            public Byte             Recommended;
            public Byte[]           Blob;

            public Server(Byte[] data, ref Int32 pos)
            {
                Galaxy_ID =         data.ToUInt32(ref pos);
                Server_Name =       data.ToString(ref pos);
                Server_Port =       data.ToUInt16(ref pos);
                Ping_Port =         data.ToUInt16(ref pos);
                Server_Population = data.ToUInt32(ref pos);
                Max_Capacity =      data.ToUInt32(ref pos);
                Max_Characters =    data.ToUInt32(ref pos);
                Distance =          data.ToInt32(ref pos);
                Status =            data.ToUInt32(ref pos);
                Recommended =       data.ToByte(ref pos);
                Blob =              data.SubArray(ref pos, 8);
            }
            public override String ToString()
            {
                return Server_Name;
            }
        }

    }
}
