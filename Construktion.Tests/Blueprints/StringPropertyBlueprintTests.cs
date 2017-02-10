namespace Construktion.Tests.Blueprints
{
    using Shouldly;
    using Xunit;

    public class StringPropertyBlueprintTests
    {
        [Fact]
        public void property_name_is_prefixed()
        {
            var construktion = new Construktion();

            var result = construktion.Build<Foo>();

            result.Name.Substring(0, 5).ShouldBe("Name-");
        }

        public class Foo
        {
            public string Name { get; set; }
        }
    }
}