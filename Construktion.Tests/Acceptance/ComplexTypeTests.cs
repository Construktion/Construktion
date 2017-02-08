// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable ArrangeTypeMemberModifiers
namespace Construktion.Tests.Acceptance
{
    using Shouldly;
    using Xunit;

    public class ComplexTypeTests
    {
        private readonly Construktion _construktion;

        public ComplexTypeTests()
        {
            _construktion = new Construktion();
        }

        [Fact]
        public void Enum()
        {
            var result = _construktion.Build<TestResult>();

            result.ShouldBeOneOf(TestResult.Pass, TestResult.Fail);
        }

        [Fact]
        public void Can_Build_Classes()
        {
            var result = _construktion.Build<Child>();

            result.Name.Substring(0, 7).ShouldBe("String-");
            result.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void Can_Build_Nested_Classes()
        {
            var result = _construktion.Build<Parent>();

            result.Name.Substring(0,7).ShouldBe("String-");
            result.Age.ShouldNotBe(default(int));
            result.Child.Name.Substring(0, 7).ShouldBe("String-");
            result.Child.Age.ShouldNotBe(default(int));
        }

        [Fact]
        public void Can_Explicitly_Set_Properties()
        {
            var result = _construktion.Build<Parent>(x =>
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
        public void Can_Build_Private_Properties()
        {
            var result = _construktion.Build<Private>();

            result.PrivateName.Substring(0,7).ShouldBe("String-");
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
