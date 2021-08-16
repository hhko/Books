# 배움은 설렘이다 for developers.

## Architectural Principles
> 건축업자가 프로그래머의 프로그램 작성 방식에 따라 건물을 짓는다면 **가장 먼저 도착하는 딱따구리가 문명을 파괴할 것입니다.**  
>
> If builders built buildings the way programmers wrote programs, **then the first woodpecker that came along would destroy civilization.**  
>
> &nbsp; - Gerald Weinberg

![](./ArchitecturalPrinciples.png)
- Link : [한국어](https://docs.microsoft.com/ko-kr/dotnet/architecture/modern-web-apps-azure/architectural-principles), [English](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)

<br/>

## Contents
1. Tutorials
   - [OpenSearch](./1.Tutorials/OpenSearch)
   - Observability
     - [NLog](./1.Tutorials/Observability/NLog)
     - [Serilog](./1.Tutorials/Observability/Serilog)
       - [Serilog Workshop](./1.Tutorials/Observability/SerilogWorkshop)
   - Clean Architecture
     - [Microsoft.Extensions.DependencyInjection](./1.Tutorials/CleanArchitecture/ServiceCollection)
     - [MediatR](./1.Tutorials/CleanArchitecture/MediatR)
     - FluentValidation
     - Polly
1. Books
   - Domain-Driven Design
     - [ ] [도메인 주도 설계 철저 입문](./2.Books/DDD/DDDGuide) 
   - Functional Programming
     - [ ] [Functional Programming in C#](./2.Books/FP/FPinCSharp)
   - Microservice
     - [ ] [Practical Microservices with Dapr and .NET](./2.Books/Microservice/DaprDotNet)
1. Lectures
   - Domain-Driven Design
     - [x] **[Refactoring from Anemic Domain Model Towards a Rich One](./3.Lectures/DDD/AnemicDomainModel/README.md)**
     - [ ] **[Domain-Driven Design in Practice](./3.Lectures/DDD/DddInPractice/README.md)**
     - [ ] **[Domain-Driven Design Fundamentals](/3.Lectures/DDD/DddFundamentals/README.md)**
     - [ ] **[FluentValidation Fundamentals](./3.Lectures/DDD/FluentValidationFundamentals/README.md)**
     - [ ] DDD and EF Core Preserving Encapsulation
     - [ ] Domain-Driven Design Fundamentals
     - [ ] Domain-Driven Design Working with Legacy Projects
     - [ ] Specification Pattern in C#
     - [ ] CQRS in Practice
     - [ ] Clean Architecture Patterns, Practices, and Principles
     - [ ] Creating N-Tier Applications in C#, Part 1
     - [ ] Creating N-Tier Applications in C#, Part 2
   - Functional Programming
     - [ ] [Defensive Programming](./3.Lectures/FP/DefensiveProgramming)
     - [ ] [Writing Purely Functional Code In C#](./3.Lectures/FP/WritingPurelyFunctionalCodeInCSharp)
     - [ ] Making Your C# Code More Functional
     - [ ] Writing Highly Maintainable Unit Tests
     - [ ] Making Your C# Code More Object-oriented
     - [ ] Tactical Design Patterns in .NET: Creating Objects
     - [ ] Tactical Design Patterns in .NET: Managing Responsibilities
     - [ ] Tactical Design Patterns in .NET: Control Flow
1. Blogs
   - Domain-Driven Design
     - [ ] [Designing with Types](./4.Blogs/DDD/DesigningWithTypes_2)
   - EF
     - [ ] [EF Tools](./4.Blogs/EF/Tools/README.md)
   - Object-Oriented Design
     - [ ] [.NET Core Console App with Dependency Injection, Logging, and Settings](./4.Blogs/OOD/BetterConsoleApp/ConsoleUI)
   - Functional Programming
     - [ ] [How To Debug LINQ Queries in C#](./4.Blogs/FP/HowToDebugLINQQueriesInCSharp)
   - Refactoring
     - [ ] [Refactoring to Aggregate Services](./4.Blogs/Refactoring/RefactoringToAggregateServices)
   - Unit Test
     - [ ] [How to Use and Unit Test ILogger](./4.Blogs/UnitTest/HowToUseAndUnitTestILogger)
   - Awesome
     - [Domain-Driven Design](./4.Blogs/Awesome/DDD)
     - [Functional Programming](./4.Blogs/Awesome/FP)
1. TODOs
   - [ ] [Adding distributed tracing instrumentation](https://docs.microsoft.com/ko-kr/dotnet/core/diagnostics/distributed-tracing-instrumentation-walkthroughs)
   - [ ] [https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices](https://docs.microsoft.com/ko-kr/dotnet/core/testing/unit-testing-best-practices)
   - [ ] [Docker Series](https://code-maze.com/docker-series/)
   - [ ] CDC(Change Data Capture) Platform
     - [CDC & CDC Sink Platform 개발 1편 - CDC Platform 개발](https://hyperconnect.github.io/2021/01/11/cdc-platform.html)
     - [CDC & CDC Sink Platform 개발 2편 - CDC Sink Platform 개발 및 CQRS 패턴의 적용](https://hyperconnect.github.io/2021/03/22/cdc-sink-platform.html)
     - [CDC & CDC Sink Platform 개발 3편 - CDC Event Application Consuming 및 Event Stream Join의 구현](https://hyperconnect.github.io/2021/06/21/cdc-event-application-consuming.html)
     - [Debezium](https://debezium.io/)
     - [Stream your data changes in MySQL into ElasticSearch using Debezium, Kafka, and Confluent JDBC Sink Connector](https://towardsdatascience.com/stream-your-data-changes-in-mysql-into-elasticsearch-using-debizium-kafka-and-confluent-jdbc-b93821d4997b)
     - [Debezium에 대한 시각적 소개](https://ichi.pro/ko/debezium-e-daehan-sigagjeog-sogae-55345991108280)
     - [이벤트 기반 변경 데이터 캡처에 대한 간단한 소개](https://ichi.pro/ko/ibenteu-giban-byeongyeong-deiteo-kaebcheoe-daehan-gandanhan-sogae-114566497460123)
   - [ ] [Elastic Stack으로 코로나19 대시보드 만들기 - 1부 : 파이프라인 구성](https://taetaetae.github.io/posts/make-dashboards-from-elasticstack-1/)
   - [ ] [Elastic Stack으로 코로나19 대시보드 만들기 - 2부 : 대시보드](https://taetaetae.github.io/posts/make-dashboards-from-elasticstack-2/)   