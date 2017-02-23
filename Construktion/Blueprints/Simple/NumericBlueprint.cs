namespace Construktion.Blueprints.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    //https://github.com/AutoFixture/AutoFixture/blob/master/Src/AutoFixture/RandomNumericSequenceGenerator.cs
    public class NumericBlueprint : Blueprint
    {
        private readonly long[] limits;
        private readonly object syncRoot = new object();
        private readonly Random _random = new Random();
        private readonly HashSet<long> numbers = new HashSet<long>();
        //this was Type codes but net standard doesn't have GetTypeCode on Type 
        //anymore...find out where it is
        private readonly IEnumerable<Type> _typesHandled = new List<Type>
        {
            typeof(byte),
            typeof(decimal),
            typeof(double),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(sbyte),
            typeof(Single),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64)
        };

        private long lower;
        private long upper;
        private long count;

        public NumericBlueprint()
        {
            limits = new long[] {1, byte.MaxValue, short.MaxValue, int.MaxValue};
            CreateRange();
        }

        public bool Matches(ConstruktionContext context)
        {
           return _typesHandled.Contains(context.RequestType) && !context.RequestType.GetTypeInfo().IsEnum;
        }

        public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline)
        { 
            return CreateRandom(context.RequestType);
        }

        private object CreateRandom(Type request)
        {
            if (request == typeof(byte))
                return (byte)GetNextRandom();

            else if (request == typeof(decimal))
                return (decimal)GetNextRandom();

            else if (request == typeof(double))
                return (double)GetNextRandom();

            else if (request == typeof(Int16))
                return (Int16)GetNextRandom();

            else if (request == typeof(Int32))
                return (Int32)GetNextRandom();

            else if (request == typeof(Int64))
                return (Int64)GetNextRandom();

            else if (request == typeof(sbyte))
                return (sbyte)GetNextRandom();

            else if (request == typeof(Single))
                return (Single)GetNextRandom();

            else if (request == typeof(UInt16))
                return (UInt16)GetNextRandom();

            else if (request == typeof(UInt32))
                return (UInt32)GetNextRandom();

            else if (request == typeof(UInt64))
                return (UInt64)GetNextRandom();

            else
                throw new InvalidOperationException($"Numeric Blueprint cannot handle the request of type {request}");
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