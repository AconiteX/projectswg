using System;

namespace PacketData.SOE.SWG
{
    class UpdateContainmentMessage : SWGBase
    {
        public
            static new UInt32       OpCode = 0x56CBDE9E;

        public UInt64               Object_ID;
        public UInt64               Containment_Id;
        public UInt32               Slot_Index;
		
        public UpdateContainmentMessage(Byte[] data) 
            : base()
        {
            Object_ID =             data.ToUInt64(ref pos);
		    Containment_Id =        data.ToUInt64(ref pos);
		    Slot_Index =            data.ToUInt32(ref pos);
        }
    }
}

