namespace Construktion.Tests
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class ClassBlueprintTests
    {
        [Fact]
        public void can_build_a_class()
        {
            var blueprint = new ClassBlueprint();

            var result = (Person)blueprint.Construct(new ConstruktionContext(typeof(Person)), Pipeline.Default);

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