using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Manages serialization surrogate objects to assist in the custom serialization and deserialization of objects.
    /// </summary>
    public class SurrogateSelector
    {
        private readonly Dictionary<Type, ISurrogate> _surrogates = new Dictionary<Type, ISurrogate>();

        /// <summary>
        /// Returns a surrogate that can support the serialization and deserialization of an object of type Type.
        /// </summary>
        /// <param name="type">Type of object to find a surrogate for.</param>
        /// <returns></returns>
        public ISurrogate GetSurrogate(Type type)
        {
            if (_surrogates.TryGetValue(type, out ISurrogate value))
            {
                return value;
            }

            return null;
        }
        /// <summary>
        /// Adds a surrogate object that can support objects of type Type.
        /// </summary>
        /// <param name="type">Type of object this surrogate can support.</param>
        /// <param name="surrogate">The serialization surrogate object.</param>
        public void AddSurrogate(Type type, ISurrogate surrogate)
        {
            if (!_surrogates.ContainsKey(type))
            {
                _surrogates.Add(type, surrogate);
            }
        }
    }
}
