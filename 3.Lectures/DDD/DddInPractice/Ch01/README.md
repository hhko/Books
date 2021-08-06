## 1장. DDD Introduction

## 개념 정리
![](./Ch01_Summary.png)

- **DDD는 YAGNI와 KISS 원칙을 보완한다.**
  - YAGNI와 KISS 원칙이 중요한 이유?
    - 코드 복작성이 시간이 지난면 개발자가 수용할 수 있는 범위를 벗어난다.
  - 도메인의 핵심 부분을 추출하고 단순화하여 불필요한 복작성 대부분을 제거한다.
    - Focus on essential parts
    - Simplifying the problem
- **DDD는 도메인 복작성을 제어한다. ≒ 도메인 지식 표현력을 높인다.**
  - 혼동 -(회피하기 위해) → 용어(Ubiquitous language) : 도메인 전문가와 개발자가 같은 용어(≒ 클래스명)를 사용한다.
  - 관점 -(이해하기 위해) → 경계(Bounded context) : 서로 다른 관점을 이해하기 위해 경계를 나눈다.
  - 지식 -(표현하기 위해) → 핵심(Core domain) : 풀어야할 가장 가장 중요한 도메인 문제(지식)에 집중한다.
- **도메인 지식은?**
  - 표현 포함(Unit Test 포함 대상) : 순수한 도메인 지식만을 표현하기 위해 격리(Isolation) 시킨다.
    - Value objects, Entities, Domain Event, Aggregates
  - 표현 제외(Unit Test 제외 대상)
    - Repositories, Factories, Domain Services
    - Application Services
    - UI