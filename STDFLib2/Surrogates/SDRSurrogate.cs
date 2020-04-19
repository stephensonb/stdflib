namespace STDFLib2
{
    public class SDRSurrogate : Surrogate<SDR>
    {
        public override void GetObjectData(SDR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue( 0,obj.HEAD_NUM);
            SerializeValue( 1,obj.SITE_GRP);
            SerializeValue( 2,obj.SITE_CNT);
            SerializeValue( 3,obj.SITE_NUM);
            SerializeValue( 4,obj.HAND_TYP);
            SerializeValue( 5,obj.HAND_ID );
            SerializeValue( 6,obj.CARD_TYP);
            SerializeValue( 7,obj.CARD_ID );
            SerializeValue( 8,obj.LOAD_TYP);
            SerializeValue( 9,obj.LOAD_ID );
            SerializeValue(10,obj.DIB_TYP );
            SerializeValue(11,obj.DIB_ID  );
            SerializeValue(12,obj.CABL_TYP);
            SerializeValue(13,obj.CABL_ID );
            SerializeValue(14,obj.CONT_TYP);
            SerializeValue(15,obj.CONT_ID );
            SerializeValue(16,obj.LASR_TYP);
            SerializeValue(17,obj.LASR_ID );
            SerializeValue(18,obj.EXTR_TYP);
            SerializeValue(19,obj.EXTR_ID );
        }

        public override void SetObjectData(SDR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_GRP = DeserializeValue<byte>(1);
            obj.SITE_CNT = DeserializeValue<byte>(2);
            obj.SITE_NUM = DeserializeValue<byte[]>(3);
            obj.HAND_TYP = DeserializeValue<string>(4);
            obj.HAND_ID  = DeserializeValue<string>(5);
            obj.CARD_TYP = DeserializeValue<string>(6);
            obj.CARD_ID  = DeserializeValue<string>(7);
            obj.LOAD_TYP = DeserializeValue<string>(8);
            obj.LOAD_ID  = DeserializeValue<string>(9);
            obj.DIB_TYP  = DeserializeValue<string>(10);
            obj.DIB_ID   = DeserializeValue<string>(11);
            obj.CABL_TYP = DeserializeValue<string>(12);
            obj.CABL_ID  = DeserializeValue<string>(13);
            obj.CONT_TYP = DeserializeValue<string>(14);
            obj.CONT_ID  = DeserializeValue<string>(15);
            obj.LASR_TYP = DeserializeValue<string>(16);
            obj.LASR_ID  = DeserializeValue<string>(17);
            obj.EXTR_TYP = DeserializeValue<string>(18);
            obj.EXTR_ID  = DeserializeValue<string>(19);
        }
    }
}
