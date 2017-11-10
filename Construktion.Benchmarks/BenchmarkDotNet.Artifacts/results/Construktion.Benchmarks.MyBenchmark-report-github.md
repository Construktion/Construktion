``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.674)
Processor=Intel Core i7-5600U CPU 2.60GHz (Broadwell), ProcessorCount=4
Frequency=2533198 Hz, Resolution=394.7579 ns, Timer=TSC
.NET Core SDK=2.0.2
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
|          Method |     Mean |    Error |   StdDev |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|---------------- |---------:|---------:|---------:|-----------:|----------:|----------:|----------:|
| TenThousandFoos | 177.7 ms | 5.422 ms | 15.82 ms | 26159.3750 | 3373.1250 | 1502.5000 |  63.89 MB |
