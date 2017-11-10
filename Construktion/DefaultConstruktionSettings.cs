using System;
using System.Collections.Generic;
using System.Reflection;
using Construktion.Blueprints;

namespace Construktion
{
    internal class DefaultConstruktionSettings : ConstruktionSettings
    {
        private readonly List<Blueprint> _blueprints;
        public IEnumerable<Blueprint> Blueprints => _blueprints;
        public IDictionary<Type, Type> TypeMap { get; }
        public Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }
        public Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }
        public int EnumuerableCount { get; }
        public int RecurssionDepth { get; }
        public bool ThrowOnRecurrsion { get; }

        public DefaultConstruktionSettings(ConstruktionRegistry registry)
        {
            _blueprints = new List<Blueprint>(registry.CustomBlueprints);
            _blueprints.AddRange(registry.DefaultBlueprints);

            TypeMap = registry.TypeMap;
            CtorStrategy = registry.CtorStrategy ?? Extensions.ModestCtor;
            PropertyStrategy = registry.PropertyStrategy ?? Extensions.PropertiesWithPublicSetter;
            EnumuerableCount = registry.RepeatCount ?? 3;
            RecurssionDepth = registry.RecurssionDepth ?? 0;
            ThrowOnRecurrsion = registry.ShouldThrowOnRecurssion ?? false;
        }
    }
}