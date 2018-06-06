using System.Reflection;
using BenchmarkDotNet.Running;

namespace Hyperion.Benchmarks
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).RunAll();
        }
    }
}