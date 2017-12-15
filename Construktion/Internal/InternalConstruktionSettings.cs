namespace Construktion.Internal
{
    using System;
    using System.Collections.Generic;

    internal interface InternalConstruktionSettings : ConstruktionSettings
    {
        void Apply(ConstruktionRegistry registry);

        void Apply(Blueprint blueprint);

        void Apply(IEnumerable<Blueprint> blueprints);

        void Apply(ExitBlueprint exitBlueprint);

        void Apply(Type contract, Type implementation);

        void UseInstance<T>(T instance);

        void Inject(Type type, object value);
    }
}