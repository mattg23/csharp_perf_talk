#import "@preview/typslides:1.2.5": *
// Project configuration
#show: typslides.with(
  ratio: "16-9",
  theme: "greeny",
)

#front-slide(
  title: "Faster Code; More time for ☕",
  subtitle: [_How to try to avoid writing the slowest code possible_],
  info: [
    Repo: #link("https://github.com/mattg23/csharp_perf_talk")
    ],
)

#slide[
  #image("./pics/dosomething.jpg", fit:"contain")
]

#slide[
    - Optimization vs. _not writing the slowest code possible_
        - When? From the beginning?
        - *Be aware* of the _context_ you are in!
    - Tradeoffs:
        - Dev time
        - Readability
        - [Portability] (mostly non-issue in .NET)
    - Learning by doing!
]

#slide[
    What to think about when programming?
    - Stack vs. Heap
    - Value vs. Reference Types
    - Loops vs. Linq
    - Process data while its loading (Streaming vs. Non-Streaming)
    - Parallel processing
    - Batching / Chunking
]

#slide[
  ```cs
var bytes = new byte[...some large byte array..];
var currentTS = BitConverter.ToDouble(
    bytes.Skip(SOME_INDEX).Take(TS_SIZE_IN_BYTES).Reverse().ToArray(), 0);
  ```
]

#slide[
  ```cs
ReadOnlySpan<byte> spanBytes 
    = bytes[SOME_INDEX..(SOME_INDEX + TS_SIZE_IN_BYTES)];
var currentTs = BinaryPrimitives.ReadDoubleBigEndian(spanBytes);
  ```
]

#focus-slide[
  Less slide more code pls 🥱
]

#slide[
    Take aways:
    - Measure - do not optimize blindly
    - Use the knowledge you have!
        - what do you know about the values you work with?
        - contraints of the data format you are parsing?
    - Think about bottlenecks:
        - Network, DB, SSD/HDD
        - external systems
        - cpu speed, memory throughput
    - `Span<T>` is your friend in .NET
    - Think before use:
        - Linq
        - Reflection
]

#slide[
    Other .NET Classes to checkout:
    - #link("https://learn.microsoft.com/en-us/dotnet/api/system.buffers.binary.binaryprimitives?view=net-9.0")[`BinaryPrimitives`]
    - #link("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.memorymarshal?view=net-8.0")[`MemoryMarshal`]
    - #link("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.collectionsmarshal?view=net-8.0")[`CollectionsMarshal`]
    - #link("https://learn.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1?view=net-8.0")[`ArrayPool<T>`]
    - #link("https://learn.microsoft.com/en-us/dotnet/api/system.memoryextensions?view=net-8.0")[`MemoryExtensions`]
]

#slide[
    Other Resources:
    - #link("https://people.freebsd.org/~lstewart/articles/cpumemory.pdf")[Paper: What every programmer should know about Memory]
    - #link("https://www.youtube.com/c/MollyRocket")[Casey Muratori]
    - #link("https://benchmarkdotnet.org")[BenchmarkDotNet]
    - #link("https://learn.microsoft.com/en-us/dotnet/framework/performance/performance-tips")[.NET Performance Tips]
]
