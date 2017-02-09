namespace Construktion.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    //https://github.com/AutoFixture/AutoFixture/blob/master/Src/AutoFixture/RandomNumericSequenceGenerator.cs
    public class NumericBuilder : Builder
    {
        private readonly long[] limits;
        private readonly object syncRoot = new object();
        private readonly Random _random = new Random();
        private readonly HashSet<long> numbers = new HashSet<long>();
        private readonly IEnumerable<TypeCode> _typesHandled = new List<TypeCode>
        {
            TypeCode.Byte,
            TypeCode.Decimal,
            TypeCode.Double,
            TypeCode.Int16,
            TypeCode.Int32,
            TypeCode.Int64,
            TypeCode.SByte,
            TypeCode.Single,
            TypeCode.UInt16,
            TypeCode.UInt32,
            TypeCode.UInt64
        };

        private long lower;
        private long upper;
        private long count;

        public NumericBuilder() 
            : this(1, byte.MaxValue, short.MaxValue, int.MaxValue)
        {
        }

        public NumericBuilder(IEnumerable<long> limits)
             : this(limits.ToArray()) 
         { 
         }

        public NumericBuilder(params long[] limits)
        {
            if (limits == null)
                throw new ArgumentNullException(nameof(limits));

            if (limits.Length < 2)
                throw new ArgumentException("Limits must be at least two ascending numbers.", nameof(limits));

            AssertLimitsAreAscending(limits);

            this.limits = limits;
            CreateRange();
        }

        public bool Matches(ConstruktionContext context)
        {
            return _typesHandled.Contains(Type.GetTypeCode(context.RequestType)) && !context.RequestType.GetTypeInfo().IsEnum;
        }

        public object Build(ConstruktionContext context, ConstruktionPipeline pipeline)
        {
            return CreateRandom(context.RequestType);
        }

        private static void AssertLimitsAreAscending(long[] limits)
        {
            if (limits.Zip(limits.Skip(1), (a, b) => a >= b).Any(b => b))
                throw new ArgumentOutOfRangeException(nameof(limits), "Limits must be ascending numbers.");
        }

        private object CreateRandom(Type request)
        {
            switch (Type.GetTypeCode(request))
            {
                case TypeCode.Byte:
                    return (byte)
                        GetNextRandom();

                case TypeCode.Decimal:
                    return (decimal)
                        GetNextRandom();

                case TypeCode.Double:
                    return (double)
                        GetNextRandom();

                case TypeCode.Int16:
                    return (short)
                        GetNextRandom();

                case TypeCode.Int32:
                    return (int)
                        GetNextRandom();

                case TypeCode.Int64:
                    return
                        GetNextRandom();

                case TypeCode.SByte:
                    return (sbyte)
                        GetNextRandom();

                case TypeCode.Single:
                    return (float)
                        GetNextRandom();

                case TypeCode.UInt16:
                    return (ushort)
                        GetNextRandom();

                case TypeCode.UInt32:
                    return (uint)
                        GetNextRandom();

                case TypeCode.UInt64:
                    return (ulong)
                        GetNextRandom();

                default:
                    throw new InvalidOperationException($"Numeric Builder cannot handle the request of type {request}");
            }
        }

        private long GetNextRandom()
        {
            lock (syncRoot)
            {
                EvaluateRange();

                long result;
                do
                {
                    if (lower >= int.MinValue &&
                        upper <= int.MaxValue)
                    {
                        result = _random.Next((int)lower, (int)upper);
                    }
                    else
                    {
                        result = GetNextInt64InRange();
                    }
                }
                while (numbers.Contains(result));

                numbers.Add(result);
                return result;
            }
        }

        private void EvaluateRange()
        {
            if (count == (upper - lower))
            {
                count = 0;
                CreateRange();
            }

            count++;
        }

        private void CreateRange()
        {
            var remaining = limits.Where(x => x > upper - 1).ToArray();
            if (remaining.Any() && numbers.Any())
            {
                lower = upper;
                upper = remaining.Min() + 1;
            }
            else
            {
                lower = limits[0];
                upper = GetUpperRangeFromLimits();
            }

            numbers.Clear();
        }

        private long GetUpperRangeFromLimits()
        {
            return limits[1] >= int.MaxValue
                    ? limits[1]
                    : limits[1] + 1;
        }

        private long GetNextInt64InRange()
        {
            var range = (ulong)(upper - lower);
            var limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong number;

            do
            {
                var buffer = new byte[sizeof(ulong)];
                _random.NextBytes(buffer);
                number = BitConverter.ToUInt64(buffer, 0);
            }
            while (number > limit);

            return (long)(number % range + (ulong)lower);
        }
    }
}