namespace Construktion.Samples.Fixie
{
    using System.Collections.Generic;
    using System.Reflection;
    using global::Fixie;

    public class FixieConvention : Convention
    {
        public FixieConvention()
        {
            Classes
                .Where(x => x.Name.EndsWith("Tests") && x.Namespace.StartsWith("Construktion.Samples.Fixie"));

            Methods
                .Where(x => x.IsPublic && x.IsVoid());

            Parameters
                .Add(ConstruktionParameterSource);
        }

        public IEnumerable<object[]> ConstruktionParameterSource(MethodInfo method)
        {
            var construktion = new Construktion();
            var parameters = new List<object>();

            foreach (var paramInfo in method.GetParameters())
            {
                var result = construktion.Construct(paramInfo);

                parameters.Add(result);
            }

            return new[] { parameters.ToArray() };
        }
    }
}