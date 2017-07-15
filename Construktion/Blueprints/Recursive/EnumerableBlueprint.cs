﻿namespace Construktion.Blueprints.Recursive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EnumerableBlueprint : Blueprint
    {
        private readonly int _enumerableCount;

        public EnumerableBlueprint() : this(3)
        {
            
        }

        public EnumerableBlueprint(int enumerableCount)
        {
            _enumerableCount = enumerableCount;
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.RequestType.GetTypeInfo().IsGenericType &&
                   context.RequestType.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var closedType = context.RequestType.GenericTypeArguments[0];

            var results = construct(closedType, pipeline);

            return results;
        }

        private IList construct(Type closedType, ConstruktionPipeline pipeline)
        {
            var items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(closedType));

            for (var i = 0; i < _enumerableCount; i++)
            {
                var result = pipeline.Send(new ConstruktionContext(closedType));

                items.Add(result);
            }

            return items;
        }
    }
}