using System;

namespace STDFLib2.Serialization
{
    public interface ISTDFSurrogateSelector
    {
        void ChainSelector(ISTDFSurrogateSelector selector);
        ISTDFSurrogateSelector GetNextSelector();
        ISTDFSerializationSurrogate GetSurrogate(Type type, out ISTDFSurrogateSelector selector);
    }
}
