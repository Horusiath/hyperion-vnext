using System;
using Hyperion.Sessions;

namespace Hyperion.Abstractions
{
    public interface ICodec
    {
        Type TargetType { get; }
        ushort Identifier { get; }
    }

    /// <summary>
    /// Codecs are responsible for serializing/deserializing objects of specific type.
    /// </summary>
    public interface ICodec<T> : ICodec
    {
        void Write<TWriter, TSession>(TWriter output, in T value, TSession session) 
            where TWriter : struct, IWriter
            where TSession : ISerializerSession;
        
        void Read<TReader, TSession>(TReader input, out T value, TSession session)
            where TReader : struct, IReader
            where TSession : IDeserializerSession;
    }
}