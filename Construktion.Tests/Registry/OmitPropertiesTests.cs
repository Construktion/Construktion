namespace Construktion.Tests.Registry
{
    using System.Collections.Generic;
    using System.Reflection;
    using Shouldly;
    using Xunit;

    public class OmitPropertiesTests
    {
        private readonly ConstruktionRegistry registry;
        private readonly Construktion construktion;

        public OmitPropertiesTests()
        {
            registry = new ConstruktionRegistry();
            construktion = new Construktion();
        }

        [Fact]
        public void omit_ids_should_omit_an_int_that_ends_in_Id()
        {
            registry.OmitIds();
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void omit_ids_should_omit_a_nullable_int_that_ends_in_Id()
        {
            registry.OmitIds();
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.NullableFooId.ShouldBe(null);
        }

        [Fact]
        public void should_be_case_sensitive()
        {
            registry.OmitIds();
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.Fooid.ShouldNotBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            registry.OmitProperties(prop => prop.Name.EndsWith("_Idx"), typeof(string));
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.String_Idx.ShouldBe(null);
        }

        [Fact]
        public void should_omit_all_properties_of_a_open_generic()
        {
            registry.OmitProperties(typeof(List<>));
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

            foo.ListInts.ShouldBe(null);
            foo.ListStrings.ShouldBe(null);
        }

        [Fact]
        public void should_ignore_virtual_properties_when_opted_in()
        {
            registry.OmitVirtualProperties();

            var foo = construktion.With(registry).Construct<Foo>();

            foo.MyVirtualClasses.ShouldBe(null);
            foo.VirtualInt.ShouldBe(0);
        }

	    [Fact]
	    public void should_omit_all_properties_of_type()
	    {
		    registry.OmitProperties(prop => prop.Name == "Omit", typeof(string), typeof(int));
		    construktion.With(registry);

		    var foo = construktion.Construct<Foo>();
		    var bar = construktion.Construct<Bar>();

			foo.Omit.ShouldBe(null);
			bar.Omit.ShouldBe(0);
	    }

		[Fact]
        public void should_omit_all_properties_that_inherit_a_generic()
        {
            registry.OmitProperties(prop => prop.PropertyType.GetTypeInfo().BaseType != null &&
                                            prop.PropertyType.BaseType.IsGenericType &&
                                            prop.PropertyType.BaseType.GetGenericTypeDefinition() ==
                                            typeof(OpenGeneric<>));
            construktion.With(registry);

            var foo = construktion.Construct<Foo>();

			foo.InheritsGeneric.ShouldBe(null);
        }

        public class Foo
        {
            public int FooId { get; set; }
            public int? NullableFooId { get; set; }
            public int Fooid { get; set; }
            public string String_Idx { get; set; }
			public string Omit { get; set; }

            public List<int> ListInts { get; set; }
            public List<string> ListStrings { get; set; }
            public virtual ICollection<MyClass> MyVirtualClasses { get; set; }
            public virtual int VirtualInt { get; set; }
            public InheritsGeneric InheritsGeneric { get; set; }
        }

	    public class Bar
	    {
		    public int Omit { get; set; }
	    }

        public class MyClass { }

        public class InheritsGeneric : OpenGeneric<string> { }

        public class OpenGeneric<T> { }
    }
}