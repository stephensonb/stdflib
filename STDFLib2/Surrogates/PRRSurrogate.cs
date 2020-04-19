namespace STDFLib2
{
    public class PRRSurrogate : Surrogate<PRR>
    {
        public override void GetObjectData(PRR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
            SerializeValue(2, obj.PART_FLG);
            SerializeValue(3, obj.NUM_TEST);
            SerializeValue(4, obj.HARD_BIN);
            SerializeValue(5, obj.SOFT_BIN);
            SerializeValue(6, obj.X_COORD);
            SerializeValue(7, obj.Y_COORD);
            SerializeValue(8, obj.TEST_T);
            SerializeValue(9, obj.PART_ID);
            SerializeValue(10,obj.PART_TXT);
            SerializeValue(11,obj.PART_FIX);
        }

        public override void SetObjectData(PRR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(1);
            obj.PART_FLG = DeserializeValue<byte>(2);
            obj.NUM_TEST = DeserializeValue<ushort>(3);
            obj.HARD_BIN = DeserializeValue<ushort>(4);
            obj.SOFT_BIN = DeserializeValue<ushort>(5);
            obj.X_COORD  = DeserializeValue<short>(6);
            obj.Y_COORD  = DeserializeValue<short>(7);
            obj.TEST_T   = DeserializeValue<uint>(8);
            obj.PART_ID  = DeserializeValue<string>(9);
            obj.PART_TXT = DeserializeValue<string>(10);
            obj.PART_FIX = DeserializeValue<ByteArray>(11);
        }
    }
}
