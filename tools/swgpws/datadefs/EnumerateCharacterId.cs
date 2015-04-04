using System;
using System.Collections.Generic;

namespace PacketData.SOE.SWG
{
    class EnumerateCharacterId : SWGBase
    {
        public
            static new UInt32   OpCode = 0x65EA4574;

        public UInt32           Character_Count;
        public Character[]      Character_List;

        private List<Character> _characters = new List<Character>();

        public EnumerateCharacterId(Byte[] data)
            : base()
        {
            Character_Count =   data.ToUInt32(ref pos);
            for (Int32 i = 0; i < Character_Count; i++)
                _characters.Add(new Character(data, ref pos));

            Character_List = _characters.ToArray();
        }

        public class Character
        {
            public String           Character_Name;
            public UInt64           Character_Id;
            public UInt32           Unknown_Field_1,
                                    Galaxy_Id,
                                    Status;

            public Character(Byte[] data, ref Int32 pos)
            {
                Character_Name = data.ToUString(ref pos);
                Character_Id = data.ToUInt64(ref pos);
                Unknown_Field_1 = data.ToUInt32(ref pos);
                Galaxy_Id = data.ToUInt32(ref pos);
                Status = data.ToUInt32(ref pos);
            }
            public override String ToString()
            {
                return Character_Name;
            }
        }
    }
}