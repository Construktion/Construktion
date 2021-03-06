﻿namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Internal;
    using Shouldly;

    public class OmitPropertyBlueprintTests
    {
        public void should_match_defined_convention()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.Name.EndsWith("Id"), typeof(int));

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.FooId))));

            matches.ShouldBe(true);
        }

        public void should_return_default_int()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.Name.EndsWith("Id"), typeof(int));

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty("FooId")),
                new DefaultConstruktionPipeline());

            result.ShouldBe(0);
        }

        public class Foo
        {
            public int FooId { get; set; }
        }
    }
}