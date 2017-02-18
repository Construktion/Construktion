namespace Construktion.Tests
{
    public static class Pipeline
    {
        public static ConstruktionPipeline Default => new DefaultConstruktionPipeline(new Construktion().Blueprints);
    }
}
