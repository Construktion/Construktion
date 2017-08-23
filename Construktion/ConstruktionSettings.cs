namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Blueprints;

    public interface ConstruktionSettings
    {
        List<Blueprint> Blueprints { get; }
        IDictionary<Type, Type> Mappings { get; }
        Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }
        Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }
        int EnumuerableCount { get; }
        int RecurssionDepth { get; }
    }

    internal class DefaultConstruktionSettings : ConstruktionSettings
    {
        public List<Blueprint> Blueprints { get; private set; }
        public IDictionary<Type, Type> Mappings { get; } = new Dictionary<Type, Type>();
        public Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }
        public Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }
        public int EnumuerableCount { get; }
        public int RecurssionDepth { get; }

        public DefaultConstruktionSettings() : this (new ConstruktionRegistry())
        {
            
        }

        public DefaultConstruktionSettings(ConstruktionRegistry registry)
        {
            Blueprints = registry.CustomBlueprints;
            Blueprints.AddRange(registry.DefaultBlueprints);

            foreach (var map in registry._typeMap)
                Mappings[map.Key] = map.Value;

            CtorStrategy = registry.CtorStrategy ?? Extensions.GreedyCtor;
            PropertyStrategy = registry.PropertyStrategy ?? Extensions.PropertiesWithPublicSetter;
            EnumuerableCount = registry.RepeatCount ?? 3;
            RecurssionDepth = registry.RecurssionDepth ?? 0;
        }

        internal void SubstituteBlueprints(IEnumerable<Blueprint> blueprints)
        {
            Blueprints= blueprints.ToList();
        }
    }
}