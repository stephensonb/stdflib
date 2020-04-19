using System;
using System.Collections.Generic;

namespace STDFLib2
{
    public class SurrogateSelector
    {
        private Dictionary<Type, ISurrogate> _surrogates = new Dictionary<Type, ISurrogate>();

        public ISurrogate GetSurrogate(Type type)
        {
            if(_surrogates.TryGetValue(type, out ISurrogate value))
            {
                return value;
            }

            return null;
        }

        public void AddSurrogate(Type type, ISurrogate surrogate)
        {
            if (!_surrogates.ContainsKey(type))
            {
                _surrogates.Add(type, surrogate);
            }
        }
    }
}
