using System;

namespace Hyperion.Abstractions
{
    /// <summary>
    /// Schema definition describes details about the particular type to be serialized.
    /// </summary>
    public sealed class SchemaDefinition
    {
        public static SchemaDefinition FromType<T>() => FromType(typeof(T));

        public static SchemaDefinition FromType(Type type)
        {
            throw new NotImplementedException();
        }

        public ushort TypeIdentifier { get; }

        public FieldDefinition[] Fields { get; }
    }

    public sealed class FieldDefinition
    {

    }
}