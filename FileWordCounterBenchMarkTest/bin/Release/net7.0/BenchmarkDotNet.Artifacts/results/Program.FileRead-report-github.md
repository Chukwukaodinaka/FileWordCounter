``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2846/21H2/November2021Update)
Intel Core i7-7600U CPU 2.80GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|                              Method |        Mean |       Error |      StdDev |
|------------------------------------ |------------:|------------:|------------:|
| GetEachWordInContentUsingReadAsText | 99,872.9 ns | 1,325.60 ns | 1,175.11 ns |
|  GetEachWordInContentUsingReadToEnd |    268.9 ns |     5.33 ns |     8.13 ns |
