using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace STDFLib
{
    /// <summary>
    /// Holds serialization data used to serialize/deserialize an object 
    /// </summary>
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

        private readonly SerializationInfoEntry[] Info;
        private static readonly Dictionary<string, SerializationInfo> InfoCache = new Dictionary<string, SerializationInfo>();
        
        protected IFormatterConverter Converter { get; private set; }
        
        /// <summary>
        /// Type of object that this SerializationInfo object holds data for.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Returns a shallow copy of the SerializationInfo object.
        /// </summary>
        /// <returns></returns>
        public SerializationInfo Clone()
        {
            SerializationInfo clone = new SerializationInfo(Type, Converter);
            for (int i = 0; i < Info.Length; i++)
            {
                clone.Info[i].Name = (string)Info[i].Name;
                clone.Info[i].Type = Info[i].Type;
                clone.Info[i].Value = Info[i].Value;
                clone.Info[i].MissingValue = Info[i].MissingValue;
            }
            return clone;
        }

        /// <summary>
        /// Static method to create and return a new SerializationInfo object for the given type.
        /// </summary>
        /// <param name="objType">Type of object to initialize the SerializationInfo object for.</param>
        /// <param name="converter">IFormatterConverter used to convert datatypes for serialization/deserialization.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a SerializationInfoEntry at the given index.
        /// </summary>
        /// <param name="index">Index of the entry to get.</param>
        /// <returns></returns>
        public SerializationInfoEntry GetEntry(int index)
        {
            return Info[index];
        }

        /// <summary>
        /// Gets a SerializationInfoEntry with the given Name.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an enumerator that can be used in an iterator (like foreach) to walk through the list of SerializationInfoEntries.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Info.GetEnumerator();
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry at the given index.
        /// </summary>
        /// <param name="index">Index of the SerializationInfoEntry.</param>
        /// <returns></returns>
        public object GetMissingValue(int index)
        {
            return GetEntry(index).MissingValue;
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry at the given index.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry.</param>
        /// <returns></returns>
        public object GetMissingValue(string name)
        {
            return GetEntry(name).MissingValue;
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry at the given index.
        /// </summary>
        /// <typeparam name="T">Type to convert the returned value to.</typeparam>
        /// <param name="index">Index of the SerializationInfoEntry.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public T GetMissingValue<T>(int index)
        {
            object value = GetMissingValue(index);
            return (value == null) ? default : (T)Converter.Convert(value, typeof(T));
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry with the given name.
        /// </summary>
        /// <typeparam name="T">Type to convert the returned value to.</typeparam>
        /// <param name="name">Name of the SerializationInfoEntry to get the value from.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public T GetMissingValue<T>(string name)
        {
            return GetMissingValue<T>(GetValueIndex(name));
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry at the given index.
        /// </summary>
        /// <param name="index">Index of the SerializationInfoEntry.</param>
        /// <returns></returns>
        public object GetValue(int index)
        {
            return Info[index]?.Value;
        }

        /// <summary>
        /// Gets the value for the SerializationInfoEntry with the given name.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry to get the value from.</param>
        /// <returns></returns>
        public object GetValue(string name)
        {
            return GetValue(GetValueIndex(name));
        }

        /// <summary>
        /// Gets the missing value representation for the SerializationInfoEntry at the given index.
        /// </summary>
        /// <typeparam name="T">Type to convert the returned value to.</typeparam>
        /// <param name="index">Index of the SerializationInfoEntry.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public T GetValue<T>(int index)
        {
            object value = GetValue(index);
            return (value == null) ? default : (T)Converter.Convert(value, typeof(T));
        }

        /// <summary>
        /// Gets the value for the SerializationInfoEntry with the given name.
        /// </summary>
        /// <typeparam name="T">Type to convert the returned value to.</typeparam>
        /// <param name="name">Name of the SerializationInfoEntry to get the value from.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public T GetValue<T>(string name)
        {
            return GetValue<T>(GetValueIndex(name));
        }

        /// <summary>
        /// Gets the index of the SerializationInfoEntry with the given name.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry to get the index of.</param>
        /// <returns></returns>
        public int GetValueIndex(string name)
        {
            for (int i = 0; i < Length; i++)
            {
                if (Info[i]?.Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns a boolean value indicating whether the Value property of the SerializationInfoEntry has been set by SetValue.
        /// </summary>
        /// <param name="index">Index of the SerializationInfoEntry to return the status for.</param>
        /// <returns></returns>
        public bool IsValueSet(int index)
        {
            return GetEntry(index).IsSet;
        }

        /// <summary>
        /// Returns a boolean value indicating whether the Value property of the SerializationInfoEntry has been set by SetValue.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry to return the status for.</param>
        /// <returns></returns>
        public bool IsValueSet(string name)
        {
            return IsValueSet(GetValueIndex(name));
        }

        /// <summary>
        /// Returns the number of entries in the SerializationInfoEntry object.
        /// </summary>
        public int Length => Info.Length;

        /// <summary>
        /// Sets the missing value representation for a SerializationInfoEntry.  This overrides the default missing value representation
        /// set for the related field/object Type this SerializationInfo represents.
        /// </summary>
        /// <param name="index">Index of the SerializationInfoEntry to modify</param>
        /// <param name="missingValue">New value to use to represent missing values for serialization/deserialization</param>
        public void SetMissingValue(int index, object missingValue)
        {
            if (Info[index] != null)
            {
                Info[index].MissingValue = missingValue;
            }
        }

        /// <summary>
        /// Sets the missing value representation for a SerializationInfoEntry.  This overrides the default missing value representation
        /// set for the related field/object Type this SerializationInfo represents.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry to modify</param>
        /// <param name="missingValue">New value to use to represent missing values for serialization/deserialization</param>
        public void SetMissingValue(string name, object missingValue)
        {
            SetMissingValue(GetValueIndex(name), missingValue);
        }


        /// <summary>
        /// Sets the current value of a SerializationInfoEntry.
        /// </summary>
        /// <param name="index">Index of the SerializationInfoEntry to modify.</param>
        /// <param name="value">Value to set the SerializationInfoEntry to to.</param>
        public void SetValue(int index, object value)
        {
            if (Info[index] != null)
            {
                Info[index].Value = value;
                Info[index].IsSet = true;
            }
        }

        /// <summary>
        /// Sets the current value of a SerializationInfoEntry.
        /// </summary>
        /// <param name="name">Name of the SerializationInfoEntry to modify.</param>
        /// <param name="value">Value to set the SerializationInfoEntry to to.</param>
        public void SetValue(string name, object value)
        {
            SetValue(GetValueIndex(name), value);
        }
    }
}
