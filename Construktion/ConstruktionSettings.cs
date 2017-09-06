namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;

    public interface ConstruktionSettings
    {
        /// <summary>
        /// All configured blueprints. The pipeline will evaulate them in the returned order.
        /// </summary>
        IEnumerable<Blueprint> Blueprints { get; }

        /// <summary>
        /// When a key in the dictionary is requested, will construct the value. Usually used to construct interfaces.
        /// </summary>
        IDictionary<Type, Type> TypeMap { get; }

        /// <summary>
        /// Resolve the constructor (Greedy or Modest). 
        /// </summary>
        Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }

        /// <summary>
        /// Resolve an objects properties to construct. 
        /// </summary>
        Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }

        /// <summary>
        /// The amount of items to create when any IEnumerable (or array) is requested. The Default is 3.
        /// </summary>
        int EnumuerableCount { get; }

        /// <summary>
        /// How many levels of recurssion to construct. By default recursive properties are ignored.
        /// </summary>
        int RecurssionDepth { get; }

        /// <summary>
        /// Determines whether an exception should be thrown when Recurssion is detected
        /// </summary>
        bool ThrowOnRecurrsion { get; }
    }

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

        public DefaultConstruktionSettings() : this (new ConstruktionRegistry())
        {
            
        }

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