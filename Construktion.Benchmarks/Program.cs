namespace Construktion.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MyBenchmark>();

            Console.ReadLine();
        }
    }

    [MemoryDiagnoser]
    public class MyBenchmark
    {
        private readonly Construktion construktion = new Construktion();

        [Benchmark]
        public List<Foo> TenThousandFoos() => construktion.ConstructMany<Foo>(10000).ToList();

        [Benchmark]
        public Foo OneFoo() => construktion.Construct<Foo>();
    }

    public class Foo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Active { get; set; }
        public Guid Guid { get; set; }
        public List<int> Ints { get; set; }
    }
}