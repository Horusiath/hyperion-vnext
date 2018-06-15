using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Hyperion.Abstractions;
using Hyperion.Codecs;
using Hyperion.Sessions;

namespace Hyperion.Compilation
{
    public static class CodecCompiler
    {
        private static readonly AssemblyBuilder AssemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Hyperion.DynamicCodecs"), AssemblyBuilderAccess.Run);

        private static readonly Type typeOfType = typeof(Type);
        private static readonly Type typeOfReader = typeof(IReader);
        private static readonly Type typeOfWriter = typeof(IWriter);
        private static readonly Type typeOfValueType = typeof(ValueType);
        private static readonly Type typeOfSerializerSession = typeof(ISerializerSession);
        private static readonly Type typeOfDeserializerSession = typeof(IDeserializerSession);

        public static ICodec CreateCodec(SchemaDef def)
        {
            var type = def.Type;
            var moduleBuilder = AssemblyBuilder.DefineDynamicModule(type.Module + "Dynamic");

            var builder = moduleBuilder.DefineType(type.Name + "$RuntimeGeneratedCodec", TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            builder.SetParent(typeOfValueType);
            builder.AddInterfaceImplementation(typeof(ICodec<>).MakeGenericType(type));

            CreateTargetType(builder, def);
            CreateIdentifier(builder, def);
            CreateReader(builder, def);
            CreateWriter(builder, def);

            var tCodec = builder.CreateType();
            return (ICodec)Activator.CreateInstance(tCodec);
        }

        private static void CreateIdentifier(TypeBuilder builder, SchemaDef def)
        {
            var pIdentifier = builder.DefineProperty(nameof(ICodec.Identifier), PropertyAttributes.None, typeof(ushort), Array.Empty<Type>());
            pIdentifier.SetConstant(def.Options.TypeId);
        }

        private static void CreateTargetType(TypeBuilder builder, SchemaDef def)
        {
            var pTargetType = builder.DefineProperty(nameof(ICodec.TargetType), PropertyAttributes.None, typeOfType, Array.Empty<Type>());

            var targetTypeGetter = builder.DefineMethod("get_TargetType", MethodAttributes.Private | MethodAttributes.Final | MethodAttributes.SpecialName, CallingConventions.HasThis, typeOfType, Array.Empty<Type>());
            var il = targetTypeGetter.GetILGenerator();

            il.Emit(OpCodes.Ldtoken, def.Type);
            il.Emit(OpCodes.Ret);

            pTargetType.SetGetMethod(targetTypeGetter);
        }

        private static void CreateReader(TypeBuilder builder, SchemaDef def)
        {
            var mRead = builder.DefineMethod("Read",
                MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual);

            // define method-specific generic parameters and their constraints
            var generics = mRead.DefineGenericParameters("TReader", "TSession");
            var tReader = generics[0];
            var tSession = generics[1];
            tReader.SetInterfaceConstraints(typeOfReader);
            tReader.SetBaseTypeConstraint(typeOfValueType);
            tSession.SetInterfaceConstraints(typeOfDeserializerSession);

            // define method parameters and their types
            mRead.SetParameters(tReader, def.Type.MakeByRefType(), tSession);

            // setup method body
            var il = mRead.GetILGenerator();
            CreateReaderBody(def, il);
        }

        private static void CreateReaderBody(SchemaDef def, ILGenerator il)
        {
            il.Emit(OpCodes.Initobj, def.Type);

            il.Emit(OpCodes.Ret);
        }

        private static void CreateWriter(TypeBuilder builder, SchemaDef def)
        {
            var mRead = builder.DefineMethod("Write",
                MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual);

            // define method-specific generic parameters and their constraints
            var generics = mRead.DefineGenericParameters("TWriter", "TSession");
            var tReader = generics[0];
            var tSession = generics[1];
            tReader.SetInterfaceConstraints(typeOfWriter);
            tReader.SetBaseTypeConstraint(typeOfValueType);
            tSession.SetInterfaceConstraints(typeOfSerializerSession);

            // define method parameters and their types
            mRead.SetParameters(tReader, def.Type.MakeByRefType(), tSession);

            // setup method body
            var il = mRead.GetILGenerator();
            CreateWriterBody(def, il);
        }

        private static void CreateWriterBody(SchemaDef def, ILGenerator il)
        {
            il.Emit(OpCodes.Ret);
        }
    }
}