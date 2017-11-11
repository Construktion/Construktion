namespace Construktion.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints.Simple;

    public class DefaultConstruktionSettings : ConstruktionSettings
    {
        private IEnumerable<Blueprint> _defaultBlueprints;
        private readonly List<Blueprint> _customBlueprints;
        private readonly List<ExitBlueprint> _exitBlueprints;

        public IEnumerable<Blueprint> Blueprints => _customBlueprints.Concat(_defaultBlueprints);
        public IEnumerable<ExitBlueprint> ExitBlueprints => _exitBlueprints;

        public IDictionary<Type, Type> TypeMap { get; }
        public Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; private set; }
        public Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; private set; }

        private int? enumerableCount;
        public int EnumuerableCount { get; private set; }

        private int? recursionDepth;
        public int RecurssionDepth { get; private set; }

        private bool? throwOnRecursion;
        public bool ThrowOnRecurrsion { get; private set; }

        public DefaultConstruktionSettings()
        {
            _defaultBlueprints = new DefaultBlueprints();
            _customBlueprints = new List<Blueprint>();
            _exitBlueprints = new List<ExitBlueprint>();

            TypeMap = new Dictionary<Type, Type>();
            PropertyStrategy = Default.PropertyStrategy;
            CtorStrategy = Default.CtorStrategy;
            EnumuerableCount = Default.EnumerableCount;
            RecurssionDepth = Default.RecursionDepth;
            ThrowOnRecurrsion = Default.ThrowOnRecursion;
        }

        internal void Apply(ConstruktionRegistry registry)
        {
            _customBlueprints.AddRange(registry.Settings._customBlueprints);
            _exitBlueprints.AddRange(registry.Settings._exitBlueprints);

            foreach (var map in registry.Settings.TypeMap)
                TypeMap[map.Key] = map.Value;

            CtorStrategy = registry.Settings.CtorStrategy ?? CtorStrategy;
            PropertyStrategy = registry.Settings.PropertyStrategy ?? PropertyStrategy;
            EnumuerableCount = registry.Settings.enumerableCount ?? EnumuerableCount;
            RecurssionDepth = registry.Settings.recursionDepth ?? RecurssionDepth;
            ThrowOnRecurrsion = registry.Settings.throwOnRecursion ?? ThrowOnRecurrsion;

            _defaultBlueprints = new DefaultBlueprints(TypeMap);
        }

        internal void Apply(Blueprint blueprint) => _customBlueprints.Add(blueprint);

        internal void Apply(IEnumerable<Blueprint> blueprints) => _customBlueprints.AddRange(blueprints);

        internal void Apply(ExitBlueprint exitBlueprint) => _exitBlueprints.Add(exitBlueprint);

        internal void Apply(Type contract, Type implementation) => TypeMap[contract] = implementation;

        internal void Apply(Ctors strategy)
        {
            switch (strategy)
            {
                case Ctors.Greedy:
                    CtorStrategy = Extensions.GreedyCtor;
                    break;
                case Ctors.Modest:
                    CtorStrategy = Extensions.ModestCtor;
                    break;
            }
        }

        internal void Apply(PropertySetters strategy)
        {
            switch (strategy)
            {
                case PropertySetters.Public:
                    PropertyStrategy = Extensions.PropertiesWithPublicSetter;
                    break;
                case PropertySetters.Accessible:
                    PropertyStrategy = Extensions.PropertiesWithAccessibleSetter;
                    break;
            }
        }

        internal void UseInstance<T>(T instance) => _customBlueprints.Insert(0, new ScopedBlueprint(typeof(T), instance));

        internal void Inject(Type type, object value) => _customBlueprints.Insert(0, new ScopedBlueprint(type, value));

        internal void SetEnumerableCount(int count) => enumerableCount = count;
        internal void SetRecursionDepth(int depth) => recursionDepth = depth;
        internal void SetThrowOnRecursion(bool shouldThrow) => throwOnRecursion = shouldThrow;

        private static class Default
        {
            public static int EnumerableCount => 3;
            public static int RecursionDepth => 0;
            public static bool ThrowOnRecursion => false;
            public static Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy => Extensions.ModestCtor;

            public static Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy => Extensions
                .PropertiesWithPublicSetter;
        }
    }
}