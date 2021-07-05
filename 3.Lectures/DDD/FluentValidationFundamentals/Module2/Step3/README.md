# Module 2. |> Step 3. Collection Properties Validation 이해하기

## 목차
- ...

## 목표
- RuleForEach
  - Inline nested rules(ChildRules) vs. Separate Validator(SetValidator)

## 배열 객체 Validation
```cs
RuleFor(x => x.Addresses)
  .NotNull()
  .Must(x => x.Length >=1 && x.Length <= 3)
  .WithMessage("The number of addresses must be between 1 and 3");
```

## 배열 개별 객체 Validation
```cs
RuleForEach(x => x.Addresses)
  .SetValidator(new AddressVilidator());

public class AddressVilidator : AbstractValidator<AddressDto>
{
  // ...
}
```

- TODO 23분 동영상