namespace STDFLib
{
    /// <summary>
    /// Pin Group Record Serialization Surrogate
    /// </summary>
    public class PGRSurrogate : Surrogate<PGR>
    {
        public override void GetObjectData(PGR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.GRP_INDX);
            SerializeValue(1, obj.GRP_NAM);
            SerializeValue(2, obj.INDX_CNT);
            SerializeValue(3, obj.PMR_INDX);
        }

        public override void SetObjectData(PGR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.GRP_INDX = DeserializeValue<ushort>(0);
            obj.GRP_NAM = DeserializeValue<string>(1);
            obj.INDX_CNT = DeserializeValue<ushort>(2);
            obj.PMR_INDX = DeserializeValue<ushort[]>(3);
        }
    }
}
