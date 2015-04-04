using System;

namespace PacketData.SOE.SWG
{
    class SceneDestroyObject : SWGBase
    {
        public
            static new UInt32       OpCode = 0x4D45D504;
        
        public UInt64               Object_Id;
		
        public SceneDestroyObject(Byte[] data) 
            : base()
        {
            Object_Id =             data.ToUInt64(ref pos);
        }
    }
}

