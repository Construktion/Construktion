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
        private readonly Construktion construktion;

        public ConstructManyTests()
        {
            construktion = new Construktion();
        }

        [Fact]
        public void should_construct_many_with_3_items_by_default()
        {
            var results = construktion.ConstructMany<int>();

            results.Count().ShouldBe(3);
            results.ShouldAllBe(x => x != 0);
        }

        [Fact]
        public void should_construct_many_with_specified_count()
        {
            var results = construktion.ConstructMany<int>(5);

            results.Count().ShouldBe(5);
            results.ShouldAllBe(x => x != 0);
        }

        [Fact]
        public void should_construct_many_with_hardcodes()
        {
            var result = construktion.ConstructMany<Bar>(x => x.Name = "Name");

            result.ShouldAllBe(x => x.Name == "Name");
        }

        [Fact]
        public void should_construct_many_with_hardcodes_and_count()
        {
            var result = construktion.ConstructMany<Bar>(x => x.Name = "Name", 5);

            result.Count().ShouldBe(5);
            result.ShouldAllBe(x => x.Name == "Name");
        }

        [Fact]
        public void should_set_enumerable_count_globally()
        {
            construktion.With(x => x.EnumerableCount(2));

            var ints = construktion.ConstructMany<int>();
            var bars = construktion.ConstructMany<Bar>();

            ints.Count().ShouldBe(2);
            bars.Count().ShouldBe(2);
        }

        [Fact]
        public void should_set_enumerable_count_for_entire_graph()
        {
            construktion.With(x => x.EnumerableCount(2));

            var foos = construktion.ConstructMany<Foo>();

            foos.Count().ShouldBe(2);
            foos.ShouldAllBe(x => x.Bars.Count() == 2);
        }

        [Fact]
        public void should_set_enumerable_count_for_arrays()
        {
            construktion.With(x => x.EnumerableCount(2));

            var ints = construktion.Construct<int[]>();

            ints.Length.ShouldBe(2);
        }

        [Fact]
        public void should_throw_when_setting_a_negative_count()
        {
            Exception<ArgumentException>.ShouldBeThrownBy(() => construktion.With(x => x.EnumerableCount(-1)));
        }

	    [Fact]
	    public void construt_many_should_throw_when_setting_a_negative_count()
	    {
		    Exception<ArgumentException>.ShouldBeThrownBy(() => construktion.ConstructMany<int>(-1));
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