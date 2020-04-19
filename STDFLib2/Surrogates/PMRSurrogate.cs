namespace STDFLib2
{
    public class PMRSurrogate : Surrogate<PMR>
    {
        public override void GetObjectData(PMR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.PMR_INDX);
            SerializeValue(1, obj.CHAN_TYP);
            SerializeValue(2, obj.CHAN_NAM);
            SerializeValue(3, obj.PHY_NAM);
            SerializeValue(4, obj.LOG_NAM);
            SerializeValue(5, obj.HEAD_NUM);
            SerializeValue(6, obj.SITE_NUM);
        }

        public override void SetObjectData(PMR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.PMR_INDX = DeserializeValue<ushort>(0);
            obj.CHAN_TYP = DeserializeValue<ushort>(1);
            obj.CHAN_NAM = DeserializeValue<string>(2);
            obj.PHY_NAM  = DeserializeValue<string>(3);
            obj.LOG_NAM  = DeserializeValue<string>(4);
            obj.HEAD_NUM = DeserializeValue<byte>(5);
            obj.SITE_NUM = DeserializeValue<byte>(6);
        }
    }

}
