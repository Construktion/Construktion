// ReSharper disable PossibleMultipleEnumeration
namespace Construktion.Tests.Acceptance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shouldly;
    using Xunit;

    public class ConstructManyTests
    {
        private readonly Construktion _contruktion;

        public ConstructManyTests()
        {
            _contruktion = new Construktion();
        }

        [Fact]
        public void should_construct_many_with_3_items_by_default()
        {
            var results = _contruktion.ConstructMany<int>();

            results.Count().ShouldBe(3);
            results.ShouldAllBe(x => x != 0);
        }

        [Fact]
        public void should_construct_many_with_specified_count()
        {
            var results = _contruktion.ConstructMany<int>(5);

            results.Count().ShouldBe(5);
            results.ShouldAllBe(x => x != 0);
        }

        [Fact]
        public void should_construct_many_with_hardcodes()
        {
            var result = _contruktion.ConstructMany<Bar>(x => x.Name = "Name");

            result.ShouldAllBe(x => x.Name == "Name");
        }

        [Fact]
        public void should_construct_many_with_hardcodes_and_count()
        {
            var result = _contruktion.ConstructMany<Bar>(x => x.Name = "Name", 5);

            result.Count().ShouldBe(5);
            result.ShouldAllBe(x => x.Name == "Name");
        }

        [Fact]
        public void should_set_enumerable_count_globally()
        {
            _contruktion.With(x => x.EnumerableCount(2));

            var ints = _contruktion.ConstructMany<int>();
            var bars = _contruktion.ConstructMany<Bar>();

            ints.Count().ShouldBe(2);
            bars.Count().ShouldBe(2);
        }

        [Fact]
        public void should_set_enumerable_count_for_entire_graph()
        {
            _contruktion.With(x => x.EnumerableCount(2));

            var foos = _contruktion.ConstructMany<Foo>();

            foos.Count().ShouldBe(2);
            foos.ShouldAllBe(x => x.Bars.Count() == 2);
        }

        [Fact]
        public void should_throw_when_setting_a_negative_count()
        {
            Should.Throw<ArgumentException>(() => _contruktion.With(x => x.EnumerableCount(-1)));
        }

        public class Bar
        {
            public string Name { get; set; }
        }

        public class Foo
        {
            public IEnumerable<Bar> Bars { get; set; }
        }
    }
}
