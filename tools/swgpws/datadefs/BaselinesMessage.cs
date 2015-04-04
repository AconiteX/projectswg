using System;

namespace PacketData.SOE.SWG
{
    class BaselinesMessage : SWGBase
    {
        public
            static new UInt32   OpCode = 0x68A75F0C;
        public UInt64           Object_Id;
        public String           Object_Type;
        public Byte             Type_Number;
        public UInt32           Object_Data_Size;
        public Object           Data;

        public BaselinesMessage(Byte[] data)
            : base()
        {
            Object_Id =         data.ToUInt64(ref pos);
            Object_Type =       data.SubArray(ref pos, 4).Reverse().GetString();
            Type_Number =       data.ToByte(ref pos);
            Object_Data_Size =  data.ToUInt32(ref pos);

            switch (Object_Type + Type_Number.ToString())
            {
                case "BUIO3":
                    Data = new BUIO3(data, ref pos);
                    break;
                case "BUIO6":
                    Data = new BUIO6(data, ref pos);
                    break;
                case "SCLT3":
                    Data = new SCLT3(data, ref pos);
                    break;
                case "SCLT6":
                    Data = new SCLT6(data, ref pos);
                    break;
                case "TANO3":
                    Data = new TANO3(data, ref pos);
                    break;
                case "TANO6":
                    Data = new TANO6(data, ref pos);
                    break;
                default:
                    Data = (Object)data.SubArray(ref pos, data.Length - pos - 3);
                    break;
            }

        }
        public class BUIO3
        {
            public UInt16 Operand_Count;
            public Single Complexity;
            public String Type, Name;

            public BUIO3(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Complexity = data.ToSingle(ref pos);
                Type = data.ToString(ref pos);
                pos += 4;
                Name = data.ToString(ref pos);
            }

        }
        public class BUIO6
        {
            public UInt16 Operand_Count;
            public UInt32 Unknown_UInt32;
            public String Type, Name;

            public BUIO6(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Unknown_UInt32 = data.ToUInt32(ref pos);
                Type = data.ToString(ref pos);
                pos += 4;
                Name = data.ToString(ref pos);
            }
        }
        public class SCLT3
        {
            public UInt16 Operand_Count;
            public UInt32 Unknown1;
            public UInt32 Unknown2;
            public UInt32 Unknown3;
            public UInt32 Unknown4;
            public UInt32 Unknown5;
            public Byte Unknown6;

            public SCLT3(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Unknown1 = data.ToUInt32(ref pos);
                Unknown2 = data.ToUInt32(ref pos);
                Unknown3 = data.ToUInt32(ref pos);
                Unknown4 = data.ToUInt32(ref pos);
                Unknown5 = data.ToUInt32(ref pos);
                Unknown6 = data.ToByte(ref pos);
            }
        }
        public class SCLT6
        {
            public UInt16 Operand_Count;
            public UInt32 Unknown1, Unknown2, Unknown3, Unknown4, Unknown5, Unknown6, Unknown7;

            public SCLT6(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Unknown1 = data.ToUInt32(ref pos);
                Unknown2 = data.ToUInt32(ref pos);
                Unknown3 = data.ToUInt32(ref pos);
                Unknown4 = data.ToUInt32(ref pos);
                Unknown5 = data.ToUInt32(ref pos);
                Unknown6 = data.ToUInt32(ref pos);
                Unknown7 = data.ToUInt32(ref pos);
            }
        }
        public class TANO3
        {
            public UInt16 Operand_Count;
            public Single Complexity;
            public String STF_Name, Default_Name, Custom_Name, Customization;
            public UInt32 Volume;
            public Byte Static;

            public TANO3(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Complexity = data.ToSingle(ref pos);
                STF_Name = data.ToString(ref pos);
                pos += 4;
                Default_Name = data.ToString(ref pos);
                pos += 4; // Custom_Name;
                Volume = data.ToUInt32(ref pos);
                pos += 30;
                pos += 4;
                Static = data.ToByte(ref pos);
            }
        }
        public class TANO6
        {
            public UInt16 Operand_Count;
            public Single Unknown1;
            public String STF_Name, Default_Name;

            public TANO6(Byte[] data, ref Int32 pos)
            {
                Operand_Count = data.ToUInt16(ref pos);
                Unknown1 = data.ToUInt32(ref pos);
                STF_Name = data.ToString(ref pos);
                pos += 4;
                Default_Name = data.ToString(ref pos);
                pos += 37;
            }
        }
    }
}
