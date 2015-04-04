using System;

namespace PacketData.SOE.SWG
{
    class CmdStartScene : SWGBase
    {
        public
            static new UInt32       OpCode = 0x3AE6DFAE;

        public Byte                 Ignore_Layout_Files;
        public UInt64               Character_Id;
        public String               Terrain_File, Shared_Race_Template;
        public Single               X, Y, Z, Unknown_Field_1;
        public Byte[]               Unknown_Field_2;

        public CmdStartScene(Byte[] data) 
            : base()
        {
            Ignore_Layout_Files =   data.ToByte(ref pos);
            Character_Id =          data.ToUInt64(ref pos);
            Terrain_File =          data.ToString(ref pos);
            X =                     data.ToSingle(ref pos);
            Y =                     data.ToSingle(ref pos);
            Z =                     data.ToSingle(ref pos);
            Unknown_Field_1 =       data.ToSingle(ref pos);
            Shared_Race_Template =  data.ToString(ref pos);
            Unknown_Field_2 =       data.SubArray(ref pos, 12);
        }
    }
}