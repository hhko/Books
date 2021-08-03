# 5장. Pushing Logic Down from Services to Domain Classes

- 리팩토링 목표
  - `Mutable` Anemic Domain Model을 `Immutable` Domain Model 지향으로 개선한다.
  - 절대적으로 필요한 작업(데이터와 행위)만 명시적으로 남긴다.
    - 외부에서 데이터 변경을 최소화 시킨다.
- 리팩토링 기대 효과
  - ↑ : 도메인 표현을 높인다.
  - ↓ : 잠재적인 실수를 줄인다.
  
## 생성자 리팩토링
<img src="./Refactoring_Constructor.png" width="60%"/>

- 목표 : 데이터 ..   
- 구현 방법
  - 필수 데이터 : 생성자 매개변수
  - 기본 값 : 생성자 멤버 젼수 초기화
  
## 컬랙션 리팩토링
<img src="./Refactoring_IReadOnlyList.png" width="60%"/>

- 목표 : 
- 구현 방법  
  - 불변 컬랙션 : IReadOnlyList(vs. IEnumerable)
  - 불변 컬랙션 필드 : get
  - 가변 행위 : 명시적 메서드 정의

## 로직 리팩토링
<img src="./Refactoring_Logic.png" width="60%"/>  

- 목표 : 
- 구현 방법
  - 메서드 통합 : 매개변수 추가
  - 불변 필드 : get

## 데이터 불일치 노출 리팩토링
<img src="./Refactoring_Inconsistency.png" width="60%"/>  
- 목표 : 
- 구현 방법
