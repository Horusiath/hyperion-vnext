using System;
using System.Collections.Generic;
using Hyperion.Abstractions;
using Hyperion.Sessions;

namespace Hyperion.Codecs
{
    public struct ArrayCodec<T> : ICodec<T>
    {
        public Type TargetType { get; }
        public ushort Identifier { get; }

        public void Write<TWriter, TSession>(TWriter output, in T value, TSession session) where TWriter : struct, IWriter where TSession : ISerializerSession
        {
            throw new NotImplementedException();
        }

        public void Read<TReader, TSession>(TReader input, out T value, TSession session) where TReader : struct, IReader where TSession : IDeserializerSession
        {
            throw new NotImplementedException();
        }
    }
}