namespace Construktion.Tests.Registry
{
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class InjectTests
    {
        [Fact]
        public void should_injected_instance()
        {
            var injected = new Inject();
            var construktion = new Construktion().Inject(injected);

            var injectedHolder = construktion.Construct<InjectHolder>();

            injectedHolder.Injected
                .GetHashCode()
                .ShouldBe(injected.GetHashCode());
        }

        [Fact]
        public void should_always_use_the_same_instance()
        {
            var injected = new Inject();
            var construktion = new Construktion().Inject(injected);

            var injectedHolder = construktion.Construct<InjectHolder>();
            var injectedHolder2 = construktion.Construct<InjectHolder>();
         
            injectedHolder.Injected
                        .GetHashCode()
                        .ShouldBe(injected.GetHashCode());

            injectedHolder2.Injected
                         .GetHashCode()
                         .ShouldBe(injected.GetHashCode());
        }

        [Fact]
        public void should_injected_from_within_pipeline()
        {
            var construktion = new Construktion().With(new InjectBlueprint());

            var injected = construktion.Construct<Inject>();
            var injectedHolder = construktion.Construct<InjectHolder>();

            injectedHolder.Injected
                        .GetHashCode()
                        .ShouldBe(injected.GetHashCode());
        }

        [Fact]
        public void should_always_use_the_same_instance_when_injecteded_from_pipeline()
        {
            var construktion = new Construktion().With(new InjectBlueprint());

            var injected = construktion.Construct<Inject>();
            var injectedHolder = construktion.Construct<InjectHolder>();
            var injectedHolder2 = construktion.Construct<InjectHolder>();
            
            injected.GetHashCode().ShouldBe(injectedHolder.Injected.GetHashCode());

            injectedHolder.Injected
                        .GetHashCode()
                        .ShouldBe(injectedHolder2.Injected.GetHashCode());
        }

        [Fact]
        public void injected_instance_should_be_scoped_to_construktion_instance()
        {
            var injected = new Inject();

            var construktion = new Construktion().Inject(injected);

            var injectedHolder = new Construktion().Construct<InjectHolder>();

            injectedHolder.Injected
                        .GetHashCode()
                        .ShouldNotBe(injected.GetHashCode());
        }

        public class InjectBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context) => context.RequestType == typeof(Inject);

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var injected = new Inject();

                pipeline.Inject(context.RequestType, injected);

                return injected;
            }
        }

        public class Inject { }

        public class InjectHolder
        {
            public Inject Injected { get; }

            public InjectHolder(Inject injected)
            {
                Injected = injected;
            }
        }
    }
}