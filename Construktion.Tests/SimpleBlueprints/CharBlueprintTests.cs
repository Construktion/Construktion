namespace Construktion.Tests.SimpleBlueprints
{
    using Blueprints.Simple;
    using Internal;
    using Shouldly;

    public class CharBlueprintTests
    {
        public void should_not_be_null()
        {
            var blueprint = new CharBlueprint();

            var result = (char)blueprint.Construct(new ConstruktionContext(typeof(char)),
                new DefaultConstruktionPipeline());

            result.ShouldNotBeNull();
        }
    }
}