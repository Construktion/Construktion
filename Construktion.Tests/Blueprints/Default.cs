namespace Construktion.Tests.Blueprints
{
    public static class Default
    {
        public static ConstruktionPipeline Pipeline => new DefaultConstruktionPipeline(new Construktion().Blueprints);
    }
}
