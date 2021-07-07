# FluentValidation Fundamentals
- [Fluentvalidation fundamentals 강의](https://www.pluralsight.com/courses/fluentvalidation-fundamentals)
- [Fluentvalidation fundamentals 강의 소스](https://github.com/vkhorikov/ValidationInDDD)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/installation.html)
- [OSS Power-Ups: FluentValidation](https://www.youtube.com/watch?v=ePrWPblO-H4)

## 목차
- Module 1
  - [Step 1 : 예제 프로그램 이해하기](./Module1/Step1)
- Module 2
  - [Step 1 : Simple Properties Validation](./Module2/Step1)
    - `FluentValidation `
  - [Step 2 : Complex Properties Validation](./Module2/Step2)
    - `Data Contract vs. Domain Model`
  - [Step 3 : Collection Properties Validation](./Module2/Step3)
  
## Fluent Validation
- Simple properties : Inline validation rules
- Complex properties : Inline validation rules vs. Separate validator
  - Inline validation rules : 비 추천(Code duplication, Verboseness)
  - Separate validator : 추천
- Collection
  - Inline nested rules(ChildRules) : 비 추천
  - Separate Validator(SetValidator)
    - 배열 : `public class AddressesVilidator : AbstractValidator<AddressDto[]>`
    - 배열 값 : `public class AddressVilidator : AbstractValidator<AddressDto>`
- ~~Inheritance(SetInheritanceValidator : Data Contract에서 존재 불가능한 개념)~~ → Only DomainModel에서 사용 
  - Data Contract에서는 상속 개념이 존재하지 않는다.
  - 상속 개념의 Validation은 DomainModel에서만 존재할 수 있다.
    - Setting up rules polymorphically
    - Only applicable to domain classes
- ~~Rule seet(RuleSet : CRUD-based)~~ → Task-based
  - validator와 Data Contract 재사용은 CRUD-based이다. Task-based 방향으로 설계해야 한다.
    - Reusing validators : Validation 재사용 비 추천
    - Don't reuese data contracts : Data Contract은 재사용 하지 않는다.
- ~~Throw exception~~
  - Validations ≠ Exceptional situation
  - Don't use exceptions for validation
  - 예외는 복구할 수 없는 하드웨어 오류와 같은 예외적인 상황에서만 사용한다.

```
## 주요 학습
## Data Contract vs. Domain Model
- Validates request data, not the domain class
  - Data Contract Validation vs. DomainModel Validation
    - Data Contract : public interface
    - DomainModel : Modeling the problem domain
  - Domain class ≠ Data contracts
    - Always keep the domain model separate from data contracts
- Register **Request** Validator vs. Register Validator
  - Register **Request** Validator : Registration input data
  - Register Validator : Registration functionality, Unit tests
```

## VS Code 명령어
- dotnet 
  - `new` 솔루션/프로젝트
    - 솔루션 `add/remove` 프로젝트
      - `add/remove` 프로젝트 reference/package 프로젝트/패키지
- 솔루션 
  - `dotnet new sln -n [솔루션명]`
  - `dotnet sln [솔루션명].sln add [프로젝트명(상대경로)].csproj`
  - `dotnet sln [솔루션명].sln remove [프로젝트명(상대경로)].csproj`
  - `dotnet sln [솔루션명].sln list`
- 프로젝트 : reference/package
  - `dotnet new classlib -n [프로젝트명] -o [프로젝트명]` : -o 생략 가능(-n와 동일)
  - `dotnet add [프로젝트명].csproj reference [프로젝트명(상대경로)].csproj` : 프로젝트 경로에서는 [프로젝트명].csproj을 생략할 수 있다.
  - `dotnet remove [프로젝트명].csproj reference [프로젝트명(상대경로)].csproj`
  - `dotnet list [프로젝트명].csproj reference`
  - `dotnet add [프로젝트명].csproj package [패키지명] -v [버전]` : 프로젝트 경로에서는 [프로젝트명].csproj을 생략할 수 있다.
  - `dotnet remove [프로젝트명].csproj package [패키지명]`
  - `dotnet list [프로젝트명].csproj package`
    - `--outdated`
    - `--include-prerelease`
  - `dotnet restore`  

## 참고 자료
- [dotnet sln](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-sln)
- [dotnet new](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-new)
- [dotnet add reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-add-reference)
- [dotnet remove reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-remove-reference)
- [dotnet list reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-list-reference)
- [dotnet add package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-add-package)
- [dotnet remove package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-remove-package)
- [dotnet list package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-list-package)
- [dotnet restore](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-restore)
