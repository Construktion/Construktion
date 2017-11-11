namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EmailAddressBlueprint : Blueprint
    {
        private readonly Random random = new Random();
        private readonly Func<PropertyInfo, bool> _convention;

        private readonly List<string> domains = new List<string>
        {
            "@gmail.com",
            "@outlook.com",
            "@yahoo.com",
            "@aol.com"
        };

        public EmailAddressBlueprint() : this(x => x.Name.Equals("Email") ||
                                                   x.Name.Equals("EmailAddress")) { }

        public EmailAddressBlueprint(Func<PropertyInfo, bool> convention)
        {
            _convention = convention;
        }

        public bool Matches(ConstruktionContext context)
        {
            return context.PropertyInfo?.PropertyType == typeof(string) && _convention(context.PropertyInfo);
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            var userName = Guid.NewGuid().ToString("N");
            var domain = domains[random.Next(domains.Count - 1)];

            return userName + domain;
        }
    }
}