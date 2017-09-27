// ReSharper disable PossibleMultipleEnumeration
namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints;
    using Blueprints.Simple;

    /// <summary>
    /// Base class for configuration settings
    /// </summary>
    public class ConstruktionRegistry
    {
        internal List<Blueprint> CustomBlueprints { get; } = new List<Blueprint>();
        internal IEnumerable<Blueprint> DefaultBlueprints { get; private set; } = new DefaultBlueprints();
        internal Func<List<ConstructorInfo>, ConstructorInfo> CtorStrategy { get; private set; } 
        internal Func<Type, IEnumerable<PropertyInfo>> PropertyStrategy { get; private set; }
        internal int? RepeatCount { get; private set; }
        internal int? RecurssionDepth { get; private set; } 
        internal Dictionary<Type, Type> TypeMap { get; } = new Dictionary<Type, Type>();
        internal bool? ShouldThrowOnRecurssion { get; private set; }

        public ConstruktionRegistry()
        {

        }

        public ConstruktionRegistry(Action<ConstruktionRegistry> configure)
        {
            configure(this);
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstruktionRegistry AddBlueprint(Blueprint blueprint)
        {
            blueprint.GuardNull();

            CustomBlueprints.Add(blueprint);
            return this;
        }

        /// <summary>
        /// Register a blueprint to be added to the pipeline.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            CustomBlueprints.Add((Blueprint) Activator.CreateInstance(typeof(TBlueprint)));
            return this;
        }

        /// <summary>
        /// Register blueprints to be added to the pipeline.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstruktionRegistry AddBlueprints(IEnumerable<Blueprint> blueprints)
        {
            blueprints.GuardNull();

            CustomBlueprints.AddRange(blueprints);
            return this;
        }

        /// <summary>
        /// When the type is requested, construct the substitute type instead.
        /// Useful when you want to construct a specific implementation whenever an interface is requested.
        /// </summary>
        /// <typeparam name="TContract">The type to be substituted</typeparam>
        /// <typeparam name="TImplementation">Will be used for substitution</typeparam>
        public ConstruktionRegistry Register<TContract, TImplementation>() where TImplementation : TContract
        {
            TypeMap[typeof(TContract)] = typeof(TImplementation);
            return this;
        }

        /// <summary>
        /// Register a single instance that will be supplied whenever the type is requested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public ConstruktionRegistry UseInstance<T>(T instance)
        {
            CustomBlueprints.Insert(0, new ScopedBlueprint(typeof(T), instance));
            return this;
        }

        /// <summary>
        /// Adds a blueprint that will use the property's attribute to construct it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry AddPropertyAttribute<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new PropertyAttributeBlueprint<T>(value);

            CustomBlueprints.Add(attributeBlueprint);
            return this;
        }

        /// <summary>
        /// Adds a blueprint that will use the parameter's attribute to construct it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry AddParameterAttribute<T>(Func<T, object> value) where T : Attribute
        {
            var attributeBlueprint = new ParameterAttributeBlueprint<T>(value);

            CustomBlueprints.Add(attributeBlueprint);
            return this;
        }

        /// <summary>
        /// Postfixes an email domain to string properties named "Email" or "EmailAddress".
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddEmailBlueprint()
        {
            CustomBlueprints.Add(new EmailAddressBlueprint());
            return this;
        }

        /// <summary>
        /// Postfixes an email domain to string properties that match the convention.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddEmailBlueprint(Func<PropertyInfo, bool> convention)
        {
            CustomBlueprints.Add(new EmailAddressBlueprint(convention));
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the fewest arguments. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseModestCtor()
        {
            CtorStrategy = Extensions.ModestCtor;
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the most arguments.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseGreedyCtor()
        {
            CtorStrategy = Extensions.GreedyCtor;
            return this;
        }

        /// <summary>
        /// Omit properties with private setters. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry OmitPrivateSetters()
        {
            PropertyStrategy = Extensions.PropertiesWithPublicSetter;
            return this;
        }

        /// <summary>
        /// Construct properties with private setters.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPrivateSetters()
        {
            PropertyStrategy = Extensions.PropertiesWithAccessibleSetter;
            return this;
        }

        /// <summary>
        /// Return 0 for ints and null for nullable ints ending in "Id"
        /// </summary>
        public ConstruktionRegistry OmitIds()
        {
            CustomBlueprints.Add(new OmitPropertyBlueprint(x => x.Name.EndsWith("Id", StringComparison.Ordinal),
                new List<Type> { typeof(int), typeof(int?) }));
            return this;
        }

        /// <summary>
        /// Omit all properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Type propertyType)
        {
            CustomBlueprints.Add(new OmitPropertyBlueprint(x => true, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, Type propertyType)
        {
            CustomBlueprints.Add(new OmitPropertyBlueprint(convention, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention)
        {
            CustomBlueprints.Add(new OmitPropertyBlueprint(convention, new List<Type>()));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified types.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, params Type[]  propertyTypes)
        {
            CustomBlueprints.Add(new OmitPropertyBlueprint(convention, propertyTypes));
            return this;
        }

        /// <summary>
        /// Omit all virtual properties.
        /// </summary>
        public void OmitVirtualProperties()
        {
            CustomBlueprints.Add(new IgnoreVirtualPropertiesBlueprint());
        }

        /// <summary>
        /// Configure how many items to be returned in an IEnumerable. The default is 3.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws argument exception when count is less than 0</exception>
        public ConstruktionRegistry EnumerableCount(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count cannot be less than 0");

            RepeatCount = count;
            return this;
        }

        /// <summary>
        /// Configure how many levels of recursion to construct. By default recursive properties are ignored.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws argument exception when limit is less than 0</exception>
        public ConstruktionRegistry RecurssionLimit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Recursion limit cannot be less than 0");

            RecurssionDepth = limit;
            return this;
        }

        /// <summary>
        /// Configure whether an exception should be thrown when recursive properties are detected. By default it won't throw.
        /// </summary>
        /// <param name="shouldThrow"></param>
        /// <returns></returns>
        public ConstruktionRegistry ThrowOnRecurssion(bool shouldThrow)
        {
            ShouldThrowOnRecurssion = shouldThrow;
            return this;
        }

        /// <summary>
        /// Construct properties from a supplied function matching a convention.
        /// </summary>
        /// <param name="convention"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPropertyUsing(Func<PropertyInfo, bool> convention, Func<object> value)
        {
            CustomBlueprints.Add(new CustomPropertyValueBlueprint(convention, value));
            return this;
        }

        internal void AddRegistry(ConstruktionRegistry registry)
        {
            registry.GuardNull();

            CustomBlueprints.AddRange(registry.CustomBlueprints);

            foreach (var map in registry.TypeMap)
                TypeMap[map.Key] = map.Value;

            CtorStrategy = registry.CtorStrategy ?? CtorStrategy;
            PropertyStrategy = registry.PropertyStrategy ?? PropertyStrategy;
            RepeatCount = registry.RepeatCount ?? RepeatCount;
            RecurssionDepth = registry.RecurssionDepth ?? RecurssionDepth;
            ShouldThrowOnRecurssion = registry.ShouldThrowOnRecurssion ?? ShouldThrowOnRecurssion;

            DefaultBlueprints = new DefaultBlueprints(TypeMap);
        }

        internal ConstruktionSettings ToSettings()
        {
            return new DefaultConstruktionSettings(this);
        }
    }
}