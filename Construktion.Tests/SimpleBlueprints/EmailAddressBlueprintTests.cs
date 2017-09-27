using Construktion.Blueprints.Simple;
using Shouldly;
using Xunit;

namespace Construktion.Tests.SimpleBlueprints
{
    public class EmailAddressBlueprintTests
    {
        [Fact]
        public void should_match_email_properties()
        {
            var blueprint = new EmailAddressBlueprint();
            var email = typeof(Foo).GetProperty("Email");
            var emailAddress = typeof(Foo).GetProperty("EmailAddress");

            blueprint.Matches(new ConstruktionContext(email)).ShouldBe(true);
            blueprint.Matches(new ConstruktionContext(emailAddress)).ShouldBe(true);
        }

        [Fact]
        public void should_construct()
        {
            var result = (string)new EmailAddressBlueprint().Construct(new ConstruktionContext(typeof(string)), new DefaultConstruktionPipeline());

            result.ShouldContain("@");
            result.ShouldContain(".com");
        }

        public class Foo
        {
            public string Email { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}