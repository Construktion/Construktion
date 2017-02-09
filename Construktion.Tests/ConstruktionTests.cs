namespace Construktion.Tests
{
    using System;
    using System.Collections.Generic;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class ConstruktionTests
    {
        [Fact]
        public void Can_Inject_Single_Blueprint_To_Beginning_Of_blueprints()
        {
            var construktion = new Construktion(new TestBlueprint());

            construktion.Blueprints[0].ShouldBeOfType<TestBlueprint>();
        }

        [Fact]
        public void Can_Inject_Multiple_blueprints_To_Beginning_Of_blueprints()
        {
            var construktion = new Construktion(new List<Blueprint>{ new TestBlueprint(), new AnotherTestBlueprint()});

            construktion.Blueprints[0].ShouldBeOfType<TestBlueprint>();
            construktion.Blueprints[1].ShouldBeOfType<AnotherTestBlueprint>();
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
                var c = new Construktion(new List<Blueprint> { new TestBlueprint(), null });
            });
        }


        public class TestBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                throw new System.NotImplementedException();
            }

            public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new System.NotImplementedException();
            }
        }

        public class AnotherTestBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context)
            {
                throw new System.NotImplementedException();
            }

            public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
