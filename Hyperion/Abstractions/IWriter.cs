using System;

namespace Hyperion.Abstractions
{
    public interface IWriter
    {
        void WriteInt8(sbyte value);
        void WriteInt16(short value);
        void WriteInt32(int value);
        void WriteInt64(long value);
        
        void WriteUInt8(byte value);
        void WriteUInt16(ushort value);
        void WriteUInt32(uint value);
        void WriteUInt64(ulong value);

        void WriteSingle(float value);
        void WriteDouble(double value);
        void WriteDecimal(decimal value);

        void WriteBytes(ReadOnlySpan<byte> value);
    }
}