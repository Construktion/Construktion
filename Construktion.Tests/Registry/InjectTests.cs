namespace Construktion.Tests.Registry
{
    using Shouldly;
    using Xunit;

    public class InjectTests
    {
        [Fact]
        public void should_inject_instance()
        {
            var inject = new Inject();
            var construktion = new Construktion().Inject(inject);

            var injectHolder = construktion.Construct<InjectHolder>();

            injectHolder.Inject
                        .GetHashCode()
                        .ShouldBe(inject.GetHashCode());
        }

	    [Fact]
	    public void should_inject_instance_with_explicit_type()
	    {
		    var injected = new StringHolderProxy("value");

		    var construktion = new Construktion().Inject(typeof(StringHolder), injected);

		    var stringHolder = construktion.Construct<StringHolder>();

		    stringHolder.Value.ShouldBe("value");
		    stringHolder.ShouldBeOfType<StringHolderProxy>();
			stringHolder.GetHashCode().ShouldBe(injected.GetHashCode());
	    }

		[Fact]
        public void should_always_use_the_same_instance()
        {
            var inject = new Inject();
            var construktion = new Construktion().Inject(inject);

            var injectHolder = construktion.Construct<InjectHolder>();
            var injectHolder2 = construktion.Construct<InjectHolder>();

            injectHolder.Inject
                        .GetHashCode()
                        .ShouldBe(inject.GetHashCode());

            injectHolder2.Inject
                         .GetHashCode()
                         .ShouldBe(inject.GetHashCode());
        }

        [Fact]
        public void should_inject_from_within_pipeline()
        {
            var construktion = new Construktion().With(new InjectBlueprint());

            var inject = construktion.Construct<Inject>();
            var injectHolder = construktion.Construct<InjectHolder>();

            injectHolder.Inject
                        .GetHashCode()
                        .ShouldBe(inject.GetHashCode());
        }

        [Fact]
        public void should_always_use_the_same_instance_when_injected_from_pipeline()
        {
            var construktion = new Construktion().With(new InjectBlueprint());

            var inject = construktion.Construct<Inject>();
            var injectHolder = construktion.Construct<InjectHolder>();
            var injectHolder2 = construktion.Construct<InjectHolder>();

            inject.GetHashCode().ShouldBe(injectHolder.Inject.GetHashCode());

            injectHolder.Inject
                        .GetHashCode()
                        .ShouldBe(injectHolder2.Inject.GetHashCode());
        }

        [Fact]
        public void inject_instance_should_be_scoped_to_construktion_instance()
        {
            var inject = new Inject();

            var construktion = new Construktion().Inject(inject);

            var injectHolder = new Construktion().Construct<InjectHolder>();

            injectHolder.Inject
                        .GetHashCode()
                        .ShouldNotBe(inject.GetHashCode());
        }

        public class InjectBlueprint : Blueprint
        {
            public bool Matches(ConstruktionContext context) => context.RequestType == typeof(Inject);

            public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
            {
                var inject = new Inject();

                pipeline.Inject(context.RequestType, inject);

                return inject;
            }
        }

        public class Inject { }

        public class InjectHolder
        {
            public Inject Inject { get; }

            public InjectHolder(Inject inject)
            {
                Inject = inject;
            }
        }

	    public class StringHolderProxy : StringHolder
	    {
		    public StringHolderProxy(string value) : base (value)
		    {
			    
		    }
	    }

	    public class StringHolder
	    {
		    public string Value { get; }

		    public StringHolder(string value)
		    {
			    Value = value;
		    }
	    }
    }
}