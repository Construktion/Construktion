namespace Construktion.Tests
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Shouldly;
    using Xunit;

    public class ConstruktionTests
    {
        [Fact]
        public void Can_Inject_Single_Builder_To_Beginning_Of_Builders()
        {
            var construktion = new Construktion(new TestBuilder());

            construktion.Builders[0].ShouldBeOfType<TestBuilder>();
        }

        [Fact]
        public void Can_Inject_Multiple_Builders_To_Beginning_Of_Builders()
        {
            var construktion = new Construktion(new List<Builder>{ new TestBuilder(), new AnotherTestBuilder()});

            construktion.Builders[0].ShouldBeOfType<TestBuilder>();
            construktion.Builders[1].ShouldBeOfType<AnotherTestBuilder>();
        }

        [Fact]
        public void Throws_If_Builder_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var c = new Construktion((Builder)null);
            });
        }

        [Fact]
        public void Throws_If_Any_Are_Null()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var c = new Construktion(new List<Builder> { new TestBuilder(), null });
            });
        }


        public class TestBuilder : Builder
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

        public class AnotherTestBuilder : Builder
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
