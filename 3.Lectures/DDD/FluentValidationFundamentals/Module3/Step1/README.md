# Module 3. |> Step 1. Rule 실행 조건(When) 지정하기

## 목차
- 목표
- Rule 실행 조건(When)
- 내장 함수

## 목표
- 전체 Rule vs. 일부 Rule 실행 조건 이해
  - 전체 Rule : `.When(x => ..., ApplyConditionTo.AllValidators)`
  - 일부 Rule : `.When(x => ..., ApplyConditionTo.CurrentValidator)`
- 정규식 내장 함수 : `.Matches`

## Rule 실행 조건(When)
- **전체 Rule** 실행 조건(When) : `.When(x => x.Phone != null, ApplyConditionTo.AllValidators)`
  ```cs
  RuleFor(x => x.Phone)
    .NotEmpty()
    .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))
    .When(x => x.Phone != null)                                     // 전체 Rule(NotEmpty, Must) 실행 조건이다.
    .WithMessage("The phone number is incorrect");
  ```

- **일부 Rule** 실행 조건(When) : `.When(x => x.Phone != null, ApplyConditionTo.CurrentValidator)`
  ```cs
  RuleFor(x => x.Phone)
    .NotEmpty()
    .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))
    .When(x => x.Phone != null, ApplyConditionTo.CurrentValidator)  // Must Rule만 실행 조건이다.
    .WithMessage("The phone number is incorrect");
  ```
  - 예제
    ```cs
    RuleFor(customer => customer.Photo)
        .NotEmpty()
        .Matches("https://wwww.photos.io/\d+\.png")
        .When(customer => customer.IsPreferredCustomer, ApplyConditionTo.CurrentValidator)
        .Empty()
        .When(customer => !customer.IsPreferredCustomer, ApplyConditionTo.CurrentValidator);
    ```
    - 첫번째 `.When` : `.Matches` Rule 실행 조건
    - 두번째 `.When` : `.Empty` Rule 실행 조건

## 내장 함수
- 정규식 내장 함수(Matches)
  ```cs
  RuleFor(x => x.Phone)
    .NotEmpty()
    .Matches("^[2-9][0-9]{9}")
    .WithMessage("The phone number is incorrect");
  ```