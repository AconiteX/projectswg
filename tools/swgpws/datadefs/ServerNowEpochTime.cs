using System;

namespace PacketData.SOE.SWG
{
    class ServerNowEpochTime : SWGBase
    {
        public
            static new UInt32 OpCode = 0x24B73893;

        public UInt32 Unknown_Field_1;

        public ServerNowEpochTime(Byte[] data)
            : base()
        {
            Unknown_Field_1 = data.ToUInt32(ref pos);
        }
    }
}