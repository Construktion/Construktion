namespace Construktion.Tests.Simple
{
    using Blueprints.Simple;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class InterfaceBlueprintTests
    {        
        [Fact]
        public void should_match_registered_interfaces()
        {
            var typeMap = new Dictionary<Type, Type>
            {
              { typeof(IFoo), typeof(Foo)}
            };
            var blueprint = new InterfaceBlueprint(typeMap);

            var matchesRegistered = blueprint.Matches(new ConstruktionContext(typeof(IFoo)));
            var matchesUnRegistered = blueprint.Matches(new ConstruktionContext(typeof(IBar)));

            matchesRegistered.ShouldBe(true);
            matchesUnRegistered.ShouldBe(false);
        }

        [Fact]
        public void should_construct()
        {
            var typeMap = new Dictionary<Type, Type>
            {
              { typeof(IFoo), typeof(Foo)}
            };
            var blueprint = new InterfaceBlueprint(typeMap);
            var pipeline = new DefaultConstruktionPipeline(Default.Blueprints.Replace(typeof(InterfaceBlueprint), blueprint));

            var result = blueprint.Construct(new ConstruktionContext(typeof(IFoo)), pipeline);

            result
                .ShouldBeOfType<Foo>()
                .Age.ShouldNotBe(0);
        }

        public interface IFoo
        {

        }

        public class Foo : IFoo
        {
            public int Age { get; set; }
        }

        public interface IBar
        {

        }
    }
}
