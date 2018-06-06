using System;
using System.IO;
using Hyperion.Abstractions;
using Hyperion.Codecs;
using Hyperion.Sessions;

namespace Hyperion
{
    public sealed class Serializer
    {
        public void Serialize<T, TWriter>(T value, TWriter writer)
            where TWriter : struct, IWriter
        {
            Serialize(value, writer, new StatelessSession());
        }
        
        public void Serialize<T, TWriter, TSession>(T value, TWriter writer, TSession session)
            where TWriter : struct, IWriter
            where TSession : ISerializerSession
        {
            throw new NotImplementedException();   
        }

        public T Deserialize<T, TReader>(TReader reader)
            where TReader : struct, IReader
        {
            return Deserialize<T, TReader, StatelessSession>(reader, new StatelessSession());
        }

        public T Deserialize<T, TReader, TSession>(TReader reader, TSession session)
            where TReader : struct, IReader
            where TSession : IDeserializerSession
        {
            throw new NotImplementedException();
        }
    }
}