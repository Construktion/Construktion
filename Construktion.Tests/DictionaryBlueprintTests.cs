namespace Construktion.Tests
{
    using System.Collections.Generic;
    using Blueprints;
    using Shouldly;
    using Xunit;

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
    }
}