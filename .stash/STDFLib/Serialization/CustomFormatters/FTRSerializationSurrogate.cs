using System.Reflection;

namespace STDFLib2.Serialization
{
    public class FTRSerializationSurrogate : STDFTestRecordSerializationSurrogate
    {
        public FTRSerializationSurrogate(ISTDFFormatterConverter converter) : base(converter) { }

        protected override void SerializeProperty(STDFSerializationContext context, STDFSerializationInfoEntry propValue)
        {
            bool isInvalid = false;

            if (context.Record is FTR ftr)
            {
                // if we do not need to save optional data for the record, skip serializing remaining fields
                switch (propValue.BaseProperty.Name)
                {
                    case "OPT_FLAG":
                        RecordLength = (ushort)(context.Writer.Position - RecordStartStreamPosition);
                        if (ftr.OPT_FLAG == 0)
                        {
                            ftr.OPT_FLAG = (byte)FTROptionalData.AllOptionalDataValid;
                            ftr.OPT_FLAG |= (byte)(ftr.CYCL_CNT == null ? FTROptionalData.CycleCountDataIsInvalid : 0);
                            ftr.OPT_FLAG |= (byte)(ftr.REL_VADR == null ? FTROptionalData.RelativeVectorAddressIsInvalid : 0);
                            ftr.OPT_FLAG |= (byte)(ftr.REPT_CNT == null ? FTROptionalData.RepeatCountOfVectorIsInvalid : 0);
                            ftr.OPT_FLAG |= (byte)(ftr.NUM_FAIL == null ? FTROptionalData.NumberOfFailsIsInvalid : 0);
                            ftr.OPT_FLAG |= (byte)(ftr.XFAIL_AD == null || ftr.YFAIL_AD == null ? FTROptionalData.XYFailAddressIsInvalid : 0);
                            ftr.OPT_FLAG |= (byte)(ftr.VECT_OFF == null ? FTROptionalData.VectorOffsetDataIsInvalid : 0);
                        }
                        context.Writer.Write(ftr.OPT_FLAG);
                        return;
                    case "CYCL_CNT":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.CycleCountDataIsInvalid) > 0);
                        break;
                    case "REL_VADR":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.RelativeVectorAddressIsInvalid) > 0);
                        break;
                    case "REPT_CNT":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.RepeatCountOfVectorIsInvalid) > 0) ;
                        break;
                    case "NUM_FAIL":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.NumberOfFailsIsInvalid) > 0);
                        break;
                    case "XFAIL_AD":
                    case "YFAIL_AD":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.XYFailAddressIsInvalid) > 0);
                        break;
                    case "VECT_OFF":
                        isInvalid = ((ftr.OPT_FLAG & (byte)FTROptionalData.VectorOffsetDataIsInvalid) > 0);
                        break;
                }

                if (isInvalid)
                {
                    context.Writer.Write(propValue.BaseProperty.PropertyType, propValue.BaseProperty.MissingOrInvalidValue);
                }
                else
                {
                    base.SerializeProperty(context, propValue);
                }
            }
        }
    }
}
