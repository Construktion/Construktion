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
        private readonly Dictionary<Type, Func<object>> _ctors = new Dictionary<Type, Func<object>>();

        public void Register<TContract, TImplementation>() where TImplementation : TContract
        {
            if (!_typeMap.ContainsKey(typeof(TContract)))
                _typeMap[typeof(TContract)] = typeof(TImplementation);
        }

        public T GetInstance<T>()
        {
            var type = typeof(T);
            if (!_typeMap.ContainsKey(type) && type.GetTypeInfo().IsInterface)
            {
                throw new Exception($"Cannot resolve {type.Name}. No registered instance could be found");
            }

            return (T) ResolveInstance(type);
        }

        private object ResolveInstance(Type type)
        {
            if (_ctors.ContainsKey(type))
            {
                return _ctors[type]();
            }

            AddCtor(type);

            return _ctors[type]();
        }

        private void AddCtor(Type type)
        {
            if (!_typeMap.ContainsKey(type) && type.HasDefaultCtor())
            {
                var defCtor = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();

                _ctors.Add(type, defCtor);
                return;
            }

            var imp = _typeMap.ContainsKey(type) ? _typeMap[type] : type;

            var greedyCtor = imp.GetTypeInfo()
                .DeclaredConstructors
                .ToList()
                .Greediest();

            var @params = new List<ConstantExpression>();
            foreach (var parameter in greedyCtor.GetParameters())
            {
                var ctorArg = parameter.ParameterType;

                if (!_typeMap.ContainsKey(ctorArg) && ctorArg.GetTypeInfo().IsInterface)
                {
                    throw new Exception($"Couldn't instantiate {type.FullName} " +
                                        $"No registered instance could be found for ctor arg {ctorArg.FullName}.");
                }

                var value = ResolveInstance(ctorArg);

                @params.Add(Expression.Constant(value));
            }

            var ctor = Expression.Lambda<Func<object>>(Expression.New(greedyCtor, @params)).Compile();

            _ctors.Add(type, ctor);
        }
    }
}