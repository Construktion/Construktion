namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class ClassBlueprintTests
    {
        [Fact]
        public void should_build_a_class_with_its_properties()
        {
            var blueprint = new ClassBlueprint();

            var result = (Person)blueprint.Construct(new ConstruktionContext(typeof(Person)), Default.Pipeline);

            result.ShouldNotBeNull();
            result.Name.ShouldNotBeNullOrEmpty();
            result.Age.ShouldNotBeNull();
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}