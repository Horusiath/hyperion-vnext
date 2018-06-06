using System;

namespace Hyperion.Abstractions
{
    public interface IReader
    {
        sbyte ReadInt8();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        
        byte ReadUInt8();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();

        float ReadSingle();
        double ReadDouble();
        decimal ReadDecimal();
        
        void ReadBytes(Span<byte> span);
    }
}