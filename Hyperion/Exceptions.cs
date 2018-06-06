using System;
using System.Runtime.Serialization;

namespace Hyperion
{
    public abstract class HyperionException : Exception
    {
        protected HyperionException()
        {
        }

        protected HyperionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected HyperionException(string message) : base(message)
        {
        }

        protected HyperionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Thrown when type identifier for a given type is not unique in scope of a given serializer.
    /// </summary>
    public sealed class TypeIdentifierNotUniqueException : HyperionException
    {

    }
}