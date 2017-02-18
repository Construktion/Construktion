// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable ArrangeTypeMemberModifiers
namespace Construktion.Tests.Acceptance
{
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
        public void should_build_nested_classes_too()
        {
            var result = _construktion.Construct<Parent>();

            result.Name.ShouldNotBeNullOrEmpty();
            result.Age.ShouldNotBe(default(int));
            result.Child.Name.ShouldNotBeNullOrEmpty(); 
            result.Child.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_hardcode_properties()
        {
            var result = _construktion.Construct<Parent>(x =>
            {
                x.Name = "Joe";
                x.Child.Name = "Lil Joe";
            });

            result.Name.ShouldBe("Joe");
            result.Age.ShouldNotBe(default(int));
            result.Child.Name.ShouldBe("Lil Joe");
            result.Child.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void should_build_private_properties()
        {
            var result = _construktion.Construct<Private>();

            result.PrivateName.ShouldNotBeNullOrEmpty();
            result.PrivateAge.ShouldNotBe(default(int));
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
        }

        enum TestResult
        {
            Pass,
            Fail
        }
    }
}
