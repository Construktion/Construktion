namespace Construktion.Tests.Registry
{
    using System;
    using Shouldly;

    public class ThrowingRecurssionTests
    {
        private readonly ConstruktionRegistry registry;

        public ThrowingRecurssionTests()
        {
            registry = new ConstruktionRegistry();
        }

        public void should_throw_when_opted_in()
        {
            registry.ThrowOnRecurssion(true);
            var construktion = new Construktion().With(registry);

            Exception<Exception>.ShouldBeThrownBy(() => construktion.Construct<Parent>());
        }

        public void should_still_throw()
        {
            registry.ThrowOnRecurssion(true);
            var newRegistry = new ConstruktionRegistry();
            var construktion = new Construktion().With(registry).With(newRegistry);

            Exception<Exception>.ShouldBeThrownBy(() => construktion.Construct<Parent>());
        }

        public void should_not_throw_when_opted_out()
        {
            registry.ThrowOnRecurssion(true);
            var newRegistry = new ConstruktionRegistry().ThrowOnRecurssion(false);
            var construktion = new Construktion().With(registry).With(newRegistry);

            Should.NotThrow(() => construktion.Construct<Parent>());
        }

        public class Parent
        {
            public string Name { get; set; }
            public Child Child { get; set; }
        }

        public class Child
        {
            public string Name { get; set; }
            public Parent RecursiveParent { get; set; }
        }
    }
}