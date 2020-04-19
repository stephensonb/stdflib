using System;
using System.Collections.Generic;
using System.Reflection;

namespace STDFLib2.Serialization
{
    public class STDFSerializationInfoEntry
    {
        public STDFSerializationInfoEntry(string name, object value, Type propertyType)
        {
            Name = name;
            Value = value;
            Type = propertyType;
        }

        public string Name { get; }
        public object Value { get; }
        public Type Type { get; }
    }

    public class STDFPropertyInfo
    {   
        public STDFPropertyInfo(Type sourceType, PropertyInfo property)
        {
            SourceType = sourceType;
            BaseProperty = property;
            PropertyType = BaseProperty.PropertyType;
            IsOptional = property.GetCustomAttribute<STDFOptionalAttribute>() != null;
            ItemCountProperty = property.GetCustomAttribute<STDFAttribute>()?.ItemCountProperty;
        }

        public string Name { get => BaseProperty.Name; }
        public PropertyInfo BaseProperty { get; }
        public Type SourceType { get; } 
        public Type PropertyType { get; }
        public Dictionary<string, object> DefaultValues { get; private set; } = new Dictionary<string, object>();
        public object MissingOrInvalidValue { get; private set; } = null;
        public bool IsOptional { get; }
        public string ItemCountProperty { get; }

        public object GetValue(ISTDFRecord record)
        {
            return BaseProperty.GetValue(record);
        }

        public void SetValue(ISTDFRecord record, object value)
        {
            BaseProperty.SetValue(record, value);
        }

        public void SetValueToMissing(ISTDFRecord record) => BaseProperty.SetValue(record, MissingOrInvalidValue);
    
        public bool SetValueToDefault(ISTDFRecord record, string contextKey)
        {
            if (TryGetDefaultValue(contextKey, out object defaultValue))
            {
                BaseProperty.SetValue(record, defaultValue);
                return true;
            }
            return false;
        }

        public bool TryGetDefaultValue(string contextKey, out object defaultValue)
        {
            defaultValue = null;
            try
            {
                defaultValue = DefaultValues[contextKey];
            } 
            catch(KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

        public void SetDefaultValue(ISTDFRecord defaultValuesRecord, string contextKey)
        {
            // only set once.  If a default value for the key already exists, ignore the request.
            if (!DefaultValues.ContainsKey(contextKey))
            {
                DefaultValues.Add(contextKey, BaseProperty.GetValue(defaultValuesRecord));
            } 
        }
        
        public void SetMissingOrInvalidValue(ISTDFRecord missingOrInvalidValuesRecord)
        {
            MissingOrInvalidValue = BaseProperty.GetValue(missingOrInvalidValuesRecord);
        }
        
        public bool HasDefault(string contextKey) => DefaultValues.ContainsKey(contextKey);

        public bool IsDefault(ISTDFRecord record, string contextKey)
        {
            var propValue = BaseProperty.GetValue(record);
            
            if (DefaultValues.TryGetValue(contextKey, out object defaultValue))
            {
                if (propValue is Array values && values.Length == 0)
                {
                    return true;
                }
                return (propValue?.Equals(defaultValue) ?? false);
            }
            return false;
        }
        
        public bool IsValid(ISTDFRecord record)
        {
            var propValue = BaseProperty.GetValue(record);

            if (propValue is Array values && values.Length == 0)
            {
                if (values.Length == 0)
                {
                    // for arrays, always consider empty arrays as missing/invalid
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // If the property value == the missing or invalid value, then return false (not true)
            // If the property value is null then  return false.
            // Only return true if property value is not the same as the missing or invalid value
            return (!propValue?.Equals(MissingOrInvalidValue) ?? false);
        }
        
        public override string ToString() => BaseProperty.ToString();
    }
}
