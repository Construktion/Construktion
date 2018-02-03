namespace Construktion.Tests.Bugs
{
    using System;
    using Blueprints.Simple;
    using Shouldly;

    public class Bug_39_interface_properties_are_matching_the_ignore_virtual_properties_blueprint
    {
        public void interface_properties_should_not_match_virtual_blueprint()
        {
            var blueprint = new IgnoreVirtualPropertiesBlueprint();
            var dateProp = typeof(Foo).GetProperty(nameof(Foo.DateTime));
            var stringProp = typeof(Foo).GetProperty(nameof(Foo.String));

            var matchesDate = blueprint.Matches(new ConstruktionContext(dateProp));
            var matchesString = blueprint.Matches(new ConstruktionContext(stringProp));

            matchesDate.ShouldBe(false);
            matchesString.ShouldBe(false);
        }

        public class Foo : IFoo
        {
            public DateTime DateTime { get; set; }
            public string String { get; set; }
        }

        public interface IFoo
        {
            DateTime DateTime { get; set; }
            string String { get; set; }
        }
    }
}