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
        private readonly List<Blueprint> _customBlueprints = new List<Blueprint>();
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        private readonly Func<List<ConstructorInfo>, ConstructorInfo> _defaultCtorStrategy = Extensions.GreedyCtor;
        private Func<List<ConstructorInfo>, ConstructorInfo> _customCtorStrategy;

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
            var defaultBlueprints = Default.Blueprints;
            defaultBlueprints.Add(new NonEmptyCtorBlueprint(_typeMap, _customCtorStrategy ?? _defaultCtorStrategy));
            defaultBlueprints.Add(new DefensiveBlueprint());

            var pipeline = new DefaultConstruktionPipeline(_customBlueprints.Concat(defaultBlueprints));

            var result = (T)pipeline.Construct(new ConstruktionContext(request));
          
            hardCodes?.Invoke(result);

            return result;
        }

        //just use a  configure method

        public Construktion AddRegistry(BlueprintRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            _customBlueprints.AddRange(registry.Blueprints);

            foreach (var map in registry.TypeMap)
                if (!_typeMap.ContainsKey(map.Key))
                    _typeMap[map.Key] = map.Value;

            _customCtorStrategy = _customCtorStrategy ?? registry.CtorStrategy;

            return this;
        }

        public Construktion AddBlueprint(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _customBlueprints.Add(blueprint);
            
            return this;
        }
    }
}