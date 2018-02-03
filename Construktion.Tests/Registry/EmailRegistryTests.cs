namespace Construktion.Tests.Registry
{
    using Shouldly;

    public class EmailRegistryTests
    {
        [Fact]
        public void should_construct_email()
        {
            var construktion = new Construktion().With(x => x.AddEmailBlueprint());

            var result = construktion.Construct<Foo>();

            result.Email.ShouldContain("@");
            result.Custom.ShouldNotContain("@");
        }

        [Fact]
        public void should_allow_custom_convention()
        {
            var construktion = new Construktion().With(x => x.AddEmailBlueprint(p => p.Name.Equals("Custom")));

            var result = construktion.Construct<Foo>();

            result.Email.ShouldNotContain("@");
            result.Custom.ShouldContain("@");
        }

        public class Foo
        {
            public string Email { get; set; }
            public string Custom { get; set; }
        }
    }
}