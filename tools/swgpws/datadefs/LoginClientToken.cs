using System;

namespace PacketData.SOE.SWG
{
    class LoginClientToken : SWGBase
    {
        public
            static new UInt32   OpCode = 0xAAB296C6;

        public UInt32           Session_Key_Size;
        public Byte[]           Session_Key;
        public UInt32           Station_Id;
        public String           Account_Name;

        public LoginClientToken(Byte[] data)
            : base()
        {
            Session_Key_Size =  data.ToUInt32(ref pos);
            Session_Key = data.SubArray(ref pos, (Int32)Session_Key_Size);
            Station_Id = data.ToUInt32(ref pos);
            Account_Name = data.ToString(ref pos);
        }
    }
}
