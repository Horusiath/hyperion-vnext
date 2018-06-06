using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using Hyperion.Abstractions;

namespace Hyperion
{
    public struct StreamWriter<TStream> : IWriter where TStream : Stream
    {
        private TStream stream;

        public StreamWriter(TStream stream)
        {
            if (stream == null) throw new ArgumentNullException();
            if (!stream.CanWrite) throw new ArgumentException("Hyperion.Abstractions.StreamWriter requires writeable stream");
            
            this.stream = stream;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt8(sbyte value)
        {
            stream.WriteByte((byte)value);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteInt16(short value)
        {
            Span<byte> span = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            WriteBytes(span);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteInt32(int value)
        {
            Span<byte> span = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            WriteBytes(span);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteInt64(long value)
        {
            Span<byte> span = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            WriteBytes(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUInt8(byte value)
        {
            stream.WriteByte(value);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteUInt16(ushort value)
        {
            Span<byte> span = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            WriteBytes(span);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUInt32(uint value)
        {
            Span<byte> span = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            WriteBytes(span);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteUInt64(ulong value)
        {
            Span<byte> span = stackalloc byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            WriteBytes(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteSingle(float value)
        {
            Span<byte> span = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(span, *(int*)&value);
            WriteBytes(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteDouble(double value)
        {
            Span<byte> span = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(span, *(long*)&value);
            WriteBytes(span);
        }

        public void WriteDecimal(decimal value)
        {
            Span<int> span = decimal.GetBits(value);
            WriteBytes(span.NonPortableCast<int, byte>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBytes(ReadOnlySpan<byte> value)
        {
            stream.Write(value);
        }
    }
}