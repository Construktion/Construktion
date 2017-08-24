namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;

    public interface ConstruktionSettings
    {
        List<Blueprint> Blueprints { get; }
        IDictionary<Type, Type> TypeMap { get; }
        Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }
        Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }
        int EnumuerableCount { get; }
        int RecurssionDepth { get; }
    }

    internal class DefaultConstruktionSettings : ConstruktionSettings
    {
        public List<Blueprint> Blueprints { get; }
        public IDictionary<Type, Type> TypeMap { get; } = new Dictionary<Type, Type>();
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

            foreach (var map in registry.TypeMap)
                TypeMap[map.Key] = map.Value;

            CtorStrategy = registry.CtorStrategy ?? Extensions.GreedyCtor;
            PropertyStrategy = registry.PropertyStrategy ?? Extensions.PropertiesWithPublicSetter;
            EnumuerableCount = registry.RepeatCount ?? 3;
            RecurssionDepth = registry.RecurssionDepth ?? 0;
        }
    }
}