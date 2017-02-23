namespace Construktion
{
    using System.Collections.Generic;
    using Blueprints;
    using Blueprints.Recursive;

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
            new ClassBlueprint(),
            new DefensiveBlueprint()
        };

        public static ConstruktionPipeline Pipeline => new DefaultConstruktionPipeline(Blueprints);
    }
}
