namespace Construktion.Tests.Bugs
{
    using System;
    using Shouldly;

    public class Bug_52_abstract_blueprints_can_match_types_other_than_t
    {
        public void abstract_blueprints_should_only_match_t()
        {
            var construktion = new Construktion();
            construktion.Apply(new FooBlueprint());

            ShouldlyExtensions.ShouldNotThrow<InvalidCastException>(() => construktion.Construct<string>());
        }

        public class FooBlueprint : AbstractBlueprint<Foo>
        {
            public override bool Matches(ConstruktionContext context) => true;

            public override Foo Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                return new Foo();
            }
        }

        public class Foo { }
    }
}