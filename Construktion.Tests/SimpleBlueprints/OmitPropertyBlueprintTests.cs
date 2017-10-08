﻿using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class OmitPropertyBlueprintTests
    {
        [Fact]
        public void should_match_defined_convention()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.Name.EndsWith("Id"), typeof(int));

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.FooId))));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_return_default_int()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.Name.EndsWith("Id"), typeof(int));

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty("FooId")), new DefaultConstruktionPipeline());

            result.ShouldBe(0);
        }        

        public class Foo
        {
            public int FooId { get; set; }            
        }
    }
}