namespace Construktion.Tests.Registry
{
    using System.Collections.Generic;
    using Shouldly;
    using Xunit;

    public class OmitPropertiesTests
    {
        private readonly ConstruktionRegistry _registry;
        private readonly Construktion _construktion;
        
        public OmitPropertiesTests()
        {
            _registry = new ConstruktionRegistry();
            _construktion = new Construktion();
        }

        [Fact]
        public void omit_ids_should_omit_an_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.FooId.ShouldBe(0);
        }

        [Fact]
        public void omit_ids_should_omit_a_nullable_int_that_ends_in_Id()
        {
            _registry.OmitIds();
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.NullableFooId.ShouldBe(null);
        }

        [Fact]
        public void should_be_case_sensitive()
        {
            _registry.OmitIds();
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.Fooid.ShouldNotBe(0);
        }

        [Fact]
        public void should_be_able_to_define_a_custom_convention()
        {
            _registry.OmitProperties(prop => prop.Name.EndsWith("_Id"), typeof(string));
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.String_Id.ShouldBe(null);
        }

        [Fact]
        public void should_omit_all_properties_of_a_open_generic()
        {
            _registry.OmitProperties(typeof(List<>));
            _construktion.With(_registry);

            var foo = _construktion.Construct<Foo>();

            foo.ListInts.ShouldBe(null);
            foo.ListStrings.ShouldBe(null);
        }

        [Fact]
        public void should_ignore_virtual_properties_when_opted_in()
        {
            _registry.OmitVirtualProperties();

            var foo = _construktion.With(_registry).Construct<Foo>();

            foo.MyVirtualClasses.ShouldBe(null);
            foo.VirtualInt.ShouldBe(0);
        }

        public class Foo 
        {
            public int FooId { get; set; }
            public int? NullableFooId { get; set; }
            public int Fooid { get; set; }
            public string String_Id { get; set; }

            public List<int> ListInts { get; set; }
            public List<string> ListStrings { get; set; }
            public virtual ICollection<MyClass> MyVirtualClasses { get; set; }
            public virtual int VirtualInt { get; set; }

        }

        public class MyClass { }
    }
}