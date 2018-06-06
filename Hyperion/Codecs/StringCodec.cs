using System;
using System.Text;
using Hyperion.Abstractions;
using Hyperion.Sessions;

namespace Hyperion.Codecs
{
    public struct StringCodec : ICodec<string>
    {
        public ushort Identifier => TypeIdentifiers.STRING_ID;
        
        public void Write<TWriter, TSession>(TWriter output, in string value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            if (session.Remember(value, out var referenceId))
            {
                // if value was used previously in this session, its ID will be reused
                output.WriteInt32(referenceId);
            }
            else
            {
                output.WriteInt32(referenceId);
                ReadOnlySpan<byte> span = value.AsReadOnlySpan().AsBytes();
                output.WriteInt32(span.Length);
                output.WriteBytes(span);
            }
        }

        public void Read<TReader, TSession>(TReader input, out string value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            var referenceId = input.ReadInt32();
            if (!session.TryRecall(referenceId, out value))
            {
                var length = input.ReadInt32();
                using (var memory = session.Borrow(length))
                {
                    input.ReadBytes(memory.Span);
                    value = new string(memory.Span.NonPortableCast<byte, char>());
                }
                
                if (referenceId > 0)
                {
                    session.Store(referenceId, value);
                } 
            }
        }
    }
}