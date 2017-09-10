namespace Construktion.Tests.Recursive
{
    using System.Collections.Generic;
    using Blueprints.Recursive;
    using Shouldly;
    using Xunit;

    public class DictionaryBlueprintTests
    {
        [Fact]
        public void should_have_4_dictionary_items_when_not_an_enum()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary = (Dictionary<int, string>) blueprint.Construct(new ConstruktionContext(typeof(Dictionary<int, string>)), new DefaultConstruktionPipeline());

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(4);
        }

        [Fact]
        public void when_dictionary_is_an_enum_should_cover_all_enum_options()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary = (Dictionary<Genre, string>)blueprint.Construct(new ConstruktionContext(typeof(Dictionary<Genre, string>)), new DefaultConstruktionPipeline());

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