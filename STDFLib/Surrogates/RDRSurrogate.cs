namespace STDFLib
{
    /// <summary>
    /// Retest Data Record Serialization Surrogate
    /// </summary>
    public class RDRSurrogate : Surrogate<RDR>
    {
        public override void GetObjectData(RDR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.NUM_BINS);
            SerializeValue(1, obj.RTST_BIN);
        }

        public override void SetObjectData(RDR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.NUM_BINS = DeserializeValue<ushort>(0);
            obj.RTST_BIN = DeserializeValue<ushort[]>(1);
        }
    }
}
