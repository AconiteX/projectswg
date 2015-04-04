using System;

namespace PacketData.SOE.SWG
{
    class ConnectPlayerMessage : SWGBase
    {
        public
            static new UInt32 OpCode = 0x2E365218;

        public UInt32 Unknown_Field_1;

        public ConnectPlayerMessage(Byte[] data)
            : base()
        {
            Unknown_Field_1 = data.ToUInt32(ref pos);
        }
    }
}
