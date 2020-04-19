using System;

namespace STDFLib2
{ 
    public class SerializationInfoEntry
    {
        public int Index;
        public string Name;
        public Type Type;
        public bool IsOptional = false;
        public bool IsSet = false;
        public bool IsMissingValue=true;
        public object Value;
        public object OriginalValue;
        public object MissingValue;
        public int? ItemCountIndex;
        public long? ParentId;

        public override string ToString()
        {
            return string.Format("{0,3} {1,10} IsOptional: {2,4}  IsSet: {3,4}  Value: {4}",
                                 Index, Name, IsOptional, IsSet, Value);
        }
    }
}
