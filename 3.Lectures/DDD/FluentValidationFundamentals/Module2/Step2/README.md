# Module 2. |> Step 2. Complex Properties Validation 이해하기

## 목차
- Data Contract vs. Domain Model

## 목표
- Data Contract vs. Domain Model 차이 이해
  - 책임이 다르다(코드 중복을 허용한다).
- Complex Properties Validation 이해
  - NULL 처리
  - Sub-Validator 

## Data Contract vs. Domain Model
- `Data Contract ≠ Domain Model` 책임은 서로 다르다.  
  - **`Data Contract` : Public Interface(외부 환경 : 사용자 입력 데이터), 이전 버전과 호환성**
  - **`Domain Model` : Modeling the problem domain**  
  - 예. 코드 중복이 발생하지만 책임이 다르기 때문에 클래스를 항상 분리 시커야 한다.
    ```cs
    public class AddressDto   // Data Contract
    { 
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class Address      // Domain Model
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }

        public Address(string street, string city, string state, string zipCode)
        {
          // 멤버 변수 초기화
        }
    }
    ``` 

## Complex Properties Validation
- ~~Case 1. Inline nested rules, 버그 有 : Address가 NULL일 때~~
  ```cs
  RuleFor(x => x.Address.Street).NotEmpty().Length(0, 100);
  RuleFor(x => x.Address.City).NotEmpty().Length(0, 40);
  RuleFor(x => x.Address.State).NotEmpty().Length(0, 2);
  RuleFor(x => x.Address.ZipCode).NotEmpty().Length(0, 5);
  ```
  - FluentValidation은 `x.Address`가 `NULL`일 경우를 판단하지 않는다(NullException 예외 발생).
- ~~Case 2. Inline nested rules, 버그 無 : Address가 NULL일 때~~
  ```cs
  RuleFor(x => x.Address).NotNull();    // NULL 유효성 검사
  RuleFor(x => x.Address.Street).NotEmpty().Length(0, 100).When(x => x.Address != null);
  RuleFor(x => x.Address.City).NotEmpty().Length(0, 40).When(x => x.Address != null);
  RuleFor(x => x.Address.State).NotEmpty().Length(0, 2).When(x => x.Address != null);
  RuleFor(x => x.Address.ZipCode).NotEmpty().Length(0, 5).When(x => x.Address != null);
  ```
  - Inline nested rules 단점
    - Verboseness
      - `NotNull()`과 `NotEmpty()`은 독립적으로 수행된다(`NotNull()`이 실패되어도 `NotEmpty()`가 실행된다). 
      - `.When(x => x.Address != null);` 조건이 true일 때만 `NotEmpty()`가 수행하도록 변경해야 한다.
    - Code duplication
      - x.Address의 유효검 검사 재사용을 하기 위해서는 코드 중복이 발생한다.
- Case 3. Separate Validator(SetValidator)
  ```cs
  RuleFor(x => x.Address).NotNull().SetValidator(new AddressVilidator());

  public class AddressVilidator : AbstractValidator<AddressDto>
  {
    // ...
  }
  ```
  - `Inline nested rules` 보다 더 Clean한 코드이다.
- 구현 코드  
  <img src="./Doc/SeparateValidator.png" width="700"/>
