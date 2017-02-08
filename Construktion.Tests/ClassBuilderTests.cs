namespace Construktion.Tests
{
    using Builders;
    using Shouldly;
    using Xunit;

    public class ClassBuilderTests
    {
        [Fact]
        public void Can_Build_A_Class()
        {
            var builder = new ClassBuilder();

            var result = (Person)builder.Build(new RequestContext(typeof(Person)), Default.Pipeline);

            result.ShouldNotBeNull();
            result.Name.ShouldNotBeNull();
            result.Age.ShouldNotBeNull();
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}