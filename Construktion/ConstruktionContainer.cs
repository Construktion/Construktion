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

        public Config<T> For<T>()
        {
            return new Config<T>(typeof(T), this);
        }

        internal void Use(Type contract, Type imp)
        {
            _typeMap.Add(contract, imp);
        }

        public T GetInstance<T>()
        {
            var t = typeof(T);
            if (!_typeMap.ContainsKey(typeof(T)) && t.GetTypeInfo().IsInterface)
            {
                throw new Exception($"No registered instance can be found for {t.Name}");
            }

            return (T) ResolveInstance(typeof(T), new Dictionary<Type, object>());
        }

        private object ResolveInstance(Type t, Dictionary<Type, object> uow)
        {
            if (uow.ContainsKey(t))
                return uow[t];

            object instance = null;
            if (_ctors.ContainsKey(t))
            {
                instance = _ctors[t]();
                uow.Add(t, instance);

                return instance;
            }

            ResolveCtor(t, uow);

            instance = _ctors[t]();
            uow.Add(t, instance);

            return instance;
        }

        private void ResolveCtor(Type t, Dictionary<Type, object> uow)
        {
            if (!_typeMap.ContainsKey(t) && t.HasDefaultCtor())
            {
                var defCtor = Expression.Lambda<Func<object>>(Expression.New(t)).Compile();

                _ctors.Add(t, defCtor);
                return;
            }

            var imp = _typeMap.ContainsKey(t) ? _typeMap[t] : t;

            var ctors = imp.GetTypeInfo()
                .DeclaredConstructors
                .ToList();

            var max = ctors.Max(x => x.GetParameters().Length);
            var greedyCtor = ctors.First(x => x.GetParameters().Length == max);

            var args = new List<ConstantExpression>();
            foreach (var parameter in greedyCtor.GetParameters())
            {
                var type = parameter.ParameterType;

                if (!_typeMap.ContainsKey(type) && type.GetTypeInfo().IsInterface)
                {
                    throw new Exception($"Cannot instantiate {t.Name}. Ctor arg {type.Name} cannot be resolved. " +
                                        "Please register it with the container");
                }

                var value = ResolveInstance(type, uow);
                args.Add(Expression.Constant(value));
            }

            var ctor = Expression.Lambda<Func<object>>(Expression.New(greedyCtor, args)).Compile();

            _ctors.Add(t, ctor);
        }

        public class Config<TContract>
        {
            private readonly Type _contract;
            private readonly ConstruktionContainer _container;

            public Config(Type contract, ConstruktionContainer container)
            {
                _contract = contract;
                _container = container;
            }

            public void Use<TImp>() where TImp : TContract
            {
                _container.Use(_contract, typeof(TImp));
            }
        }
    }
}