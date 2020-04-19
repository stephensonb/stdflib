using System;
using System.Collections.Generic;

namespace STDFLib2.Serialization
{
    public class STDFTestRecordSerializationSurrogate : STDFRecordSerializationSurrogate
    {
        protected Dictionary<string, object> CurrentRecordDefaults = new Dictionary<string, object>();

        public STDFTestRecordSerializationSurrogate(ISTDFFormatterConverter converter) : base(converter) { }

        protected override STDFDeserializationContext CreateDeserializationContext(ISTDFBinaryReader reader, ISTDFRecord record)
        {
            if (record is ITestResult test)
            {
                test.TEST_NUM = reader.ReadUInt32();
                test.HEAD_NUM = reader.ReadByte();
                test.SITE_NUM = reader.ReadByte();
            }

            return base.CreateDeserializationContext(reader, record);
        }

        protected override string CreateSerializationContextKey(ISTDFRecord record)
        {
            if (record is ITestResult test)
            {
                return string.Format("{0}:{1}", test.TEST_NUM, test.GetType().Name);
            }

            return base.CreateSerializationContextKey(record);
        }

        protected override void DeserializeProperty(STDFDeserializationContext context, STDFPropertyInfo prop)
        {
            // skip the properties already deserialized
            switch(prop.Name)
            {
                case "TEST_NUM":
                case "HEAD_NUM":
                case "SITE_NUM":
                    return;
            }

            // Perform the normal deserialization
            base.DeserializeProperty(context, prop);
  
            // for properties marked optional, check if we need to use the default value (deserialized value is missing or not valid).
            if (prop.IsOptional)
            {
                // If the deserialized value from the stream represents missing or invalid data (not valid)
                if (prop.IsValid(context.Record))
                {
                    // If deserializing the first record for this context, set as the default value
                    if (context.IsFirst)
                    {
                        prop.SetDefaultValue(context.Record, context.ContextKey);
                    }
                } 
                else
                {
                    // value is missing or invalid, so try to set it to a default value
                    prop.SetValueToDefault(context.Record, context.ContextKey);
                }
            }
        }

        protected override void SerializeProperty(STDFSerializationContext context, STDFSerializationInfoEntry propValue)
        {
            if (propValue.BaseProperty.IsOptional && propValue.IsValid && propValue.IsDefault)
            {
                // if the current property is optional, valid and the value of the property is the same as the 
                // default value, then we set the value to the missing/invalid value defined for this property.
                // this will allow the record to be truncated if following values are missing/invalid also.
                propValue = new STDFSerializationInfoEntry(propValue.BaseProperty, propValue.BaseProperty.MissingOrInvalidValue, true, false);
            }
            
            // continue on with normal serialization of the property.
            base.SerializeProperty(context, propValue);
        }
    }
}
