using System.Collections.Generic;
using Hyperion.Abstractions;
using Hyperion.Sessions;

namespace Hyperion.Codecs
{
    public struct ArrayCodec : ICodec
    {
        public ushort Identifier { get; }

        public void Write<TWriter, TSession>(TWriter output, object value, TSession session) where TWriter : struct, IWriter where TSession : ISerializerSession
        {
            throw new System.NotImplementedException();
        }

        public object Read<TReader, TSession>(TReader input, TSession session) where TReader : struct, IReader where TSession : IDeserializerSession
        {
            throw new System.NotImplementedException();
        }
    }
}