namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
 
    public class Construktion
    {
        private readonly List<Blueprint> _blueprints = new List<Blueprint>
        {
            new StringPropertyBlueprint(),
            new StringBlueprint(),
            new NumericBlueprint(),
            new CharBlueprint(),
            new GuidBlueprint(),
            new BoolBlueprint(),
            new EnumBlueprint(),
            new ClassBlueprint()
        };
        public IReadOnlyList<Blueprint> Blueprints => _blueprints;

        public Construktion()
        {
        }

        public Construktion(Blueprint additionalBlueprint) : this (Enumerable.Repeat(additionalBlueprint, 1))
        {
        }

        public Construktion(IEnumerable<Blueprint> blueprints)
        {
            var customBlueprints = blueprints?.ToList();

            if (customBlueprints == null)
                throw new ArgumentNullException(nameof(blueprints));

            if (customBlueprints.Any(x => x == null))
                throw new ArgumentNullException(nameof(blueprints), "There are blueprints in the collection that are null");

            _blueprints.InsertRange(0, customBlueprints);
        }

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
            var pipeline = new DefaultConstruktionPipeline(_blueprints);

            var result = (T)pipeline.Construct(new ConstruktionContext(request));

            hardCodes?.Invoke(result);

            return result;
        }
    }
}
