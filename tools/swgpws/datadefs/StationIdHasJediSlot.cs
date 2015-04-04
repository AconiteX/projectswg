using System;

namespace PacketData.SOE.SWG
{
    class StationIdHasJediSlot : SWGBase
    {   // This message only contains a single string
        public
            static new UInt32 OpCode = 0xCC9FCCF8;

        public UInt32 Unknown_Field_1;

        public StationIdHasJediSlot(Byte[] data)
            : base()
        {
            Unknown_Field_1 = data.ToUInt32(ref pos);
        }
    }
}