using System;

namespace STDFLib2
{
    public class MRRSurrogate : Surrogate<MRR>
    {
        public override void GetObjectData(MRR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.FINISH_T);
            SerializeValue(1, obj.DISP_COD);
            SerializeValue(2, obj.USR_DESC);
            SerializeValue(3, obj.EXC_DESC);
        }

        public override void SetObjectData(MRR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.FINISH_T = DeserializeValue<DateTime>(0);
            obj.DISP_COD = DeserializeValue<char>(1);
            obj.USR_DESC = DeserializeValue<string>(2);
            obj.EXC_DESC = DeserializeValue<string>(3);
        }
    }
}
