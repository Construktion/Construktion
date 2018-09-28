namespace Construktion.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints;
    using Blueprints.Simple;

    public class DefaultConstruktionSettings : ConstruktionSettings
    {
        private IEnumerable<Blueprint> defaultBlueprints;
        private readonly List<Blueprint> _customBlueprints;
        private readonly List<ExitBlueprint> _exitBlueprints;

        private Func<IEnumerable<ConstructorInfo>, ConstructorInfo> ctorStrategy;
        private Func<Type, IEnumerable<PropertyInfo>> propertyStrategy;

        public IEnumerable<Blueprint> Blueprints => _customBlueprints.Concat(defaultBlueprints);
        public IEnumerable<ExitBlueprint> ExitBlueprints => _exitBlueprints;

        public IDictionary<Type, Type> TypeMap { get; }

        public ConstructorInfo CtorStrategy(IEnumerable<ConstructorInfo> constructors)
        {
            var ctors = constructors.ToList();

            return ctorStrategy != null
                ? ctorStrategy(ctors)
                : Default.CtorStrategy(ctors);
        }

        public IEnumerable<PropertyInfo> PropertyStrategy(Type request)
        {
            return propertyStrategy != null
                ? propertyStrategy(request)
                : Default.PropertyStrategy(request);
        }

        private int? enumerableCount;
        public int EnumerableCount { get; private set; }

        private int? recursionDepth;
        public int RecursionDepth { get; private set; }

        private bool? throwOnRecursion;
        public bool ThrowOnRecursion { get; private set; }

        private int? maxDepth;
        public int? MaxDepth { get; private set; }

        public DefaultConstruktionSettings()
        {
            defaultBlueprints = new DefaultBlueprints();
            _customBlueprints = new List<Blueprint>();
            _exitBlueprints = new List<ExitBlueprint>();

            TypeMap = new Dictionary<Type, Type>();
            EnumerableCount = Default.EnumerableCount;
            RecursionDepth = Default.RecursionDepth;
            ThrowOnRecursion = Default.ThrowOnRecursion;
        }

        public void Apply(DefaultConstruktionSettings settings)
        {
            _customBlueprints.AddRange(settings._customBlueprints);
            _exitBlueprints.AddRange(settings._exitBlueprints);

            foreach (var map in settings.TypeMap)
                TypeMap[map.Key] = map.Value;

            ctorStrategy = settings.ctorStrategy ?? ctorStrategy;
            propertyStrategy = settings.propertyStrategy ?? propertyStrategy;
            EnumerableCount = settings.enumerableCount ?? EnumerableCount;
            RecursionDepth = settings.recursionDepth ?? RecursionDepth;
            ThrowOnRecursion = settings.throwOnRecursion ?? ThrowOnRecursion;
            MaxDepth = settings.maxDepth ?? MaxDepth;

            defaultBlueprints = new DefaultBlueprints(TypeMap);
        }

        public void Apply(Blueprint blueprint) => _customBlueprints.Add(blueprint);

        public void Apply(IEnumerable<Blueprint> blueprints) => _customBlueprints.AddRange(blueprints);

        public void Apply(ExitBlueprint exitBlueprint) => _exitBlueprints.Add(exitBlueprint);

        public void Apply(Type contract, Type implementation) => TypeMap[contract] = implementation;

        public void UseInstance<T>(T instance) => _customBlueprints.Insert(0, new SingletonBlueprint(typeof(T), instance));

        public void Inject(Type type, object value) => _customBlueprints.Insert(0, new SingletonBlueprint(type, value));

        public void SetCtorStrategy(Ctors strategy)
        {
            switch (strategy)
            {
                case Ctors.Modest:
                    ctorStrategy = Extensions.ModestCtor;
                    break;
                case Ctors.Greedy:
                    ctorStrategy = Extensions.GreedyCtor;
                    break;
            }
        }

        public void SetPropertyStrategy(PropertySetters strategy)
        {
            switch (strategy)
            {
                case PropertySetters.Public:
                    propertyStrategy = Extensions.PropertiesWithPublicSetter;
                    break;
                case PropertySetters.Accessible:
                    propertyStrategy = Extensions.PropertiesWithAccessibleSetter;
                    break;
            }
        }

        public void SetEnumerableCount(int count) => enumerableCount = count;
        public void SetRecursionDepth(int depth) => recursionDepth = depth;
        public void SetThrowOnRecursion(bool shouldThrow) => throwOnRecursion = shouldThrow;
        public void SetMaxDepth(int depth) => maxDepth = depth;

        private static class Default
        {
            public static int EnumerableCount => 3;
            public static int RecursionDepth => 0;
            public static bool ThrowOnRecursion => false;
            public static Func<IEnumerable<ConstructorInfo>, ConstructorInfo> CtorStrategy => Extensions.ModestCtor;
            public static Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy => Extensions.PropertiesWithPublicSetter;
        }
    }
}