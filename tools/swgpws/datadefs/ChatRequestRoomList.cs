using System;

namespace PacketData.SOE.SWG
{
    class ChatRequestRoomList : SWGBase
    {
        public
            static new UInt32 OpCode = 0x4C3D2CFA;

        public ChatRequestRoomList(Byte[] data)
            : base()
        {

        }
    }
}