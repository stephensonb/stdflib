namespace STDFLib2
{
    public class DTRSurrogate : Surrogate<DTR>
    {
        public override void GetObjectData(DTR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.TEXT_DAT);
        }

        public override void SetObjectData(DTR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.TEXT_DAT = DeserializeValue<string>(0);
        }
    }
}
