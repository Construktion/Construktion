namespace Construktion.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Blueprints;
    using Blueprints.Recursive;
    using Blueprints.Simple;

    internal class DefaultBlueprints : IEnumerable<Blueprint>
    {
        private readonly IEnumerable<Blueprint> _defaultBlueprints;

        public DefaultBlueprints() : this(new Dictionary<Type, Type>()) { }

        public DefaultBlueprints(IDictionary<Type, Type> typeMap)
        {
            _defaultBlueprints = new List<Blueprint>
            {
                new StringPropertyBlueprint(),
                new StringBlueprint(),
                new NumericBlueprint(),
                new CharBlueprint(),
                new GuidBlueprint(),
                new BoolBlueprint(),
                new TimespanBlueprint(),
                new DictionaryBlueprint(),
                new EnumerableBlueprint(),
                new ArrayBlueprint(),
                new EnumBlueprint(),
                new DateTimeBlueprint(),
                new NullableTypeBlueprint(),
                new EmptyCtorBlueprint(),
                new NonEmptyCtorBlueprint(),
                new InterfaceBlueprint(typeMap),
                new DefensiveBlueprint()
            };
        }

        public IEnumerator<Blueprint> GetEnumerator()
        {
            return _defaultBlueprints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}