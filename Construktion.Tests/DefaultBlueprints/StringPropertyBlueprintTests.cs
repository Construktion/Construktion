namespace Construktion.Tests.DefaultBlueprints
{
    using Shouldly;
    using Xunit;

    public class StringPropertyBlueprintTests
    {
        [Fact]
        public void name_is_prefixed_on_string()
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