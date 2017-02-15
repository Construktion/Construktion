namespace Construktion.Tests
{
    using System;
    using System.Collections.Generic;
    using global::Construktion.Blueprints;
    using Shouldly;
    using Xunit;

    public class ConstruktionTests
    {
        [Fact]
        public void custom_blueprints_should_be_listed_first()
        {
            var construktion = new Construktion(new FooBlueprint());

            construktion.Blueprints[0].ShouldBeOfType<FooBlueprint>();
        }

        [Fact]
        public void Can_Inject_Multiple_blueprints_To_Beginning_Of_blueprints()
        {
            var construktion = new Construktion(new List<Blueprint>{ new FooBlueprint(), new BarBlueprint()});

            construktion.Blueprints[0].ShouldBeOfType<FooBlueprint>();
            construktion.Blueprints[1].ShouldBeOfType<BarBlueprint>();
        }

        [Fact]
        public void Throws_If_Blueprint_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var c = new Construktion((Blueprint)null);
            });
        }

        [Fact]
        public void Throws_If_Any_Are_Null()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var c = new Construktion(new List<Blueprint> { new FooBlueprint(), null });
            });
        }


        public class FooBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                throw new NotImplementedException();
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }

        public class BarBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                throw new NotImplementedException();
            }

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new NotImplementedException();
            }
        }
    }
}
