namespace Construktion.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
    using Shouldly;
    using Xunit;

    public class EnumerableBlueprintTests 
    {
        [Fact]
        public void should_match_all_enumerables_of_t()
        {
            var blueprint = new EnumerableBlueprint();

            var matches = blueprint.Matches(new ConstruktionContext(typeof(IEnumerable<>)));

            matches.ShouldBe(true);
        }

        [Fact]
        public void should_build_simple_enumerable_type()
        {
            var result = new Construktion().Construct<IEnumerable<int>>();
            
            result.Count().ShouldBe(3);
            result.ShouldAllBe(x => x != 0);
        }

        [Fact]
        public void should_build_a_complex_enumerable_type()
        {
            var result = new Construktion().Construct<IEnumerable<Foo>>();

            
            result.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Bar));
            result.ShouldAllBe(x => x.Baz != 0);
        }

        [Fact]
        public void should_build_nested_enumerables()
        {
            var result = new Construktion().Construct<IEnumerable<Bar>>();

            result.ShouldAllBe(x => x != null);

            var foos = result.SelectMany(x => x.Foo);
            foos.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Bar));
            foos.ShouldAllBe(x => x.Baz != 0);
        }

        [Fact]
        public void should_build_a_list()
        {
            var result = new Construktion().Construct<List<int>>();

            result.ShouldNotBe(null);
            result.ShouldAllBe(x => x != default(int));
        }

        [Fact]
        public void should_build_icollections()
        {
            var result = new Construktion().Construct<ICollection<Foo>>();

            result.ShouldNotBe(null);
            result.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Bar));
            result.ShouldAllBe(x => x.Baz != 0);
        }

        public class Foo
        {
            public string Bar { get; set; }
            public int Baz { get; set; }
        }

        public class Bar
        {
            public IEnumerable<Foo> Foo { get; set; }
        }
    }
}