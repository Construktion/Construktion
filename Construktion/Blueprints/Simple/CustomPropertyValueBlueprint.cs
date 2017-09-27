using System;
using System.Reflection;

namespace Construktion.Blueprints.Simple
{
    public class CustomPropertyValueBlueprint : Blueprint
    {
        private readonly Func<PropertyInfo, bool> _convention;
        private readonly Func<object> _value;

        public CustomPropertyValueBlueprint(Func<PropertyInfo, bool> convention, Func<object> value)
        {
            _convention = convention;
            _value = value;
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo != null && _convention(context.PropertyInfo);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return _value();
        }
    }
}