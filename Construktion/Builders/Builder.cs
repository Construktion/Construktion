namespace Construktion.Builders
{
    using System;

    public interface Builder 
    {
        bool CanBuild(Type request);
        object Build(RequestContext context);
    }
}
