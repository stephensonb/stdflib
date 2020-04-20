namespace STDFLib
{
    /// <summary>
    /// Begin Program Sequence Record Serialization Surrogate
    /// </summary>
    public class BPSSurrogate : Surrogate<BPS>
    {
        public override void GetObjectData(BPS obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.SEQ_NAME);
        }

        public override void SetObjectData(BPS obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.SEQ_NAME = DeserializeValue<string>(0);
        }
    }
}
