namespace Construktion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ConstruktionContainer
    {
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Func<object>> _ctorCache = new Dictionary<Type, Func<object>>();

        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            if (!_typeMap.ContainsKey(typeof(TContract)))
                _typeMap[typeof(TContract)] = typeof(TImplementation);
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        public object GetInstance(Type request)
        {
            if (!_typeMap.ContainsKey(request) && request.GetTypeInfo().IsInterface)
            {
                throw new Exception($"Cannot resolve {request.FullName}. No registered instance could be found");
            }

            return ResolveInstance(request);
        }

        private object ResolveInstance(Type request)
        {
            if (_ctorCache.ContainsKey(request))
            {
                return _ctorCache[request]();
            }

            CreateCtor(request);

            return _ctorCache[request]();
        }

        private void CreateCtor(Type request)
        {
            if (!_typeMap.ContainsKey(request) && request.HasDefaultCtor())
            {
                var defCtor = Expression.Lambda<Func<object>>(Expression.New(request)).Compile();

                _ctorCache.Add(request, defCtor);
                return;
            }

            var implementation = _typeMap.ContainsKey(request) ? _typeMap[request] : request;

            var greedyCtor = implementation.GetTypeInfo()
                .DeclaredConstructors
                .ToList()
                .Greediest();

            var @params = new List<ConstantExpression>();
            foreach (var parameter in greedyCtor.GetParameters())
            {
                var ctorArg = parameter.ParameterType;

                if (!_typeMap.ContainsKey(ctorArg) && ctorArg.GetTypeInfo().IsInterface)
                {
                    throw new Exception($"Couldn't instantiate {implementation.FullName} " +
                                        $"No registered instance could be found for ctor arg {ctorArg.FullName}.");
                }

                var value = ResolveInstance(ctorArg);

                @params.Add(Expression.Constant(value));
            }

            var ctor = Expression.Lambda<Func<object>>(Expression.New(greedyCtor, @params)).Compile();

            _ctorCache.Add(request, ctor);
        }
    }
}