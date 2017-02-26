namespace Construktion.Tests
{
    using System.Collections.Generic;
    using Shouldly;
    using Xunit;

    public class RecursionGuardTests
    {
        [Fact]
        public void should_ignore_on_recursion()
        {
            var one = new Construktion().Construct<One>();

            one.Name.ShouldNotBeNullOrWhiteSpace();

            one.Two.Name.ShouldNotBeNullOrWhiteSpace();
            one.Two.One.ShouldBe(null);
        }

        [Fact]
        public void should_handle_recurrsion_deep_in_the_graph()
        {
            var deepReccursion = new Construktion().Construct<DeepRecurrsion>();

            deepReccursion.Name.ShouldNotBeNullOrWhiteSpace();
            deepReccursion.MyClass.Name.ShouldNotBeNullOrWhiteSpace();

            deepReccursion.MyClass.One.ShouldNotBe(null);
            deepReccursion.MyClass.One.Name.ShouldNotBeNullOrWhiteSpace();

            deepReccursion.MyClass.One.Two.ShouldNotBe(null);
            deepReccursion.MyClass.One.Two.Name.ShouldNotBeNullOrWhiteSpace();

            deepReccursion.MyClass.One.Two.One.ShouldBe(null);
        }

        [Fact]
        public void should_handle_an_enunmerable_with_recursive_property_correctly()
        {
            var deepReccursion = new Construktion().Construct<DeepRecurrsionWithEnumerable>();

            deepReccursion.Name.ShouldNotBeNullOrWhiteSpace();

            deepReccursion.MyClass.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Name));
            deepReccursion.MyClass.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.One.Name));
            deepReccursion.MyClass.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.One.Two.Name));
            deepReccursion.MyClass.ShouldAllBe(x => x.One.Two.One == null);
        }


        public class One
        {
            public string Name { get; set; }
            public Two Two { get; set; }
        }

        public class Two
        {
            public string Name { get; set; }
            public One One { get; set; }
        }

        public class DeepRecurrsion
        {
            public string Name { get; set; }
            public MyClass MyClass { get; set; }
        }

        public class DeepRecurrsionWithEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<MyClass> MyClass { get; set; }
        }

        public class MyClass
        {
            public string Name { get; set; }
            public One One { get; set; }
        }
    }
}