namespace Construktion
{
    using System;

    public class RequestContext
    {
        public Construktion Construktion { get; }
        public Type Request { get; }

        public RequestContext(Construktion construktion, Type request)
        {
            Construktion = construktion;
            Request = request;
        }
    }
}