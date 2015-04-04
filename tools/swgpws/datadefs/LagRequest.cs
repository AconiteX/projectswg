using System;

namespace PacketData.SOE.SWG
{
    class LagRequest : SWGBase
    {
        public
            static new UInt32 OpCode = 0x31805EE0;

        public LagRequest(Byte[] data)
            : base()
        {
            
        }
    }
}