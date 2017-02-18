namespace Construktion.Tests.Blueprints
{
    public static class Pipeline
    {
        public static ConstruktionPipeline Default => new DefaultConstruktionPipeline(new Construktion().Blueprints);
    }
}
