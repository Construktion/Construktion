namespace Construktion.Blueprints
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Recursive;
	using Simple;

	public class DefaultBlueprints : IEnumerable<Blueprint>
    {
        private readonly IEnumerable<Blueprint> _defaultBlueprints;

        public DefaultBlueprints() : this(new Dictionary<Type, Type>()) { }

        public DefaultBlueprints(IDictionary<Type, Type> typeMap)
        {
            _defaultBlueprints = new List<Blueprint>
            {
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
                new ComplexClassBlueprint(),
                new InterfaceBlueprint(typeMap),
                //always last
                new DefensiveBlueprint()
            };
        }

        public IEnumerator<Blueprint> GetEnumerator() => _defaultBlueprints.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}