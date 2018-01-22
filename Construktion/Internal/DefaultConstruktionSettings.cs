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
        private IEnumerable<Blueprint> _defaultBlueprints;
        private readonly List<Blueprint> _customBlueprints;
        private readonly List<ExitBlueprint> _exitBlueprints;

        public IEnumerable<Blueprint> Blueprints => _customBlueprints.Concat(_defaultBlueprints);
        public IEnumerable<ExitBlueprint> ExitBlueprints => _exitBlueprints;

        public IDictionary<Type, Type> TypeMap { get; }

	    private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
		public Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; private set; }

	    private Func<Type, IEnumerable<PropertyInfo>> _propertyStrategy;
		public Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; private set; }

        private int? _enumerableCount;
        public int EnumuerableCount { get; private set; }

        private int? _recursionDepth;
        public int RecurssionDepth { get; private set; }

        private bool? _throwOnRecursion;
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

        public void Apply(DefaultConstruktionSettings settings)
        {
            _customBlueprints.AddRange(settings._customBlueprints);
            _exitBlueprints.AddRange(settings._exitBlueprints);

            foreach (var map in settings.TypeMap)
                TypeMap[map.Key] = map.Value;

            CtorStrategy = settings._ctorStrategy ?? CtorStrategy;
            PropertyStrategy = settings._propertyStrategy ?? PropertyStrategy;
            EnumuerableCount = settings._enumerableCount ?? EnumuerableCount;
            RecurssionDepth = settings._recursionDepth ?? RecurssionDepth;
            ThrowOnRecurrsion = settings._throwOnRecursion ?? ThrowOnRecurrsion;

            _defaultBlueprints = new DefaultBlueprints(TypeMap);
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
				    _ctorStrategy = Extensions.ModestCtor;
				    break;
			    case Ctors.Greedy:
				    _ctorStrategy = Extensions.GreedyCtor;
				    break;
			}
	    }

        public void SetPropertyStrategy(PropertySetters strategy)
	    {
		    switch (strategy)
		    {
			    case PropertySetters.Public:
				    _propertyStrategy = Extensions.PropertiesWithPublicSetter;
				    break;
			    case PropertySetters.Accessible:
				    _propertyStrategy = Extensions.PropertiesWithAccessibleSetter;
				    break;
		    }
	    }

        public void SetEnumerableCount(int count) => _enumerableCount = count;
        public void SetRecursionDepth(int depth) => _recursionDepth = depth;
        public void SetThrowOnRecursion(bool shouldThrow) => _throwOnRecursion = shouldThrow;

        private static class Default
        {
            public static int EnumerableCount => 3;
            public static int RecursionDepth => 0;
            public static bool ThrowOnRecursion => false;
            public static Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy => Extensions.ModestCtor;
            public static Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy => Extensions.PropertiesWithPublicSetter;
        }
    }
}