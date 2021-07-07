# Module 3. |> Step 2. 실패 처리(CascadeMode) 이해하기

## 목차
- 목표
- CascadeMode 이해
- CascadeMode 적용 수준 이해

## 목표
- CascadeMode 이해와 Validation 실패 정보 관계 이해
  - N개 : 모든 Validation 실행, `CascadeMode.Continue`
  - 1개 : 첫 실패까지 Validation 실행, `CascadeMode.Stop`

## CascadeMode 이해
- `Continue` : When a rule fails, execution continues to the next rule. 기본값
- `Stop` : When a rule fails, validation is immediately halted.

## CascadeMode 적용 수준 이해
- `Rule Chain` 영역 : 특정 Rule Chain에만 적용한다.
  ```cs
  RuleFor(x => x.Phone)
    .Cascade(CascadeMode.Stop)                // Rule Chain수준
    .NotEmpty()
    .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))  // NotEmpty가 실패하면 Must는 실행하지 않는다.
    .WithMessage("The phone number is incorrect");
  ```
  - Validation 실패 정보 **N개(모든 실패)**
    ```cs
    ValidationResult result = validator.Validate(request);
    // result.Errors N개(모든 실패)
    ``` 
- `Class` 영역 : 특정 클래스에 정의된 모든 Rule Chain에 적용한다.
  ```cs
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {
    public RegisterRequestValidator()
    {
      CascadeMode = CascadeMode.Stop;         // Class 수준
    }
  }
  ```
  - Validation 실패 정보 **1개(첫 실패)**
    ```cs
    ValidationResult result = validator.Validate(request);
    // result.Errors 1개(첫 실패)
    ``` 
- `Global` 영역 : 모든 Rule Chain에 적용한다.
  ```cs
  public void ConfigureServices(IServiceCollection services)
  {
    // ...

    ValidatorOptions.Global.CascadeMode = CascadeMode.Stop; 
  }
  ```
  - Validation 실패 정보 **1개(첫 실패)**
    ```cs
    ValidationResult result = validator.Validate(request);
    // result.Errors 1개(첫 실패)
    ``` 
