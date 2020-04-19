using System;
using System.IO;

namespace STDFLib2.Serialization
{
    public abstract class STDFBinaryFormatter : ISTDFBinaryFormatter, IDisposable
    {
        public ISTDFSurrogateSelector SurrogateSelector { get; set; } 

        public STDFBinaryFormatter()
        { 
        }

        public abstract object Deserialize(Stream stream);

        public abstract void Serialize(Stream stream, object obj);

        #region IDisposable Support
        protected bool disposedValue = false; // To detect redundant calls

        protected abstract void Dispose(bool disposing);

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
