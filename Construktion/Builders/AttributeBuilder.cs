using System.ComponentModel.DataAnnotations;

namespace Construktion.Builders
{
    using System.Linq;
    using System.Reflection;

    public class AttributeBuilder : Builder
    {
        public bool CanBuild(RequestContext context)
        {
            return context.PropertyInfo?.GetCustomAttributes(typeof(MaxLengthAttribute)).ToList().Any() ?? false;
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            return "attr";
        }
    }
}