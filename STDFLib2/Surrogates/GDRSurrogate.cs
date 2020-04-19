namespace STDFLib2
{
    public class GDRSurrogate : Surrogate<GDR>
    {
        public override void GetObjectData(GDR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.FLD_CNT);
            SerializeValue(1, obj.GEN_DATA);
        }

        public override void SetObjectData(GDR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.FLD_CNT =  DeserializeValue<ushort>(0);
            obj.GEN_DATA = DeserializeValue<VarDataField[]>(1);
        }
    }
}
