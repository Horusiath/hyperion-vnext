using System;
using System.Text;
using Hyperion.Abstractions;
using Hyperion.Sessions;

namespace Hyperion.Codecs
{
    public struct StringCodec : ICodec<string>
    {
        public Type TargetType => typeof(string);
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
            else if (ReferenceEquals(value, null))
            {
                output.WriteInt32(Constants.NULL_REFERENCE_ID);
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
            if (referenceId == Constants.NULL_REFERENCE_ID)
            {
                value = null;
            }
            else if (!session.TryRecall(referenceId, out value))
            {
                var length = input.ReadInt32();
                var memory = session.Borrow(length);
                try
                {
                    var span = new Span<byte>(memory, 0, length);
                    input.ReadBytes(span);
                    value = new string(span.NonPortableCast<byte, char>());

                }
                finally
                {
                    session.Return(memory);
                }
                
                
                if (referenceId > 0)
                {
                    session.Store(referenceId, value);
                } 
            }
        }
    }
}