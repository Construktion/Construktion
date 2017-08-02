``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700HQ CPU 2.60GHz (Skylake), ProcessorCount=8
Frequency=2531247 Hz, Resolution=395.0622 ns, Timer=TSC
dotnet cli version=1.0.4
  [Host]     : .NET Core 4.6.25211.01, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.25211.01, 64bit RyuJIT


```
 |          Method |     Mean |    Error |   StdDev |
 |---------------- |---------:|---------:|---------:|
 | TenThousandFoos | 172.8 ms | 4.655 ms | 13.20 ms |
