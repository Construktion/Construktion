// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable ArrangeTypeMemberModifiers
namespace Construktion.Tests.Acceptance
{
    using System.Collections.Generic;
    using Shouldly;
    using Xunit;

    public class ComplexConstruktionTests
    {
        private readonly Construktion _construktion;

        public ComplexConstruktionTests()
        {
            _construktion = new Construktion();
        }

        [Fact]
        public void enums()
        {
            var result = _construktion.Construct<TestResult>();

            result.ShouldBeOneOf(TestResult.Pass, TestResult.Fail);
        }

        [Fact]
        public void should_build_classes()
        {
            var result = _construktion.Construct<Child>();

            result.Name.ShouldNotBeNullOrEmpty();
            result.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_build_nested_classes()
        {
            var result = _construktion.Construct<Parent>();

            result.Name.ShouldNotBeNullOrEmpty();
            result.Age.ShouldNotBe(default(int));
            result.Child.Name.ShouldNotBeNullOrEmpty(); 
            result.Child.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_prefix_properties_with_property_name()
        {
            var child = _construktion.Construct<Child>();

            child.Name.ShouldStartWith("Name-");
            child.Age.ShouldNotBe(0);
        }

        [Fact]
        public void should_hardcode_properties()
        {
            var result = _construktion.Construct<Parent>(x =>
            {
                x.Name = "Foo";
                x.Child.Name = "Lil Foo";
            });

            result.Name.ShouldBe("Foo");
            result.Age.ShouldNotBe(default(int));
            result.Child.Name.ShouldBe("Lil Foo");
            result.Child.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_build_private_properties()
        {
            var result = _construktion.Construct<Private>();

            result.PrivateName.ShouldNotBeNullOrEmpty();
            result.PrivateAge.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_not_set_properties_without_a_setter()
        {
            var parent = _construktion.Construct<Parent>();

            parent.NoSetter.ShouldBe(0);
        }

        [Fact]
        public void should_build_any_enumerable_type()
        {
            var result = _construktion.Construct<IReadOnlyCollection<Child>>();

            result.Count.ShouldBe(3);
            result.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Name));
            result.ShouldAllBe(x => x.Age != 0);
        }

        [Fact]
        public void should_build_arrays()
        {
            var result = _construktion.Construct<string[]>();

            result.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x));
        }

        [Fact]
        public void should_build_dictionaries()
        {
            var result = _construktion.Construct<Dictionary<string,int>>();

            result.ShouldNotBe(null);
            result.Count.ShouldBe(4);
            result.Keys.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x));
            result.Values.ShouldAllBe(x => x != 0);
        }

        class Private
        {
            public string PrivateName { get; private set; }
            public int PrivateAge { get; private set; }
        }

        class Child
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        class Parent
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Child Child { get; set; }
            public int NoSetter { get; }
        }

        enum TestResult
        {
            Pass,
            Fail
        }
    }
}
