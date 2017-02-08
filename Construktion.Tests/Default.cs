namespace Construktion.Tests
{
    public static class Default
    {
        public static ConstruktionPipeline Pipeline => new DefaultConstruktionPipeline(new Construktion().Builders);
    }
}
