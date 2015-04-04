using System;

namespace PacketData.SOE.SWG
{
    class SelectCharacter : SWGBase
    {
        public
            static new UInt32 OpCode = 0xB5098D76;

        public UInt64 Character_Id;

        public SelectCharacter(Byte[] data)
            : base()
        {
            Character_Id = data.ToUInt64(ref pos);
        }
    }
}