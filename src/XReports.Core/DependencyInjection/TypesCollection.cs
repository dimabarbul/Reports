using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XReports.DependencyInjection
{
    public class TypesCollection<TBaseType> : IEnumerable<Type>
    {
        private readonly List<Type> types = new List<Type>();
        private readonly List<Type> baseTypes = new List<Type>();
        private readonly List<(Assembly, Type)> assemblies = new List<(Assembly, Type)>();

        public IReadOnlyCollection<Type> Types => this.LoadTypes();

        public TypesCollection<TBaseType> Add(params Type[] types)
        {
            foreach (Type type in types)
            {
                this.ValidateType(type);
            }

            this.types.AddRange(types);

            return this;
        }

        public TypesCollection<TBaseType> AddFromAssembly(Assembly assembly)
        {
            return this.AddFromAssembly(assembly, typeof(TBaseType));
        }

        public TypesCollection<TBaseType> AddFromAssembly<TCustomBaseType>(Assembly assembly)
            where TCustomBaseType : TBaseType
        {
            return this.AddFromAssembly(assembly, typeof(TCustomBaseType));
        }

        public TypesCollection<TBaseType> AddFromAssembly(Assembly assembly, Type baseType)
        {
            this.ValidateBaseType(baseType);

            this.assemblies.Add((assembly, baseType));

            return this;
        }

        public TypesCollection<TBaseType> AddByBaseType<TCustomBaseType>()
        {
            return this.AddByBaseType(typeof(TCustomBaseType));
        }

        public TypesCollection<TBaseType> AddByBaseType(Type type)
        {
            this.ValidateBaseType(type);

            this.baseTypes.Add(type);

            return this;
        }

        private Type[] LoadTypes()
        {
            return this.types
                .Concat(this.baseTypes.SelectMany(this.GetImplementingTypes))
                .Concat(this.assemblies.SelectMany(this.GetTypesInAssembly))
                .Distinct()
                .ToArray();
        }

        private IEnumerable<Type> GetImplementingTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(this.IsTypeValid)
                .Where(interfaceType.IsAssignableFrom)
                .ToArray();
        }

        private IEnumerable<Type> GetTypesInAssembly((Assembly, Type) assemblyLoadInfo)
        {
            return assemblyLoadInfo.Item1.GetTypes()
                .Where(this.IsTypeValid)
                .Where(assemblyLoadInfo.Item2.IsAssignableFrom);
        }

        private bool IsTypeValid(Type type)
        {
            return type.IsClass && !type.IsAbstract && typeof(TBaseType).IsAssignableFrom(type);
        }

        private void ValidateType(Type type)
        {
            if (!this.IsTypeValid(type))
            {
                throw new ArgumentException(
                    $"Type {type} is invalid. It should be non-abstract class that implements {typeof(TBaseType)}.",
                    nameof(type));
            }
        }

        private void ValidateBaseType(Type baseType)
        {
            if (!this.IsBaseTypeValid(baseType))
            {
                throw new ArgumentException(
                    $"{baseType} should be assignable to {typeof(TBaseType)}",
                    nameof(baseType));
            }
        }

        private bool IsBaseTypeValid(Type baseType)
        {
            return typeof(TBaseType).IsAssignableFrom(baseType);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return this.Types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}