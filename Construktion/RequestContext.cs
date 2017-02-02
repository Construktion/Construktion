namespace Construktion
{
    using System;

    public class RequestContext
    {
        public Type Request { get; }

        public RequestContext(Type request)
        {
            Request = request;
        }
    }
}