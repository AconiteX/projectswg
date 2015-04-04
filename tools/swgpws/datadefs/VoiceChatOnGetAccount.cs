using System;

namespace PacketData.SOE.SWG
{
    class VoiceChatOnGetAccount : SWGBase
    {   // This message probably has something to do with voice
        public
            static new UInt32   OpCode = 0x326E6B43;

        public Byte[]           Unknown_Field_1;

        public VoiceChatOnGetAccount(Byte[] data) 
            : base()
        {
            Unknown_Field_1 =   data.SubArray(ref pos, data.Length - 4);
        }
    }
}