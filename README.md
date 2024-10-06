# Crypto Price Fetcher

A .net core solution that pulls crypto symbols and their prices and saves them.

<p/>

## Objectives

Build a console application that will...
- Make an web API call to get a list of Crypto Currency symbols.
- Make another web API call to get the prices of each Crypto Currency.
- Save the Crypto Currency, Price, and Date / time it was pulled.

[Done: 10-03-2024]Stage 1: Everything will be hardcoded and will follow a bad monolithic design.  
Stage 2: Create a web API project and add a workservice that will perform the task. Move configurable items into the config file. Implement logging with SeriLog.
Stage 3: Implement the Clean architecture.  
Stage 4: Add a front end web page to show the Crypto Currency and their current price.  
Stage 5: Update the page so that it automatically updates as soon as prices are refreshed.  
Stage 6: Add a graph that shows the trend of each Crypto Currency.

<p/>

## Repository

The source code can be found on [Github](https://github.com/mrametharam/crypto-price-fetcher). The different stages have been broken down into branches.

<p/>

## API Ninja

The crypto symbols and prices will be pulled from [API Ninja](https://api-ninjas.com) using the following API endpoints:

**GET** CryptoSymbols - https://api.api-ninjas.com/v1/cryptosymbols

This will be used to get all available crypto symbols.


**GET** CryptoPrice - https://api.api-ninjas.com/v1/cryptoprice?symbol={crypto-symbol}

This will be used to get the price of each crypto symbol.

<p/>

## Stage 01

Build a console application that will make an API call to get a list of crypto symbols. It will then get the price of each crypto symbol and save them. Everything will be hardcoded; no configuration file will be used. Code has been refactored and broken into smaller functions.

<p/>

## References

- [Github: Serilog](https://github.com/serilog/serilog)
- [C# Logging with Serilog and Seq - Structured Logging Made Easy](https://www.youtube.com/watch?v=_iryZxv8Rxw)
- [7 Serilog Best Practices for Better Structured Logging](https://www.youtube.com/watch?v=w7yDuoCLVvQ)
- [Structured Logging Using Serilog and Seq in .NET](https://www.youtube.com/watch?v=mT8ZkXafuZk)
- [How Structured Logging With Serilog Can Make Your Life Easier](https://www.youtube.com/watch?v=nVAkSBpsuTk)
- [ASP.NET Core Web API .NET 8 2024 - 10. Repository Pattern + DI, EF](https://www.youtube.com/watch?v=6vsONJla1Fk)
- [CRUD Operations using ADO.NET with SQL Server](https://www.youtube.com/watch?v=MCSN7ghS0vI)
- [Inserting 10 Million Records in SQL Server with C# and ADO.NET (Efficient way)](https://www.youtube.com/watch?v=khdR_8r9YQU)
- [ADO.NET Tutorial for Beginners](https://www.youtube.com/watch?v=aoFDyt8oG0k&list=PL6n9fhu94yhX5dzHunAI2t4kE0kOuv4D7)
- [Microsoft SQL Server Database Project in Visual Studio 2022 (Getting Started)](https://www.youtube.com/watch?v=5nTlqgQLkIc);
- [Stopwatch and Benchmark](https://www.youtube.com/watch?v=NTz99yN2urc)
- [C# Yield - Creating Iterators for beginners](https://www.youtube.com/watch?v=uv74SZ5MX5Q)
- [How to CALL POST API in C#! - THIS EASY!](https://www.youtube.com/watch?v=ufHlJLPK5CA)

<p/>

## Other References

- [ASP.NET Core fundamentals overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-8.0&tabs=windows)
- [ASP.NET Core Blazor fundamentals](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/?view=aspnetcore-8.0)
