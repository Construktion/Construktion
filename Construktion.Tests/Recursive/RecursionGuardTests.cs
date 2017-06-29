namespace Construktion.Tests.Recursive
{
    using System;
    using System.Collections.Generic;
    using Shouldly;
    using Xunit;
    using System.Linq;

    public class RecursionGuardTests
    {
        [Fact]
        public void should_ignore_on_recursion()
        {
            var parent = new Construktion().Construct<Parent>();

            parent.Name.ShouldNotBeNullOrWhiteSpace();

            parent.Child.Name.ShouldNotBeNullOrWhiteSpace();
            parent.Child.RecursiveParent.ShouldBe(null);
        }

        [Fact]
        public void should_ignore_recursive_property_in_the_graph()
        {
            var sut = new Construktion().Construct<DeepRecursiveParent>();

            sut.Name.ShouldNotBeNullOrWhiteSpace();

            sut.Parent.ShouldNotBe(null);
            sut.Parent.Name.ShouldNotBeNullOrWhiteSpace();

            sut.Parent.Child.ShouldNotBe(null);
            sut.Parent.Child.Name.ShouldNotBeNullOrWhiteSpace();

            sut.Parent.Child.RecursiveParent.ShouldBe(null);
        }

        [Fact]
        public void should_handle_an_enunmerable_with_recursive_property_correctly()
        {
            var sut = new Construktion().Construct<DeepRecursiveParents>();

            sut.Name.ShouldNotBeNullOrWhiteSpace();

            sut.Parents.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Name));
            sut.Parents.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Child.Name));
            sut.Parents.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Child.Name));
            sut.Parents.ShouldAllBe(x => x.Child != null);

            sut.Parents.Select(x => x.Child).ShouldAllBe(x => x.RecursiveParent == null);
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

        public class DeepRecursiveParent
        {
            public string Name { get; set; }
            public Parent Parent { get; set; }
        }

        public class DeepRecursiveParents
        {
            public string Name { get; set; }
            public IEnumerable<Parent> Parents { get; set; }
        }            
    }
}