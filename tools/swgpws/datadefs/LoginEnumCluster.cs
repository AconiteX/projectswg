using System;
using System.Collections.Generic;

namespace PacketData.SOE.SWG
{
    class LoginEnumCluster : SWGBase
    {   // This message only contains a single string
        public
            static new UInt32       OpCode = 0xC11C63B9;

        public UInt32               Server_Count;
        public Server[]             Server_List;
        public UInt32               Max_Characters;

        private List<Server>        _servers = new List<Server>();

        public LoginEnumCluster(Byte[] data)
            : base()
        {
            Server_Count =          data.ToUInt32(ref pos);

            for (Int32 i = 0; i < Server_Count; i++)
                _servers.Add(new Server(data, ref pos));

            Server_List =           _servers.ToArray();
            Max_Characters =        data.ToUInt32(ref pos);
        }

        public class Server
        {
            public UInt32           Server_Id;
            public String           Server_Name;
            public UInt32           Unknown_Field_1;
            public Server(Byte[] data, ref Int32 pos)
            {
                Server_Id =         data.ToUInt32(ref pos);
                Server_Name =       data.ToString(ref pos);
                Unknown_Field_1 =   data.ToUInt32(ref pos);
            }
            public override String ToString()
            {
                return Server_Name;
            }
        }
    }
}
