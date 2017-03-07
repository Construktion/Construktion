namespace Construktion.Tests.Simple
{
    using System.Reflection;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class OmitPropertyBlueprintTests
    {
        [Fact]
        public void should_match_defined_convention()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.EndsWith("Id"), typeof(int));

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.FooId))));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_return_default_int()
        {
            var blueprint = new OmitPropertyBlueprint(x => x.EndsWith("Id"), typeof(int));

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty("FooId")), Default.Pipeline);

            result.ShouldBe(0);
        }

        [Fact]
        public void should_be_case_sensitive()
        {
            var registry = new BlueprintRegistry();
            registry.OmitIds();
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.Fooid.ShouldNotBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            var registry = new BlueprintRegistry();
            registry.OmitProperties(x => x.EndsWith("_Id"), typeof(string));
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();
            
            foo.String_Id.ShouldBe(null);
        }

        public class Foo
        {
            public int FooId { get; set; }
            public int Fooid { get; set; }
            public int Foo_id { get; set; }
            public string String_Id { get; set; }
        }
    }
}