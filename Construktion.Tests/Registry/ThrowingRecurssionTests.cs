namespace Construktion.Tests.Registry
{
    using System;
    using Shouldly;

    public class ThrowingRecursionTests
    {
        private readonly ConstruktionRegistry registry;

        public ThrowingRecursionTests()
        {
            registry = new ConstruktionRegistry();
        }

        public void should_throw_when_opted_in()
        {
            registry.ThrowOnRecursion(true);
            var construktion = new Construktion().Apply(registry);

            Exception<Exception>.ShouldBeThrownBy(() => construktion.Construct<Parent>());
        }

        public void should_still_throw()
        {
            registry.ThrowOnRecursion(true);
            var newRegistry = new ConstruktionRegistry();
            var construktion = new Construktion().Apply(registry).Apply(newRegistry);

            Exception<Exception>.ShouldBeThrownBy(() => construktion.Construct<Parent>());
        }

        public void should_not_throw_when_opted_out()
        {
            registry.ThrowOnRecursion(true);
            var newRegistry = new ConstruktionRegistry().ThrowOnRecursion(false);
            var construktion = new Construktion().Apply(registry).Apply(newRegistry);

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