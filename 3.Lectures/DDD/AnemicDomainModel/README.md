# Refactoring from Anemic Domain Model Towards a Rich One

## 목차
1. [Introduction](./Ch01/README.md)
   - Encapsulation 정의
   - Encapsulation 구현 방법
   - (Mutable) Anemic Domain Model vs. Immutable Anemic Domain Model 차이점
1. [Introducing an Anemic Domain Model](./Ch02/README.md)
   - 예제 프로그램 Domain Model 소개
1. [Decoupling the Domain Model from Data Contracts](./Ch03/README.md)
   - Data Contracts 정의
   - Data Contracts 필요성(기대 효과)
   - Data Contracts 구현 방법(DTO : Data Transfer Objects)
1. [Using Value Objects as Domain Model Building Blocks](./Ch04/README.md)
   - Value Object 정의
   - Value Object 필요성(기대 효과)
   - Value Object 구현 방법
1. [Pushing Logic Down from Services to Domain Classes](./Ch05/README.md)
   - Domain Model(Value Object & Entity) 리팩토링 목표
   - Service 리팩토링 목표
1. [Organizing the Application Services Layer](./Ch06/README.md)
   - Application Service 정의
   - Repositories와 Unit of Work 책임 정의
   - 예외 처리
   - 도메인 중심 솔루션 탐색기 구조화
1. [Domain Modeling Best Practices](./Ch07/README.md)
   - YAGNI
   - Coupling vs Code Duplication
   - Domain Model Encapsulation 
   - Domain Model Isolation vs. Domain Model Encapsulation 

## 참고 사이트
- [ ] [How to know if your Domain model is properly isolated?](https://enterprisecraftsmanship.com/posts/how-to-know-if-your-domain-model-is-properly-isolated/)
- [ ] [Domain model isolation](https://enterprisecraftsmanship.com/posts/domain-model-isolation/)
- [ ] [AnemicDomainModel Github](https://github.com/vkhorikov/AnemicDomainModel)
- [ ] [C# code contracts vs input validation](https://enterprisecraftsmanship.com/posts/code-contracts-vs-input-validation/)
- [ ] [Validation and DDD](https://enterprisecraftsmanship.com/posts/validation-and-ddd/)
- [ ] [Combining ASP.NET Core validation attributes with Value Objects](https://enterprisecraftsmanship.com/posts/combining-asp-net-core-attributes-with-value-objects/)