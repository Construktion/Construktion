namespace Construktion.Tests.Recursive
{
    using System.Collections.Generic;
    using Blueprints.Recursive;
    using Shouldly;
    using Xunit;

    public enum genres
    {
        Fantasy,
        Sci_FI,
        Biography,
        Fiction,
        Cooking
    }

    public enum small_genres
    {
        Fiction,
        Cooking
    }

    public class DictionaryBlueprintTests
    {
        [Fact]
        public void should_build_dictionary()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary = (Dictionary<int, string>) blueprint.Construct(new ConstruktionContext(typeof(Dictionary<int, string>)), Default.Pipeline);

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(4);
        }
        
        [Fact]
        public void should_build_enum_key_dictionary()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary = (Dictionary<genres, string>)blueprint.Construct(new ConstruktionContext(typeof(Dictionary<genres, string>)), Default.Pipeline);

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(4);
        }

        [Fact]
        public void should_build_small_enum_key_dictionary()
        {
            var blueprint = new DictionaryBlueprint();

            var dictionary = (Dictionary<small_genres, string>)blueprint.Construct(new ConstruktionContext(typeof(Dictionary<small_genres, string>)), Default.Pipeline);

            dictionary.ShouldNotBe(null);
            dictionary.Count.ShouldBe(2);
        }


    }
}