namespace Construktion.Tests.RecursiveBlueprints
{
    using System.Collections.Generic;
    using Blueprints.Recursive;
    using Internal;
    using Shouldly;

    public class DictionaryBlueprintTests
    {
        public void should_respect_default_count()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary =
                (Dictionary<int, string>)blueprint.Construct(new ConstruktionContext(typeof(Dictionary<int, string>)),
                    new DefaultConstruktionPipeline());

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(3);
        }

        public void should_disregard_preset_and_cover_enum()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary =
                (Dictionary<Genre, string>)blueprint.Construct(
                    new ConstruktionContext(typeof(Dictionary<Genre, string>)), new DefaultConstruktionPipeline());

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(5);
        }
    }

    public enum Genre
    {
        Fantasy,
        SciFi,
        Biography,
        Fiction,
        Cooking
    }
}