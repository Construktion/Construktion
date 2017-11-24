``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.64)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531251 Hz, Resolution=395.0616 ns, Timer=TSC
.NET Core SDK=2.0.0
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
|       Method |         Mean |         Error |        StdDev |     Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
|------------- |-------------:|--------------:|--------------:|----------:|---------:|---------:|------------:|
|          One |     14.27 us |     0.1870 us |     0.1562 us |    2.0294 |        - |        - |      6.6 KB |
| FiveThousand | 78,940.24 us | 1,918.9198 us | 2,426.8191 us | 8250.0000 | 375.0000 | 125.0000 | 32802.11 KB |
