namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;

    public interface ConstruktionSettings
    {
        /// <summary>
        /// All configured blueprints. The pipeline will evaluate them in the returned order.
        /// </summary>
        IEnumerable<Blueprint> Blueprints { get; }

        /// <summary>
        /// When a key in the dictionary is requested, will construct the value. Usually used to construct interfaces.
        /// </summary>
        IDictionary<Type, Type> TypeMap { get; }

        /// <summary>
        /// Resolve the constructor (Greedy or Modest). Uses modest by default.
        /// </summary>
        Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }

        /// <summary>
        /// Resolve an objects properties to construct. 
        /// </summary>
        Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }

        /// <summary>
        /// The amount of items to create when any IEnumerable (or array) is requested. The Default is 3.
        /// </summary>
        int EnumerableCount { get; }

        /// <summary>
        /// How many levels of recursion to construct. By default recursive properties are ignored.
        /// </summary>
        int RecursionDepth { get; }

        /// <summary>
        /// When true, an exception will be thrown when Recursion is detected. False by default.
        /// </summary>
        bool ThrowOnRecursion { get; }
    }

    internal class DefaultConstruktionSettings : ConstruktionSettings
    {
        private readonly List<Blueprint> _blueprints;
        public IEnumerable<Blueprint> Blueprints => _blueprints;
        public IDictionary<Type, Type> TypeMap { get; }
        public Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; }
        public Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; }
        public int EnumerableCount { get; }
        public int RecursionDepth { get; }
        public bool ThrowOnRecursion { get; }

        public DefaultConstruktionSettings() : this(new ConstruktionRegistry())
        {

        }

        public DefaultConstruktionSettings(ConstruktionRegistry registry)
        {
            _blueprints = new List<Blueprint>(registry.CustomBlueprints);
            _blueprints.AddRange(registry.DefaultBlueprints);

            TypeMap = registry.TypeMap;
            CtorStrategy = registry.CtorStrategy ?? Extensions.ModestCtor;
            PropertyStrategy = registry.PropertyStrategy ?? Extensions.PropertiesWithPublicSetter;
            EnumerableCount = registry.RepeatCount ?? 3;
            RecursionDepth = registry.RecursionDepth ?? 0;
            ThrowOnRecursion = registry.ShouldThrowOnRecursion ?? false;
        }
    }
}