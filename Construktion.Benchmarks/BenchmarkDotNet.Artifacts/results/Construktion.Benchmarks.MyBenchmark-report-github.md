``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.674)
Processor=Intel Core i7-5600U CPU 2.60GHz (Broadwell), ProcessorCount=4
Frequency=2533198 Hz, Resolution=394.7579 ns, Timer=TSC
.NET Core SDK=2.0.2
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
|          Method |          Mean |         Error |         StdDev |        Median |      Gen 0 |     Gen 1 |     Gen 2 |   Allocated |
|---------------- |--------------:|--------------:|---------------:|--------------:|-----------:|----------:|----------:|------------:|
| TenThousandFoos | 181,299.70 us | 5,876.6895 us | 16,671.1899 us | 179,794.07 us | 26151.8750 | 3382.5000 | 1489.3750 | 65572.63 KB |
|          OneFoo |      12.30 us |     0.2449 us |      0.6745 us |      12.08 us |     3.0518 |         - |         - |     6.57 KB |
