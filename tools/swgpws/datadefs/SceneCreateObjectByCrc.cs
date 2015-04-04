using System;

namespace PacketData.SOE.SWG
{
    class SceneCreateObjectByCrc : SWGBase
    {
        public
            static new UInt32   OpCode = 0xFE89DDEA;

        public UInt64           Object_Id;
        public Single           Orientation_X, Orientation_Y, Orientation_Z, Orientation_W;
        public Single           Position_X, Position_Y, Position_Z;
        public UInt32           Object_CRC;
        public Byte             Byte_Flag;

        public SceneCreateObjectByCrc(Byte[] data) 
            : base()
        {
            Object_Id =         data.ToUInt64(ref pos);
            Orientation_X =     data.ToSingle(ref pos);
            Orientation_Y =     data.ToSingle(ref pos);
            Orientation_Z =     data.ToSingle(ref pos);
            Orientation_W =     data.ToSingle(ref pos);
            Position_X =        data.ToSingle(ref pos);
            Position_Y =        data.ToSingle(ref pos);
            Position_Z =        data.ToSingle(ref pos);
            Object_CRC =        data.ToUInt32(ref pos);
            Byte_Flag =         data.ToByte(ref pos);
        }
    }
}