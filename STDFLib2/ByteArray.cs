using System;

namespace STDFLib2
{
    public class ByteArray
    {
        private byte[] _value { get; set; }

        private ByteArray(byte[] value)
        {
            _value = value;
        }

        public ByteArray(int count)
        {
            _value = new byte[count];
        }

        public static implicit operator byte[](ByteArray value) 
        {
            return value != null ? value._value : null;
        }

        public static implicit operator ByteArray(byte[] value)
        {
            return value != null ? new ByteArray(value) : null;
        }

        public int ByteCount { get => _value?.Length ?? 0; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is ByteArray array2)
            {
                if (ByteCount != array2.ByteCount)
                {
                    return false;
                }

                for (int i = 0; i < ByteCount; i++)
                {
                    if (_value[i] != array2._value[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public byte this[int index]
        {

            get 
            {
                if (index < 0 || index > _value.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                return _value[index];
            }

            set 
            {
                if (index < 0 || index > _value.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                _value[index] = value;
            }
        }
    }
}
