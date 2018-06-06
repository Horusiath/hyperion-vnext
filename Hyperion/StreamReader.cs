using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using Hyperion.Abstractions;

namespace Hyperion
{
    /// <summary>
    /// An instance of binary stream reader, which uses fast approach to serialization:
    /// a data is stored as is in the most naive manner without applying possible
    /// compaction steps.
    /// </summary>
    /// <typeparam name="TStream"></typeparam>
    public struct StreamReader<TStream> : IReader where TStream : Stream
    {
        public const int StackallocSpanThreshold = 60;

        private TStream stream;

        public StreamReader(TStream stream)
        {
            if (stream == null) throw new ArgumentNullException();
            if (!stream.CanRead) throw new ArgumentException("Hyperion.Abstractions.StreamReader requires readable stream.");

            this.stream = stream;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadInt8()
        {
            return (sbyte)stream.ReadByte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16()
        {
            Span<byte> span = stackalloc byte[sizeof(short)];
            ReadBytes(span);
            return BinaryPrimitives.ReadInt16BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            Span<byte> span = stackalloc byte[sizeof(int)];
            ReadBytes(span);
            return BinaryPrimitives.ReadInt32BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64()
        {
            Span<byte> span = stackalloc byte[sizeof(long)];
            ReadBytes(span);
            return BinaryPrimitives.ReadInt64BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadUInt8()
        {
            return (byte)stream.ReadByte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16()
        {
            Span<byte> span = stackalloc byte[sizeof(ushort)];
            ReadBytes(span);
            return BinaryPrimitives.ReadUInt16BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            Span<byte> span = stackalloc byte[sizeof(uint)];
            ReadBytes(span);
            return BinaryPrimitives.ReadUInt32BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64()
        {
            Span<byte> span = stackalloc byte[sizeof(ulong)];
            ReadBytes(span);
            return BinaryPrimitives.ReadUInt64BigEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe float ReadSingle()
        {
            Span<byte> span = stackalloc byte[sizeof(int)];
            ReadBytes(span);
            var i = BinaryPrimitives.ReadInt32BigEndian(span);
            return BitConverter.Int32BitsToSingle(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble()
        {
            Span<byte> span = stackalloc byte[sizeof(long)];
            ReadBytes(span);
            var i = BinaryPrimitives.ReadInt64BigEndian(span);
            return BitConverter.Int64BitsToDouble(i);
        }

        public decimal ReadDecimal()
        {
            Span<int> parts = stackalloc int[4];
            ReadBytes(parts.AsBytes());
            
            var sign = (parts[3] & 0x80000000) != 0;

            var scale = (byte)((parts[3] >> 16) & 0x7F);
            var newValue = new decimal(parts[0], parts[1], parts[2], sign, scale);
            return newValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(Span<byte> span)
        {
            var readBytes = stream.Read(span);
            //TODO: check if readBytes is equal to span length and try to repeat read
        }
    }
}