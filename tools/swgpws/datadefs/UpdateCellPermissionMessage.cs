using System;

namespace PacketData.SOE.SWG
{
    class UpdateCellPermissionMessage : SWGBase
    {
        public
            static new UInt32       OpCode = 0xF612499C;

		public Byte                 Permission_Flag;
        public UInt64               Cell_Id;
	
        public UpdateCellPermissionMessage(Byte[] data) 
            : base()
        {
		    Permission_Flag =       data.ToByte(ref pos);
            Cell_Id =               data.ToUInt64(ref pos);
        }
    }
}

