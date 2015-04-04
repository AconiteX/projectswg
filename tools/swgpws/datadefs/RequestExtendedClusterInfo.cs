using System;

namespace PacketData.SOE.SWG
{
    class RequestExtendedClusterInfo : SWGBase
    {
        public
            static new UInt32   OpCode = 0x8E33ED05;

        public UInt32           Unknown_Field_1;

        public RequestExtendedClusterInfo(Byte[] data)
            : base()
        {
            Unknown_Field_1 =   data.ToUInt32(ref pos);
        }
    }
}