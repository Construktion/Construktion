﻿namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using Blueprints;

    public class Construktion
    {
        private readonly List<Blueprint> _blueprints = Default.Blueprints;

        public Construktion()
        {

        }

        public Construktion(Blueprint blueprint)
        {
            if (blueprint == null)
                throw new ArgumentNullException(nameof(blueprint));

            _blueprints.Insert(0, blueprint);
        }

        public Construktion(BlueprintRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            _blueprints.InsertRange(0, registry.Blueprints);
        }

        public Construktion(Action<BlueprintRegistry> config)
        {
            var registry = new BlueprintRegistry();

            config(registry);

            _blueprints.InsertRange(0, registry.Blueprints);
        }

        public T Construct<T>()
        {
            return DoConstruct<T>(typeof(T), null);
        }

        public T Construct<T>(Action<T> hardCodes)
        {
            return DoConstruct(typeof(T), hardCodes);
        }

        public object Construct(Type request)
        {
            return DoConstruct<object>(request, null);
        }

        private T DoConstruct<T>(Type request, Action<T> hardCodes)
        {
            var pipeline = new DefaultConstruktionPipeline(_blueprints);

            var result = (T)pipeline.Construct(new ConstruktionContext(request));
          
            hardCodes?.Invoke(result);

            return result;
        }
    }
}