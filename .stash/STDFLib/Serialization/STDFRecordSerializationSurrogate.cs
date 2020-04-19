using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

namespace STDFLib2.Serialization
{
    public class STDFRecordSerializationSurrogate : ISTDFSerializationSurrogate
    {
        private Dictionary<string, object> DefaultValues { get; } = new Dictionary<string, object>();

        protected string DefaultValuesKeyPrefix = "";

        protected virtual void SetDefaultValuesKeyPrefix(STDFSerializationInfo info)
        {
            DefaultValuesKeyPrefix = info.Type.Name + ":";
        }

        protected string GetDefaultValuesKey(string propertyName)
        {
            return DefaultValuesKeyPrefix + ":" + propertyName;
        }

        // Determines if the record's current value of the property referenced by propertyInfo
        // is valid or not
        protected virtual bool IsValidValue(ISTDFRecord record, PropertyInfo propInfo, object value)
        {
            // default check - override to change behavior

            // get the default value from the uninitialized record
            var defaultValue = propInfo.GetValue(record);

            // empty strings are considered missing values
            if (value is string sval && sval == "")
            {
                return false;
            }

            // null values are considered missing data
            if (value == null)
            {
                return false;
            }

            // empty array is considered missing data
            if (value is Array values && values.Length == 0)
            {
                return false;
            }

            // If the value is the same as the values from the uninitialized data
            // then consider it as missing/invalid
            if (value == defaultValue)
            {
                return false;
            }

            // seems ok, so call it valid.
            return true;
        }

        // Default implementation for missing value.  Override if you need to provide
        // other logic for providing a missing value for a property.
        protected virtual object GetMissingValue(Type type, string propertyName)
        {

            if (type.IsValueType)
            {
                return 0;
            }

            if (type.Name == "String")
            {
                return "";
            }

            if (type.IsArray)
            {
                return Array.CreateInstance(type.GetElementType(), 0);
            }

            // If a defined missing value for the object type and property is defined, then return it.
            if (STDFFormatterServices.TryGetMissingValue(type, propertyName, out object missingValue))
            {
                return missingValue;
            }

            return null;
        }

        protected bool IsDefaultValue(string propertyName, object propertyValue)
        {
            if (DefaultValues.TryGetValue(GetDefaultValuesKey(propertyName), out object defaultValue))
            {
                return true;
            }

            return false;
        }

        protected void SaveDefaultValue(string propertyName, object defaultValue)
        {
            string key = GetDefaultValuesKey(propertyName);

            if (!DefaultValues.ContainsKey(key))
            {
                DefaultValues.Add(key, defaultValue);
            }
        }

        protected bool TryGetDefaultValue(string propertyName, out object propValue)
        {
            return DefaultValues.TryGetValue(GetDefaultValuesKey(propertyName), out propValue);
        }

        protected bool HasDefaultValues(STDFSerializationInfo info)
        {
            return DefaultValues.Keys.FirstOrDefault(x => x.StartsWith(DefaultValuesKeyPrefix)) != null;
        }

        public void GetObjectData(object obj, STDFSerializationInfo info)
        {
            string lastValidProperty = "";

            if (obj is ISTDFRecord record)
            {
                //bool withDefaults = record.GetType().GetCustomAttribute<STDFDefaultsAttribute>() != null;
                //bool saveDefaults = false;

                //if (withDefaults)
                //{
                //    SetDefaultValuesKeyPrefix(info);
                //    saveDefaults = HasDefaultValues(info);
                //}

                // get data for each serializable property on the record
                foreach (var propInfo in STDFFormatterServices.GetSerializeableProperties(info.Type))
                {
                    var propValue = propInfo.GetValue(record);
                    bool isValid = IsValidValue(record, propInfo, propValue);
                    //bool isOptional = propInfo.GetCustomAttribute<STDFOptionalAttribute>() != null;

                    //// handle optional properties with valid data
                    //if (isOptional && isValid)
                    //{
                    //    // handle records with default values
                    //    if (withDefaults)
                    //    {
                    //        // If we are serializing the first record (per value returned by the DefaultsValueKeyPrefix method)
                    //        // and the value is valid, then save it as the default for future matching records
                    //        if (saveDefaults)
                    //        {
                    //            SaveDefaultValue(propInfo.Name, propValue);
                    //        }
                    //        else
                    //        {
                    //            if (IsDefaultValue(propInfo.Name, propValue))
                    //            {
                    //                propValue = GetMissingValue(info.Type, propInfo.Name);
                    //                isValid = false;
                    //            }
                    //        }
                    //    }
                    //}

                    if (isValid)
                    {
                        // If property is required or property is optional but has a valid value,
                        // then save the name or this property for later truncation of record
                        lastValidProperty = propInfo.Name;
                    }

                    // Add the property
                    info.AddValue(propInfo.Name, propInfo.GetValue(record), propInfo.PropertyType);
                }

                // now truncate the serialization data starting after the last valid property
                bool truncateProperties = false;
                foreach (var item in info)
                {
                    if (truncateProperties)
                    {
                        info.Remove(item);
                    }
                    else
                    {
                        if (item.Name == lastValidProperty)
                        {
                            truncateProperties = true;
                        }
                    }
                }
            }
        }

        public void SetObjectData(object obj, STDFSerializationInfo info, ISTDFSurrogateSelector selector)
        {
            if (obj is ISTDFRecord record)
            {
                //bool withDefaults = info.Type.GetCustomAttribute<STDFDefaultsAttribute>() != null;
                //bool saveDefaults = false;

                //if (withDefaults)
                //{
                //    SetDefaultValuesKeyPrefix(info);
                //    saveDefaults = HasDefaultValues(info);
                //}

                // get data for each serializable property on the record
                foreach (var propInfo in info)
                {
                    var targetProperty = info.Type.GetProperty(propInfo.Name);
                    var propValue = info.GetValue(propInfo.Name, propInfo.Type);
                    //bool isValid = propValue != null && IsValidValue(record, targetProperty, propValue);
                    //bool isOptional = !(targetProperty.GetCustomAttribute<STDFOptionalAttribute>() == null);

                    //// handle optional properties
                    //if (isOptional)
                    //{
                    //    // handle records with default values
                    //    if (withDefaults)
                    //    {
                    //        // If we are serializing the first record (per value returned by the DefaultsValueKeyPrefix method)
                    //        // and the value is valid, then save it as the default for future matching records
                    //        if (saveDefaults && isValid)
                    //        {
                    //            SaveDefaultValue(propInfo.Name, propValue);
                    //        }
                    //        else
                    //        {
                    //            if (!isValid)
                    //            {
                    //                // If the value is missing or invalid and we have defaults set
                    //                // then try to get a default value for this property
                    //                isValid = TryGetDefaultValue(propInfo.Name, out propValue);
                    //            }
                    //        }
                    //    }
                    //}

                    //if (isValid)
                    //{
                    //    // if the value is valid then we set the property on the record to the deserialized value
                    targetProperty.SetValue(record, propValue);
                }
            }
        }
    }
}