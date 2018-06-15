using Hyperion.Abstractions;
using Hyperion.Compilation;
using Xunit;

namespace Hyperion.Tests.Codecs
{
    public class CodecCompilerSpecs
    {
        [Schema(100)]
        class Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        
        public CodecCompilerSpecs()
        {
        }

        [Fact]
        public void CodecCompiler_should_create_codec_struct_for_simple_class()
        {
            var schema = SchemaDef.OfType(typeof(Point));
            var codec = CodecCompiler.CreateCodec(schema);
        }
    }
}