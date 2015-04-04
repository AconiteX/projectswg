using System;

namespace PacketData.SOE.SWG
{
    class ClientIdMsg : SWGBase
    {
        public
            static new UInt32   OpCode = 0xD5899226;

        public UInt32           Session_Key_Size;
        public Byte[]           Session_Key;
        public String           Version;

        public ClientIdMsg(Byte[] data)
            : base()
        {
            pos += 4;
            Session_Key_Size =  data.ToUInt32(ref pos);
            Session_Key =       data.SubArray(ref pos, (Int32)Session_Key_Size);
            Version =           data.ToString(ref pos);
        }
    }
}