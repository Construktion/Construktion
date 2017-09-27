using Construktion.Blueprints.Recursive;
using Shouldly;
using Xunit;

namespace Construktion.Tests.RecursiveBlueprints
{
    public class ArrayBlueprintTests
    {
        [Fact]
        public void should_match_arrays()
        {
            var blueprint = new ArrayBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(int[])));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_build_arrays()
        {
            var blueprint = new ArrayBlueprint();

            var ints = (int[])blueprint.Construct(new ConstruktionContext(typeof(int[])), new DefaultConstruktionPipeline());
            
            ints.ShouldNotBe(null);
            ints.ShouldAllBe(x => x != 0);
        }
    }
}