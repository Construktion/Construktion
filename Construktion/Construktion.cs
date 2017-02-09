﻿namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
 
    public class Construktion
    {
        private readonly List<Blueprint> _blueprints = new List<Blueprint>
        {
            new StringBlueprint(),
            new NumericBlueprint(),
            new CharBlueprint(),
            new GuidBlueprint(),
            new BoolBlueprint(),
            new EnumBlueprint(),
            new ClassBlueprint()
        };

        public IList<Blueprint> Blueprints => _blueprints;

        public Construktion()
        {
        }

        public Construktion(Blueprint additionalBlueprint) : this (new List<Blueprint> { additionalBlueprint })
        {
        }

        public Construktion(IList<Blueprint> additionalBlueprints)
        {
            if (additionalBlueprints.Any(x => x == null))
                throw new ArgumentNullException(nameof(additionalBlueprints), "There are items in the list that are null");

            _blueprints.InsertRange(0, additionalBlueprints);
        }

        public T Build<T>()
        {
            return DoBuild<T>(typeof(T), null);
        }

        public T Build<T>(Action<T> hardCodes)
        {
            return DoBuild(typeof(T), hardCodes);
        }

        public object Build(Type type)
        {
            return DoBuild<object>(type, null);
        }

        private T DoBuild<T>(Type request, Action<T> hardCodes)
        {
            var pipeline = new DefaultConstruktionPipeline(_blueprints);

            var result = (T)pipeline.Build(new ConstruktionContext(request));

            hardCodes?.Invoke(result);

            return result;
        }
    }
}
