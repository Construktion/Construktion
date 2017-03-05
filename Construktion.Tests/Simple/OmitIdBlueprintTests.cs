namespace Construktion.Tests.Simple
{
    using System.Reflection;
    using Blueprints.Simple;
    using Shouldly;
    using Xunit;

    public class OmitIdBlueprintTests
    {
        [Fact]
        public void should_match_property_ending_in_Id()
        {
            var blueprint = new OmitIdBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(Foo).GetProperty(nameof(Foo.FooId))));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_return_default_int()
        {
            var blueprint = new OmitIdBlueprint();

            var result = blueprint.Construct(new ConstruktionContext(typeof(Foo).GetProperty("FooId")), Default.Pipeline);

            result.ShouldBe(0);
        }

        [Fact]
        public void should_omit_ids()
        {
            var registry = new BlueprintRegistry();
            registry.OmitIds();
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            var registry = new BlueprintRegistry();
            registry.OmitIds(x => x.EndsWith("_id"));
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();

            foo.Foo_id.ShouldBe(0);
        }

        [Fact]
        public void should_be_able_to_override_type_and_convention()
        {
            var registry = new BlueprintRegistry();
            registry.OmitIds(x => x.EndsWith("String_Id"), typeof(string));
            var construktion = new Construktion().AddRegistry(registry);

            var foo = construktion.Construct<Foo>();
            
            foo.String_Id.ShouldBe(null);
        }

        public class Foo
        {
            public int FooId { get; set; }
            public int Foo_id { get; set; }
            public string String_Id { get; set; }
        }
    }

    public class Re : BlueprintRegistry
    {
        public Re()
        {
            OmitIds();
        }
    }
}