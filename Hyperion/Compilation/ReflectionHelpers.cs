using System;
using System.Reflection;

namespace Hyperion.Compilation
{
    public static class ReflectionHelpers
    {
        public static Assembly[] LoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}