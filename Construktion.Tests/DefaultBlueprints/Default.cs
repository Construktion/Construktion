namespace Construktion.Tests.DefaultBlueprints
{
    public static class Default
    {
        public static ConstruktionPipeline Pipeline => new DefaultConstruktionPipeline(new Construktion().Blueprints);
    }
}
