using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace STDFLib2
{
    public class SerializationInfo : IEnumerable
    {
        protected SerializationInfo(Type objType, IFormatterConverter converter)
        {
            Type = objType;
            Converter = converter;

            // create an instance of the object to get missing values from the field initializers.
            object instance = STDFFormatterServices.GetUninitializedObject(Type);

            FieldInfo[] fields = STDFFormatterServices.GetSerializeableFields(Type);

            Info = new SerializationInfoEntry[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                object value = field.GetValue(instance);
                string itemCountField = (field.GetCustomAttribute<STDFAttribute>() ??
                                         field.GetCustomAttribute<STDFOptionalAttribute>())?.ItemCountProperty;
                var entry = new SerializationInfoEntry()
                {
                    Index = i,
                    Name = field.Name,
                    Type = field.FieldType,
                    IsOptional = field.GetCustomAttribute<STDFOptionalAttribute>() != null,
                    IsSet = false,
                    ItemCountIndex = Array.FindIndex(fields, x => x.Name == itemCountField),
                    Value = value,
                    MissingValue = value
                };
                Info[i] = entry;
            }
        }
        SerializationInfoEntry[] Info;
        static Dictionary<string, SerializationInfo> InfoCache = new Dictionary<string, SerializationInfo>();
        protected IFormatterConverter Converter { get; private set; } 
        public Type Type;

        public SerializationInfo Clone()
        {
            SerializationInfo clone = new SerializationInfo(Type, Converter);
            for (int i = 0; i < Info.Length; i++)
            {
                clone.Info[i].Name = Info[i].Name;
                clone.Info[i].Type = Info[i].Type;
                clone.Info[i].Value = Info[i].Value;
                clone.Info[i].MissingValue = Info[i].MissingValue;
            }
            return clone;
        }
        public static SerializationInfo Create(Type objType, IFormatterConverter converter)
        {
            if (!InfoCache.TryGetValue(objType.Name, out SerializationInfo info))
            {
                info = new SerializationInfo(objType, converter);
                InfoCache.Add(objType.Name, info);
            }
            else
            {
                if (info != null)
                {
                    // for existing info object, set the isset property to false and copy the missing value
                    // to the value to get ready for serialization/deserialization.
                    for (int i = 0; i < info.Info.Length; i++)
                    {
                        if (info.Info[i] != null)
                        {
                            info.Info[i].IsSet = false;
                            info.Info[i].Value = info.Info[i].MissingValue;
                        }
                    }
                }
            }
            return info;
        }
        public SerializationInfoEntry GetEntry(int index) => Info[index];
        public SerializationInfoEntry GetEntry(string name)
        {
            for (int i = 0; i < Info.Length; i++)
            {
                if (Info[i]?.Name == name)
                {
                    return Info[i];
                }
            }
            return null;
        }
        IEnumerator IEnumerable.GetEnumerator() => Info.GetEnumerator();
        public object GetMissingValue(int index) => GetEntry(index).MissingValue;
        public object GetMissingValue(string name) => GetEntry(name).MissingValue;
        public T GetMissingValue<T>(int index) => (T)Converter.Convert(GetMissingValue(index), typeof(T));
        public T GetMissingValue<T>(string name) => (T)Converter.Convert(GetEntry(name).MissingValue, typeof(T));
        public object GetValue(int index)
        {
            return Info[index]?.Value;
        }
        public object GetValue(string name) => GetValue(GetValueIndex(name));
        public T GetValue<T>(int index)
        {
            object value = GetValue(index);
            return (value == null) ? default : (T)Converter.Convert(GetValue(index), typeof(T));
        }
        public T GetValue<T>(string name) => GetValue<T>(GetValueIndex(name));
        public int GetValueIndex(string name)
        {
            for (int i=0;i<Length;i++)
            {
                if (Info[i]?.Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        public bool IsValueSet(int index) => GetEntry(index).IsSet;
        public bool IsValueSet(string name) => IsValueSet(GetValueIndex(name));
        public bool IsValueOptional(int index) => GetEntry(index).IsOptional;
        public bool IsValueOptional(string name) => IsValueOptional(GetValueIndex(name));
        public int Length => Info.Length;
        public void SetMissingValue(int index, object missingValue)
        {
            if (Info[index] != null)
            {
                Info[index].MissingValue = missingValue;
            }
        }
        public void SetMissingValue(string name, object missingValue)
        {
            SetMissingValue(GetValueIndex(name), missingValue);
        }
        public void SetOptional(int index, bool isOptional)
        {
            if (Info[index] != null)
            {
                Info[index].IsOptional = isOptional;
            }
        }
        public void SetOptional(string name, bool isOptional)
        {
            SetOptional(GetValueIndex(name), isOptional);
        }
        public void SetValue(int index, object value)
        {
            if (Info[index] != null)
            {
                Info[index].Value = value;
                Info[index].IsSet = true;
            }
        }
        public void SetValue(string name, object value)
        {
            SetValue(GetValueIndex(name), value);
        }        
    }
}
