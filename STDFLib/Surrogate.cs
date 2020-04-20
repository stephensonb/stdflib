using System;
using System.Collections.Generic;

namespace STDFLib
{
    public class Surrogate<T> : ISurrogate, ISurrogate<T> where T : STDFRecord
    {
        public Surrogate() { }

        readonly Dictionary<ulong, SerializationInfo> Defaults = new Dictionary<ulong, SerializationInfo>();
        protected SerializationInfo CurrentDefaults { get; private set; }
        protected T CurrentObject { get; private set; }
        protected SerializationInfo CurrentInfo { get; private set; }
        protected void AddDefaults()
        {
            if (CurrentDefaults == null)
            {
                ulong hash = GetHashKey();
                if (hash > 0)
                {
                    if (!Defaults.ContainsKey(hash))
                    {
                        Defaults.Add(hash, CurrentInfo.Clone());
                    }
                }
            }
        }
        protected void GetDefaults()
        {
            ulong hash = GetHashKey();
            if (hash != 0)
            {
                try
                {
                    CurrentDefaults = Defaults[hash];
                }
                catch (KeyNotFoundException)
                {
                    CurrentDefaults = null;
                }
            }
        }
        protected virtual ulong GetHashKey()
        {
            ulong hashcode = 0;
            return hashcode;
        }

        protected bool IsMissingValue(int index, object value)
        {
            return IsEqual(value, CurrentInfo.GetMissingValue(index));
        }

        protected bool IsMissingValue(string name, object value)
        {
            return IsMissingValue(CurrentInfo.GetValueIndex(name), value);
        }

        protected virtual object OnSerializingValue(int index)
        {
            return CurrentInfo.GetValue(index);
        }

        protected virtual object OnDeserializingValue(int index)
        {
            return CurrentInfo.GetValue(index);
        }

        protected virtual bool IsArrayEqual(Array array1, Array array2)
        {
            if (array1?.Length != array2?.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1.GetValue(i) != array2.GetValue(i))
                {
                    return false;
                }
            }
            return true;
        }

        protected virtual bool IsEqual(object object1, object object2)
        {
            if (object1 is Array array1 && object2 is Array array2)
            {
                return IsArrayEqual(array1, array2);
            }

            if (object1 == null)
            {
                return object1 == object2 ? true : false;
            }

            return object1.Equals(object2);
        }

        protected virtual U DeserializeValue<U>(int index)
        {
            object value = DeserializeValue(index);

            return value == null ? default : (U)value;
        }

        protected virtual object DeserializeValue(int index)
        {
            SerializationInfoEntry entry = CurrentInfo.GetEntry(index);

            object currentValue = entry.Value;
            object missingValue = entry.MissingValue;
            object defaultValue = CurrentDefaults?.GetValue(index);

            // if field is not an optional field, then no further checking required, 
            // just return the value as read from the instream (or initialized with missing value)
            if (entry.IsOptional)
            {

                // logic to determine which value to deserialize to (as read, missing, or default value)
                //
                // Truth table:
                //
                // NV = Value is null
                // MV = Value is missing value
                // DV = Value is default value
                //
                // N M D                                      
                // V V V   Action                             
                // - - -   -----------------------------------
                // 0 0 0   Return value as read                
                // 0 0 1   Return value as read            
                // 0 1 0   Return default value               
                // 0 1 1   Return default value               
                // 1 0 0   Return value as read               
                // 1 0 1   Return value as read              
                // 1 1 0   Return default value               
                // 1 1 1   Return default value               

                int writeFlag = currentValue == null ? 0b100 : 0b000;
                writeFlag |= IsEqual(currentValue, missingValue) ? 0b010 : 0b000;
                writeFlag |= IsEqual(currentValue, defaultValue) ? 0b001 : 0b000;

                switch (writeFlag)
                {
                    case 0b000:  // as read value (!= null && != missing && != default)
                    case 0b001:  // as read value (!= null && != missing && == default)
                    case 0b100:  // as read value (== null && != missing && != default)
                    case 0b101:  // as read value (== null && != missing && == default)
                        return currentValue;
                    default:      // all other combinations, return the default value.
                        return defaultValue;
                }
            }
            return currentValue;
        }

        protected virtual void SerializeValue(int index, object currentValue, object originalValue = null)
        {
            SerializationInfoEntry entry = CurrentInfo.GetEntry(index);
            object missingValue = entry.MissingValue;
            object defaultValue = CurrentDefaults?.GetValue(index);

            // if field is not an optional field, then no further checking required, 
            // just return the value as is
            if (entry.IsOptional)
            {
                // logic to determine what value to serialize
                //
                // Truth table:
                //
                // OM = Original = missing value
                // DV = Current = default value
                // MV = Current = missing value
                //
                // O D M                                          
                // M V V   Action                                 
                // - - -   -----------------------------------    
                // 0 0 0   Write current value                    
                // 0 0 1   Write missing value                    
                // 0 1 0   Write current value                    
                // 0 1 1   Write missing value                    
                // 1 0 0   Write current value                    
                // 1 0 1   Write missing value                    
                // 1 1 0   Write missing value                    
                // 1 1 1   Write missing value                    

                int writeFlag = 0b000;
                writeFlag |= IsEqual(originalValue, missingValue) ? 0b100 : 0b000;
                writeFlag |= IsEqual(currentValue, defaultValue) ? 0b010 : 0b000;
                writeFlag |= IsEqual(currentValue, missingValue) ? 0b001 : 0b000;

                switch (writeFlag)
                {
                    case 0b000:  // original value != missing && current value (!= default && != missing)
                    case 0b010:  // original value != missing && current value (== default && != missing)
                    case 0b100:  // original value == missing) && current value (!= default && != missing) 
                        entry.Value = currentValue;
                        entry.IsMissingValue = false;
                        break;
                    default:      // all other combinations, return the missing value.
                        entry.Value = missingValue;
                        entry.IsMissingValue = true;
                        break;
                }
            }
            else
            {
                entry.Value = currentValue;
                entry.IsMissingValue = IsEqual(currentValue, missingValue);
            }
        }

        protected virtual void SetOptionalFlags(T obj)
        {
            // default implementation does nothing 
            return;
        }

        public virtual void GetObjectData(T obj, SerializationInfo info)
        {
            CurrentInfo = info;
            CurrentObject = obj;
        }
        public virtual void SetObjectData(T obj, SerializationInfo info)
        {
            CurrentInfo = info;
            CurrentObject = obj;
        }

        void ISurrogate.GetObjectData(object obj, SerializationInfo info)
        {
            GetObjectData((T)obj, info);
        }

        void ISurrogate.SetObjectData(object obj, SerializationInfo info)
        {
            SetObjectData((T)obj, info);
        }
    }
}
