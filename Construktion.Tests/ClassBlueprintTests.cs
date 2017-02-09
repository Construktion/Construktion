namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class ClassBlueprintTests
    {
        [Fact]
        public void Can_Build_A_Class()
        {
            var blueprint = new ClassBlueprint();

            var result = (Person)blueprint.Build(new ConstruktionContext(typeof(Person)), Default.Pipeline);

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