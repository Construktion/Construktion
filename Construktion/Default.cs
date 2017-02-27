namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
    using Blueprints.Recursive;
    using Blueprints.Simple;

    public static class Default
    {
        public static List<Blueprint> Blueprints => new List<Blueprint>
        {
            new StringPropertyBlueprint(),
            new StringBlueprint(),
            new NumericBlueprint(),
            new CharBlueprint(),
            new GuidBlueprint(),
            new BoolBlueprint(),
            new DictionaryBlueprint(),
            new EnumerableBlueprint(),
            new ArrayBlueprint(),
            new EnumBlueprint(),
            new NullableTypeBlueprint(),
            new EmptyCtorBlueprint(),
            //added at runtime
            //new NonEmptyCtorBlueprint(new Dictionary<Type, Type>()),
            //new DefensiveBlueprint()
        };

        public static ConstruktionPipeline Pipeline
            =>
                new DefaultConstruktionPipeline(
                    Blueprints.Concat(new List<Blueprint>
                    {
                        new NonEmptyCtorBlueprint(new Dictionary<Type, Type>()),
                        new DefensiveBlueprint()
                    }));
    }
}
