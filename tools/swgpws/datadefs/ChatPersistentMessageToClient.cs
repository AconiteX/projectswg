using System;

namespace PacketData.SOE.SWG
{
    class ChatPersistentMessageToClient : SWGBase
    {
        public
            static new UInt32   OpCode = 0x08485E17;

        public String           Sender, Game, Server;
        public UInt32           Mail_Id;
        public Byte             Type;

        public ChatPersistentMessageToClient(Byte[] data) 
            : base()
        {
            Sender =            data.ToString(ref pos);
            Game =              data.ToString(ref pos);
            Server =            data.ToString(ref pos);
            Mail_Id =           data.ToUInt32(ref pos);
            Type =              data.ToByte(ref pos);
        }
    }
}