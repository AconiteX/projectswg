using System;

namespace PacketData.SOE.SWG
{
    class SceneEndBaselines : SWGBase
    {
        public
            static new UInt32       OpCode = 0x2C436037;

        public UInt64               Object_Id;
		
        public SceneEndBaselines(Byte[] data) 
            : base()
        {
            Object_Id =             data.ToUInt64(ref pos);
        }
    }
}

