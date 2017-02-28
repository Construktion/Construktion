namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints;
    using Blueprints.Recursive;

    public class Construktion
    {
        private Func<List<ConstructorInfo>, ConstructorInfo> _ctorStrategy;
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();
        private readonly List<Blueprint> _customBlueprints = new List<Blueprint>();

        public T Construct<T>()
        {
            return DoConstruct<T>(typeof(T), null);
        }

        public T Construct<T>(Action<T> hardCodes)
        {
            return DoConstruct(typeof(T), hardCodes);
        }

        public object Construct(Type request)
        {
            return DoConstruct<object>(request, null);
        }

        private T DoConstruct<T>(Type request, Action<T> hardCodes)
        {
            var blueprints = PrepareBlueprints();

            var pipeline = new DefaultConstruktionPipeline(blueprints);

            var result = (T)pipeline.Construct(new ConstruktionContext(request));
          
            hardCodes?.Invoke(result);

            return result;
        }

        public Construktion AddRegistry(BlueprintRegistry registry)
        {
            addRegistry(registry);
            return this;
        }

        public Construktion AddRegistry(Action<BlueprintRegistry> registry)
        {
            var _registry = new BlueprintRegistry();

            registry(_registry);

            addRegistry(_registry);
            return this;
        }

        public Construktion AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _customBlueprints.Add(blueprint);
            return this;
        }

        private void addRegistry(BlueprintRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            _ctorStrategy = _ctorStrategy ?? registry.CtorStrategy;

            foreach (var map in registry.TypeMap)
            {
                if (!_typeMap.ContainsKey(map.Key))
                    _typeMap[map.Key] = map.Value;
            }

            _customBlueprints.AddRange(registry.Blueprints);
        }

        private IEnumerable<Blueprint> PrepareBlueprints()
        {
            var defaults = Default.Blueprints;

            var idx = defaults.FindIndex(x => x.GetType() == typeof(NonEmptyCtorBlueprint));

            _ctorStrategy = _ctorStrategy ?? Extensions.GreedyCtor;

            defaults[idx] = new NonEmptyCtorBlueprint(_typeMap, _ctorStrategy);

            return _customBlueprints.Concat(defaults);
        }
    }
}