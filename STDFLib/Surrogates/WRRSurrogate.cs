using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Results Record Serialization Surrogate
    /// </summary>
    public class WRRSurrogate : Surrogate<WRR>
    {
        public override void GetObjectData(WRR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_GRP);
            SerializeValue(2, obj.FINISH_T);
            SerializeValue(3, obj.PART_CNT);
            SerializeValue(4, obj.RTST_CNT);
            SerializeValue(5, obj.ABRT_CNT);
            SerializeValue(6, obj.GOOD_CNT);
            SerializeValue(7, obj.FUNC_CNT);
            SerializeValue(8, obj.WAFER_ID);
            SerializeValue(9, obj.FABWF_ID);
            SerializeValue(10, obj.FRAME_ID);
            SerializeValue(11, obj.MASK_ID);
            SerializeValue(12, obj.USR_DESC);
            SerializeValue(13, obj.EXC_DESC);
        }

        public override void SetObjectData(WRR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_GRP = DeserializeValue<byte>(1);
            obj.FINISH_T = DeserializeValue<DateTime>(2);
            obj.PART_CNT = DeserializeValue<uint>(3);
            obj.RTST_CNT = DeserializeValue<uint>(4);
            obj.ABRT_CNT = DeserializeValue<uint>(5);
            obj.GOOD_CNT = DeserializeValue<uint>(6);
            obj.FUNC_CNT = DeserializeValue<uint>(7);
            obj.WAFER_ID = DeserializeValue<string>(8);
            obj.FABWF_ID = DeserializeValue<string>(9);
            obj.FRAME_ID = DeserializeValue<string>(10);
            obj.MASK_ID = DeserializeValue<string>(11);
            obj.USR_DESC = DeserializeValue<string>(12);
            obj.EXC_DESC = DeserializeValue<string>(13);
        }
    }
}
