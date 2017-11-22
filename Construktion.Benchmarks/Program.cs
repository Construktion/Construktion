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
		private static readonly Construktion construktion = new Construktion();

		[Benchmark]
		public Foo One() => construktion.Construct<Foo>();

		[Benchmark]
		public List<Foo> FiveThousand()
		{
			return createFoo().ToList();

			IEnumerable<Foo> createFoo()
			{
				for (var i = 1; i <= 5000; i++)
					yield return construktion.Construct<Foo>();
			}
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