using System;
using System.Runtime.CompilerServices;
using Hyperion.Abstractions;
using Hyperion.Sessions;

namespace Hyperion.Codecs
{
    public struct BoolCodec : ICodec<bool>
    {
        private const byte TRUE = 1;
        private const byte FALSE = 0;

        public ushort Identifier => TypeIdentifiers.BOOL_ID;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in bool value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => 
            output.WriteUInt8(value ? TRUE : FALSE);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out bool value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadUInt8() != FALSE;
    }
    
    public struct CharCodec : ICodec<char>
    {
        public ushort Identifier => TypeIdentifiers.CHAR_ID;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in char value, TSession session)
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt16((short)value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out char value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = (char)input.ReadInt16();
    }
    
    public struct SByteCodec : ICodec<sbyte>
    {
        public ushort Identifier => TypeIdentifiers.SBYTE_ID;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in sbyte value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt8(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out sbyte value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadInt8();
    }
    
    public struct ByteCodec : ICodec<byte>
    {
        public ushort Identifier => TypeIdentifiers.BYTE_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in byte value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteUInt8(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out byte value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadUInt8();
    }
    
    public struct ShortCodec : ICodec<short>
    {
        public ushort Identifier => TypeIdentifiers.INT16_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in short value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt16(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out short value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadInt16();
    }
    
    public struct UShortCodec : ICodec<ushort>
    {
        public ushort Identifier => TypeIdentifiers.UINT16_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in ushort value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteUInt16(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out ushort value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadUInt16();
    }
    
    public struct IntCodec : ICodec<int>
    {
        public ushort Identifier => TypeIdentifiers.INT32_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in int value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt32(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out int value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadInt32();
    }
    
    public struct UIntCodec : ICodec<uint>
    {
        public ushort Identifier => TypeIdentifiers.UINT32_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in uint value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteUInt32(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out uint value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadUInt32();
    }

    public struct LongCodec : ICodec<long>
    {
        public ushort Identifier => TypeIdentifiers.INT64_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in long value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt64(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out long value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadInt64();
    }
    
    public struct ULongCodec : ICodec<ulong>
    {
        public ushort Identifier => TypeIdentifiers.UINT64_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in ulong value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteUInt64(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out ulong value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadUInt64();
    }
    
    public struct FloatCodec : ICodec<float>
    {
        public ushort Identifier => TypeIdentifiers.SINGLE_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in float value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteSingle(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out float value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadSingle();
    }
    
    public struct DoubleCodec : ICodec<double>
    {
        public ushort Identifier => TypeIdentifiers.DOUBLE_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in double value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteDouble(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out double value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = input.ReadDouble();
    }
    
    public struct DecimalCodec : ICodec<decimal>
    {
        public ushort Identifier => TypeIdentifiers.DECIMAL_ID;

        public void Write<TWriter, TSession>(TWriter output, in decimal value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            var bytes = decimal.GetBits(value);
            output.WriteInt32(bytes[0]);
            output.WriteInt32(bytes[1]);
            output.WriteInt32(bytes[2]);
            output.WriteInt32(bytes[3]);
        }

        public void Read<TReader, TSession>(TReader input, out decimal value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            var parts = new[]
            {
                input.ReadInt32(),
                input.ReadInt32(),
                input.ReadInt32(),
                input.ReadInt32()
            };
            var sign = (parts[3] & 0x80000000) != 0;

            var scale = (byte) ((parts[3] >> 16) & 0x7F);
            value = new decimal(parts[0], parts[1], parts[2], sign, scale);
        }
    }
    
    public struct TimeSpanCodec : ICodec<TimeSpan>
    {
        public ushort Identifier => TypeIdentifiers.TIMESPAN_ID;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TWriter, TSession>(TWriter output, in TimeSpan value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession => output.WriteInt64(value.Ticks);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<TReader, TSession>(TReader input, out TimeSpan value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession => value = new TimeSpan(input.ReadInt64());
    }
    
    public struct DateTimeCodec : ICodec<DateTime>
    {
        public ushort Identifier => TypeIdentifiers.DATETIME_ID;

        public void Write<TWriter, TSession>(TWriter output, in DateTime value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            output.WriteInt64(value.Ticks);
            output.WriteUInt8((byte)value.Kind);
        }

        public void Read<TReader, TSession>(TReader input, out DateTime value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            value = new DateTime(input.ReadInt64(), (DateTimeKind)input.ReadUInt8());
        }
    }
    
    public struct DateTimeOffsetCodec : ICodec<DateTimeOffset>
    {
        public ushort Identifier => TypeIdentifiers.DATETIME_OFFSET_ID;

        public void Write<TWriter, TSession>(TWriter output, in DateTimeOffset value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            output.WriteInt64(value.Date.Ticks);
            output.WriteInt64(value.Offset.Ticks);
        }

        public void Read<TReader, TSession>(TReader input, out DateTimeOffset value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            var date = new DateTime(input.ReadInt64());
            var offset = new TimeSpan(input.ReadInt64());
            value = new DateTimeOffset(date, offset);
        }
    }
    
    public struct GuidCodec : ICodec<Guid>
    {
        public ushort Identifier => TypeIdentifiers.GUID_ID;

        public void Write<TWriter, TSession>(TWriter output, in Guid value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            Span<byte> span = stackalloc byte[16];
            value.TryWriteBytes(span);
            output.WriteBytes(span);
        }

        public void Read<TReader, TSession>(TReader input, out Guid value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            Span<byte> span = stackalloc byte[16];
            input.ReadBytes(span);
            value = new Guid(span);
        }
    }
    
    public struct BinaryCodec : ICodec<byte[]>
    {
        public ushort Identifier => TypeIdentifiers.BYTE_ARRAY_ID;

        public void Write<TWriter, TSession>(TWriter output, in byte[] value, TSession session) 
            where TWriter : struct, IWriter 
            where TSession : ISerializerSession
        {
            output.WriteInt32(value.Length);
            output.WriteBytes(value);
        }

        public void Read<TReader, TSession>(TReader input, out byte[] value, TSession session) 
            where TReader : struct, IReader 
            where TSession : IDeserializerSession
        {
            var length = input.ReadInt32();
            var bytes = new byte[length];
            input.ReadBytes(bytes);
            value = bytes;
        }
    }
}