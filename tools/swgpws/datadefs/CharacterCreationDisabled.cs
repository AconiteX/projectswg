using System;

namespace PacketData.SOE.SWG
{
    class CharacterCreationDisabled : SWGBase
    {   // This message only contains a single string
        public
            static new UInt32 OpCode = 0xF41A5265;

        public UInt32 Unknown_Field_1;

        public CharacterCreationDisabled(Byte[] data)
            : base()
        {
            Unknown_Field_1 = data.ToUInt32(ref pos);
        }
    }
}