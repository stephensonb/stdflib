using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace STDFLib2.Serialization
{
    public class STDFSurrogateSelector : ISTDFSurrogateSelector
    {
        protected ISTDFSurrogateSelector NextSelector { get; set; } = null;
        protected Dictionary<Type, ISTDFSerializationSurrogate> Surrogates = new Dictionary<Type, ISTDFSerializationSurrogate>();
        
        public void ChainSelector(ISTDFSurrogateSelector selector)
        {
            NextSelector = selector;
        }

        public ISTDFSurrogateSelector GetNextSelector()
        {
            return NextSelector;
        }

        public ISTDFSerializationSurrogate GetSurrogate(Type type, out ISTDFSurrogateSelector selector)
        {
            selector = null;

            if (Surrogates.ContainsKey(type))
            {
                selector = this;
                return Surrogates[type];
            }

            // If this selector cannot handle the type, try the next selector if defined, otherwise return null;

            return GetNextSelector()?.GetSurrogate(type, out selector);
        }

        public virtual void RemoveSurrogate(Type type)
        {
            if (Surrogates.ContainsKey(type))
            {
                Surrogates.Remove(type);
            }
        }
    }
}
