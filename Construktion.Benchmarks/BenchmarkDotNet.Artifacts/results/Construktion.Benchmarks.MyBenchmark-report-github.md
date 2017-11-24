``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.64)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531251 Hz, Resolution=395.0616 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
|       Method |         Mean |       Error |      StdDev |     Gen 0 |    Gen 1 |   Gen 2 |   Allocated |
|------------- |-------------:|------------:|------------:|----------:|---------:|--------:|------------:|
|          One |     14.78 us |   0.2936 us |   0.6506 us |    1.9836 |        - |       - |     6.46 KB |
| FiveThousand | 74,058.87 us | 803.3225 us | 712.1244 us | 7941.4063 | 406.2500 | 85.9375 | 31625.65 KB |
