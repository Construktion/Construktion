namespace Construktion.Tests.RecursiveBlueprints
{
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints.Recursive;
    using Internal;
    using Shouldly;

    public class NullableTypeBlueprintTests
    {
        [Fact]
        public void should_match_nullable_type()
        {
            var blueprint = new NullableTypeBlueprint();
            var context = new ConstruktionContext(typeof(Foo).GetProperty("NullableAge"));

            var matches = blueprint.Matches(context);

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_not_match_non_nullable_type()
        {
            var blueprint = new NullableTypeBlueprint();
            var context = new ConstruktionContext(typeof(Foo).GetProperty("NonNullableAge"));

            var matches = blueprint.Matches(context);

            matches.ShouldBe(false);
        }

        [Fact]
        public void nullable_type_should_be_null_sometimes()
        {
            var blueprint = new NullableTypeBlueprint();
            var context = new ConstruktionContext(typeof(Foo).GetProperty("NullableAge"));

            var values = new List<int?>();

            for (var i = 0; i <= 30; i++)
            {
                var value = blueprint.Construct(context, new DefaultConstruktionPipeline());
                values.Add((int?)value);
            }

            values.Any(x => x == null).ShouldBe(true);
            values.Any(x => x != null).ShouldBe(true);
        }

        [Fact]
        public void should_not_match_reference_types()
        {
            var blueprint = new NullableTypeBlueprint();
            var context = new ConstruktionContext(typeof(Foo));

            var matches = blueprint.Matches(context);

            matches.ShouldBe(false);
        }

        public class Foo
        {
            public int NonNullableAge { get; set; }
            public int? NullableAge { get; set; }
        }
    }
}