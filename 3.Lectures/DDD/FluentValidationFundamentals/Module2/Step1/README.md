# Module 2. |> Step 1. Simple Properties Validation 이해하기

## 목차
- FluentValidation 개요
- Simple Properties Validation

## 목표
- FluentValidation 패키지 이해
  - 개별 클래스
  - Validation 구현 장소(생성자)
- Simple Properties Validation 구현
  - RuleFor

## FluentValidation 개요
- FluentValidation 최신 패키지 설치
  ```
  dotnet add Api.proj package FluentValidation
  ```
- Validation 역할을 분리한다. : `AbstractValidator<T>` 클래스 상속
  ```cs
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {

  }
  ```
- Validation 규칙을 생성자에서 수행한다.
  ```cs
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {
      public RegisterRequestValidator()
      {
        // Validation 규칙
      }
  }
  ``` 

## Simple Properties Validation
- Validation 규칙은 `Fluent Interface Pattern(Inline)`(코드 가독성 향상)으로 구현한다. : `RuleFor`
  ```cs
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {
      public RegisterRequestValidator()
      {
          RuleFor(x => x.Name)    // Fluent Interface
              .NotEmpty()
              .Length(0, 200);
      }
  }
  ```
- NotNull vs . NotEmpty
  | 역할 | NotNull | NotEmpty |
  |---|---|---|
  | NULL | O | O |
  | Empty | X | O |

## 이해 못한 부분
- Register **Request** Validator vs. Register Validator
  - Register **Request** Validator : Registration input data
  - Register Validator : Registration functionality, Unit tests