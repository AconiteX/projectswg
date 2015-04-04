using System;

namespace PacketData.SOE.SWG
{
    class LoginClientId : SWGBase
    {
        public
            static new UInt32   OpCode = 0x41131F96;

        public String           Account_Name;
        public String           Password;
        public String           Version;

        public LoginClientId(Byte[] data)
            : base()
        {
            Account_Name =      data.ToString(ref pos);
            Password =          data.ToString(ref pos);
            Version =           data.ToString(ref pos);
        }
    }
}