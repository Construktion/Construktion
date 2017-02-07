namespace Construktion.Builders
{
    using System.ComponentModel.DataAnnotations;

    public class AttributeBuilder : Builder
    {
        public bool CanBuild(RequestContext context)
        {
            return context.HasAttribute<MaxLengthAttribute>();
        }

        public object Build(RequestContext context, ConstruktionPipeline pipeline)
        {
            return "attr";
        }
    }
}