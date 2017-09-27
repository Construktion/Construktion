using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Construktion.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MyBenchmark>();

            Console.ReadLine();
        }
    }

    public class MyBenchmark
    {
        private readonly Construktion construktion = new Construktion();

        [Benchmark]
        public List<Foo> TenThousandFoos()
        {
            return construktion.ConstructMany<Foo>(10000).ToList();
        }
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