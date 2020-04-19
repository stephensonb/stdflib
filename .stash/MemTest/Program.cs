using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq;

namespace MemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);



        }
    }

    public class SerializationInfoEntry
    {
        public string Name;
        public Type Type;
        public bool IsOptional;
        public bool IsSet;
        public object Value;
        public object MissingValue;
    }

    public class SerializationInfo
    {
        protected SerializationInfo(Type objType)
        {
            Type = objType;

            // create an instance of the object to get missing values from the field initializers.
            object instance = Activator.CreateInstance(Type);
            
            FieldInfo[] fields = objType.GetFields();
            
            Info = new SerializationInfoEntry[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var entry = new SerializationInfoEntry()
                {
                    Name = field.Name,
                    Type = field.FieldType,
                    IsOptional = false,
                    IsSet = false,
                    Value = null,
                    MissingValue = field.GetValue(instance)
                };
                Info[i] = entry;
            }
        }

        static Dictionary<string, SerializationInfo> InfoCache = new Dictionary<string, SerializationInfo>();
        SerializationInfoEntry[] Info;
        public Type Type;

        public SerializationInfo Clone()
        {
            SerializationInfo clone = new SerializationInfo(Type);
            for(int i=0;i<Info.Length;i++)
            {
                clone.Info[i].Name = Info[i].Name;
                clone.Info[i].Type = Info[i].Type;
                clone.Info[i].Value = Info[i].Value;
                clone.Info[i].MissingValue = Info[i].MissingValue;
            }
            return clone;
        }
        public static SerializationInfo Create(Type objType)
        {
            if (!InfoCache.TryGetValue(objType.Name, out SerializationInfo info))
            {
                info = new SerializationInfo(objType);
                InfoCache.Add(objType.Name, info);
            }
            return info;
        }
        public SerializationInfoEntry GetEntry(string name)
        {
            for(int i=0;i<Info.Length;i++)
            {
                if (Info[i].Name == name)
                {
                    return Info[i];
                }
            }
            return null;
        }
        public IEnumerator GetEnumerator => Info.GetEnumerator();
        public object GetMissingValue(int index) => Info[index].MissingValue;
        public object GetMissingValue(string name) => GetEntry(name).MissingValue;
        public object GetValue(int index) => Info[index].Value;
        public object GetValue(string name) => GetEntry(name).Value;
        public T GetMissingValue<T>(int index) => (T)Info[index].MissingValue;
        public T GetMissingValue<T>(string name) => (T)GetEntry(name).MissingValue;
        public T GetValue<T>(int index) => (T)Info[index].Value;
        public T GetValue<T>(string name) => (T)GetValue(name);
        public void Initialize()
        {
            for(int i=0;i<Info.Length;i++)
            {
                Info[i] = null;
                Info[i].IsSet = false;
            }
        }
        public bool IsValueSet(string name) => GetEntry(name).IsSet;
        public bool IsValueOptional(string name) => GetEntry(name).IsOptional;
        public int Length => Info.Length;
        public void SetMissingValue(int index, object missingValue)
        {
            Info[index].MissingValue = missingValue;
        }
        public void SetMissingValue(string name, object missingValue)
        {
            GetEntry(name).MissingValue = missingValue;
        }
        public void SetOptional(int index, bool isOptional)
        {
            Info[index].IsOptional = isOptional;
        }
        public void SetOptional(string name, bool isOptional)
        {
            GetEntry(name).IsOptional = isOptional;
        }
        public void SetValue(int index, object value)
        {
            Info[index].Value = value;
            Info[index].IsSet = true;
        }
        public void SetValue(string name, object value)
        {
            GetEntry(name).Value = value;
            GetEntry(name).IsSet = true;
        }
        public bool IsValueSet(int index) => Info[index].IsSet;
        public bool IsValueOptional(int index) => Info[index].IsOptional;
    }

    public class Record
    {
        public uint TEST_NUM;
        public byte HEAD_NUM;
        public byte SITE_NUM;
        public byte TEST_FLG;
        public byte PARM_FLG;
        public float? RESULT;
        public string TEST_TXT;
        public string ALARM_ID;
        public byte OPT_FLAG;
        public sbyte? RES_SCAL;
        public sbyte? LLM_SCAL;
        public sbyte? HLM_SCAL;
        public float? LO_LIMIT;
        public float? HI_LIMIT;
        public string UNITS; 
        public string C_RESFMT;
        public string C_LLMFMT;
        public string C_HLMFMT;
        public float? LO_SPEC;
        public float? HI_SPEC;
    }

    public class Surrogate<T> 
    {
        public Surrogate()
        {
            // intialize the serialization info structure for this type
            SerializationInfo info = SerializationInfo.Create(typeof(T));
        }

        Dictionary<ulong, SerializationInfo> Defaults = new Dictionary<ulong, SerializationInfo>();
        protected SerializationInfo CurrentDefaults { get; private set; }
        protected T CurrentObject { get; private set; }
        protected SerializationInfo CurrentInfo { get; private set; }
        
        protected void AddDefaults()
        {
            ulong hash = CreateHash();
            if (!Defaults.ContainsKey(hash))
            {
                Defaults.Add(hash, CurrentInfo.Clone());
            }
        }
        protected void SetDefaults()
        {
            CurrentDefaults = null;
            ulong hash = CreateHash();
            Defaults.TryGetValue(hash, out CurrentDefaults);
        }
        protected virtual ulong CreateHash()
        {
            ulong hashcode = 0;
                // Hashcode:  Create unsigned 64-bit integer.  In byte order (high to low:
                //            66665555 55555544 44444444 33333333 33222222 22221111 11111100 00000000
                //            32109876 54321098 76543210 98765432 10987654 32109876 54321098 76543210
                //            <---- ZEROS ----> <-HEAD-> <-SITE-> <----------- TEST NUMBER --------->  
                //hashcode = (ulong)rec.HEAD_NUM << 40 | (ulong)rec.SITE_NUM << 32 | (ulong)rec.TEST_NUM;
            return hashcode;
        }
        protected virtual object OnSerializingValue(int index)
        {
            return CurrentInfo.GetValue(index);
        }
        protected virtual object OnDeserializingValue(int index)
        {
            return CurrentInfo.GetValue(index);
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
    }
}