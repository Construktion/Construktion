namespace Construktion.Tests.Registry
{
    using Shouldly;

    public class MaxDepthTests
    {
        public void should_construct_top_level_properties()
        {
            var registry = new ConstruktionRegistry(x => x.MaxDepth(1));
            var construktion = new Construktion().With(registry);

            var result = construktion.Construct<LevelOne>();

            result.LevelTwo.ShouldNotBe(null);
            result.LevelTwo.Name.ShouldBe(null);
        }

        public void should_construct_deeper()
        {
            var registry = new ConstruktionRegistry(x => x.MaxDepth(2));
            var construktion = new Construktion().With(registry);

            var result = construktion.Construct<LevelOne>();

            result.LevelTwo.ShouldNotBe(null);
            result.LevelTwo.Name.ShouldNotBe(null);
            result.LevelTwo.LevelThree.ShouldNotBe(null);
            result.LevelTwo.LevelThree.Name.ShouldBe(null);
        }

        public void new_registries_should_overwrite_previous()
        {
            var registry = new ConstruktionRegistry(x => x.MaxDepth(2));
            var registry2 = new ConstruktionRegistry(x => x.MaxDepth(1));
            var construktion = new Construktion().With(registry).With(registry2);

            var result = construktion.Construct<LevelOne>();

            result.LevelTwo.ShouldNotBe(null);
            result.LevelTwo.Name.ShouldBe(null);
        }

        public void new_registries_without_a_setting_should_not_overwrite()
        {
            var registry = new ConstruktionRegistry(x => x.MaxDepth(1));
            var registry2 = new ConstruktionRegistry();
            var construktion = new Construktion().With(registry).With(registry2);

            var result = construktion.Construct<LevelOne>();

            result.LevelTwo.ShouldNotBe(null);
            result.LevelTwo.Name.ShouldBe(null);
        }

        public class LevelOne
        {
            public string Name { get; set; }
            public LevelTwo LevelTwo { get; set; }
        }

        public class LevelTwo
        {
            public string Name { get; set; }
            public LevelThree LevelThree { get; set; }
        }

        public class LevelThree
        {
            public string Name { get; set; }
        }
    }
}