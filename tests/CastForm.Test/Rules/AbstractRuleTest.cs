﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using AutoFixture;

namespace CastForm.Test.Rules
{
    public abstract class AbstractRuleTest
    {
        protected Fixture Fixture { get; }

        public AbstractRuleTest()
        {
            Fixture = new Fixture();
        }


        static AbstractRuleTest()
        {
            var assemblyName = new AssemblyName("RuleTesAssembly");
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        }
        
        private static AssemblyBuilder AssemblyBuilder { get; }

        private ModuleBuilder _module;
        protected ModuleBuilder Module
        {
            get
            {
                if (_module == null)
                {
                    var typeName = this.GetType().Name;

                    _module = AssemblyBuilder.DefineDynamicModule($"{typeName}Module");
                }
                return _module;
            }
        }

        private TypeBuilder _typeBuilder;
        private MethodBuilder _methodBuilder;
        protected AbstractRuleTest DefineClassAndMethod(Type source, Type destiny)
        {
            var typeName = $"{source.Name}To{destiny.Name}Mapper";

            var mapper = typeof(IMap<,>).MakeGenericType(source, destiny);

            var interfaces = new[] { mapper, typeof(IMap) };

            const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            _typeBuilder = Module.DefineType(typeName, typeAttributes, null, interfaces);
            _methodBuilder = _typeBuilder.DefineMethod(nameof(IMapper.Map),
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                destiny, new[] { source });
            return this;
        }
        
    }
}
