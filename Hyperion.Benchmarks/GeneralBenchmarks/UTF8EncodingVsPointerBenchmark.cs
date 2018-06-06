using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Hyperion.Benchmarks.GeneralBenchmarks
{
    public class UTF8EncodingVsPointerBenchmark
    {
        public string TestString = new string('h', 100);
        
        [Benchmark]
        public void ReadWriteToBytesUtf8()
        {
            var bytes = Encoding.UTF8.GetBytes(TestString);
            var str = Encoding.UTF8.GetString(bytes);
        }
        
        [Benchmark]
        public unsafe void ReadWriteToBytesPointers()
        {
            fixed (char* ptr = TestString)
            {
                var bytePtr = (byte*) ptr;

                var str = new string((char*) bytePtr);
            }
        }

        [Benchmark]
        public void Utf8ToStream()
        {
            using (var stream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(TestString);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
            }
        }

        [Benchmark]
        public unsafe void PointerToStream()
        {
            fixed (char* ptr = TestString)
            {
                var count = Encoding.UTF8.GetByteCount(TestString);
                var bytePtr = (byte*)ptr;

                using (var unmanaged = new UnmanagedMemoryStream(bytePtr, count))
                using (var stream = new MemoryStream())
                {
                    unmanaged.CopyTo(stream);
                }
            }
        }
    }
}