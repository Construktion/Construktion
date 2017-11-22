``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.64)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531253 Hz, Resolution=395.0613 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
|       Method |         Mean |       Error |      StdDev |     Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
|------------- |-------------:|------------:|------------:|----------:|---------:|---------:|------------:|
|          One |     13.08 us |   0.1626 us |   0.1358 us |    2.0294 |        - |        - |     6.58 KB |
| FiveThousand | 70,885.95 us | 686.9747 us | 608.9851 us | 8233.7963 | 351.8519 | 113.4259 | 32135.66 KB |
