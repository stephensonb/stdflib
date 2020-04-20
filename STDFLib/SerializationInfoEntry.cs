using System;

namespace STDFLib
{
    /// <summary>
    /// Holds serialization information for a field/property of an object.
    /// </summary>
    public class SerializationInfoEntry
    {
        /// <summary>
        /// Index within the SerializationInfo list of SerializationInfoEntries.
        /// </summary>
        public int Index;
        /// <summary>
        /// Name of the field/property in the source object Type that this entry represents.
        /// </summary>
        public string Name;
        /// <summary>
        /// The data Type of the Value in this SerializationInfoEntry.
        /// </summary>
        public Type Type;
        /// <summary>
        /// Indicates if this field/property was decorated with the STDFOptional attribute in the source object Type.
        /// </summary>
        public bool IsOptional = false;
        /// <summary>
        /// Indicates if the Value of this SerializationInfoEntry has been set via the SetValue method.
        /// </summary>
        public bool IsSet = false;
        /// <summary>
        /// Indicates if the current value is equivalent to the MissingValue property.
        /// </summary>
        public bool IsMissingValue = true;
        /// <summary>
        /// Current value of this SerializationInfoEntry.
        /// </summary>
        public object Value;
        /// <summary>
        /// Original value of this SerializationInfoEntry (set the first time SetValue is called).
        /// </summary>
        public object OriginalValue;
        /// <summary>
        /// The value that represents that current Value of this SerializationInfoEntry is considered "missing".
        /// </summary>
        public object MissingValue;
        /// <summary>
        /// The index to the SerializationInfoEntry that can provide the number of items to serialize/deserialize for this SerializationInfoEntry.
        /// Use for array type fields where another field provides the item count.
        /// </summary>
        public int? ItemCountIndex;

        public override string ToString()
        {
            return string.Format("{0,3} {1,10} IsOptional: {2,4}  IsSet: {3,4}  Value: {4}",
                                 Index, Name, IsOptional, IsSet, Value);
        }
    }
}
