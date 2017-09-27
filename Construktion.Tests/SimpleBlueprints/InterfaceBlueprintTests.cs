using System;
using System.Collections.Generic;
using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
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
