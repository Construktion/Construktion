// ReSharper disable PossibleMultipleEnumeration

namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Blueprints.Simple;
    using Internal;

    public class ConstruktionRegistry
    {
        internal DefaultConstruktionSettings Settings { get; }

        public ConstruktionRegistry()
        {
            Settings = new DefaultConstruktionSettings();
        }

        public ConstruktionRegistry(Action<ConstruktionRegistry> configure) : this()
        {
            configure(this);
        }

        /// <summary>
        /// Add a custom blueprint to the pipeline that will construct an object.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstruktionRegistry AddBlueprint(Blueprint blueprint)
        {
            blueprint.GuardNull();

            Settings.Apply(blueprint);
            return this;
        }

        /// <summary>
        /// Add a custom blueprint to the pipeline that will construct an object.
        /// </summary>
        public ConstruktionRegistry AddBlueprint<TBlueprint>() where TBlueprint : Blueprint, new()
        {
            Settings.Apply((Blueprint)Activator.CreateInstance(typeof(TBlueprint)));
            return this;
        }

        /// <summary>
        /// Add custom blueprints to the pipeline that will construct an object.
        /// </summary>
        public ConstruktionRegistry AddBlueprints(IEnumerable<Blueprint> blueprints)
        {
            blueprints.GuardNull();
            Settings.Apply(blueprints);
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
            Settings.Apply(typeof(TContract), typeof(TImplementation));
            return this;
        }

        /// <summary>
        /// Register a single instance that will be supplied whenever the type is requested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public ConstruktionRegistry UseInstance<T>(T instance)
        {
            Settings.UseInstance(instance);
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

            Settings.Apply(attributeBlueprint);
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

            Settings.Apply(attributeBlueprint);
            return this;
        }

        /// <summary>
        /// Postfixes an email domain to string properties named "Email" or "EmailAddress".
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddEmailBlueprint()
        {
            Settings.Apply(new EmailAddressBlueprint());
            return this;
        }

        /// <summary>
        /// Postfixes an email domain to string properties that match the convention.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry AddEmailBlueprint(Func<PropertyInfo, bool> convention)
        {
            Settings.Apply(new EmailAddressBlueprint(convention));
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the fewest arguments. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseModestCtor()
        {
            Settings.Apply(Ctors.Modest);
            return this;
        }

        /// <summary>
        /// Construct objects using the constructor with the most arguments.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry UseGreedyCtor()
        {
            Settings.Apply(Ctors.Greedy);
            return this;
        }

        /// <summary>
        /// Omit properties with private setters. This is the default behavior.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry OmitPrivateSetters()
        {
            Settings.Apply(PropertySetters.Public);
            return this;
        }

        /// <summary>
        /// Construct properties with private setters.
        /// </summary>
        /// <returns></returns>
        public ConstruktionRegistry ConstructPrivateSetters()
        {
            Settings.Apply(PropertySetters.Accessible);
            return this;
        }

        /// <summary>
        /// Return 0 for ints and null for nullable ints ending in "Id"
        /// </summary>
        public ConstruktionRegistry OmitIds()
        {
            Settings.Apply(new OmitPropertyBlueprint(x => x.Name.EndsWith("Id", StringComparison.Ordinal),
                new List<Type> { typeof(int), typeof(int?) }));
            return this;
        }

        /// <summary>
        /// Omit all properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Type propertyType)
        {
            Settings.Apply(new OmitPropertyBlueprint(x => true, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified type.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, Type propertyType)
        {
            Settings.Apply(new OmitPropertyBlueprint(convention, propertyType));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention)
        {
            Settings.Apply(new OmitPropertyBlueprint(convention, new List<Type>()));
            return this;
        }

        /// <summary>
        /// Specify a convention to omit properties of the specified types.
        /// </summary>
        public ConstruktionRegistry OmitProperties(Func<PropertyInfo, bool> convention, params Type[] propertyTypes)
        {
            Settings.Apply(new OmitPropertyBlueprint(convention, propertyTypes));
            return this;
        }

        /// <summary>
        /// Omit all virtual properties.
        /// </summary>
        public void OmitVirtualProperties()
        {
            Settings.Apply(new IgnoreVirtualPropertiesBlueprint());
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

            Settings.SetEnumerableCount(count);
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

            Settings.SetRecursionDepth(limit);
            return this;
        }

        /// <summary>
        /// Configure whether an exception should be thrown when recursive properties are detected. By default it won't throw.
        /// </summary>
        /// <param name="shouldThrow"></param>
        /// <returns></returns>
        public ConstruktionRegistry ThrowOnRecurssion(bool shouldThrow)
        {
            Settings.SetThrowOnRecursion(shouldThrow);
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
            Settings.Apply(new CustomPropertyValueBlueprint(convention, value));
            return this;
        }

        /// <summary>
        /// Add an exit blueprint to the pipeline. These blueprints are called 
        /// at the end of the chain after all regular blueprints. They receive
        /// a fully constructed object and are the final chance to  alter the
        ///  result of an object.   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public ConstruktionRegistry AddExitBlueprint<T>() where T : ExitBlueprint, new()
        {
            Settings.Apply(new T());
            return this;
        }

        /// <summary>
        /// Add an exit blueprint to the pipeline. These blueprints are called 
        /// at the end of the chain after all regular blueprints. They receive
        /// a fully constructed object and are the final chance to  alter the
        ///  result of an object.   
        /// </summary>
        public ConstruktionRegistry AddExitBlueprint(ExitBlueprint blueprint)
        {
            Settings.Apply(blueprint);
            return this;
        }
    }
}