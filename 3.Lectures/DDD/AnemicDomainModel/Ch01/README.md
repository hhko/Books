# 1장. Introduction

## Anemic Domain Model
### (Mutable) Anemic Domain Model
- 정의
  - `getter` & `setter` 데이터로 구성된 도메인 모델(Mutable Anemic Domain Model) + 행위만 있는(상태가 없는 : stateless) 서비스
- 문제점
  - Discoverability of operations  
    데이터와 관련 행위가 분리되어 있다(떨어져 있다).  
  - Potential duplication  
    서비스에서 행위 코드가 중복될 수 있다.  
  - Encapsulation 필요
    데이터 변경(생성 : 유효성, 생성 후 : 무결성)에 제약이 없다(제어할 수 없다).

### Immutable Anemic Domain Model(Functional Programming)
- 정의
  - `getter` 데이터로만 구성된 데이터 모델 + 행위만 있는(상태가 없는 : stateless) 서비스
  - 데이터 변경이 없기 때문에(무결성 보장) Encapsulation이 필요 없다(Functional Programming).

## 객체지향
### Encapsulation
- 정의
  - Encapsulation is an act of protecting the data integrity.
    캡슐화는 **데이터 무결성을 보호하는 행위입니다.**  
	데이터 무결성이 보장된다면 캡슐화가 필요없다(Functional Programming).
- 구현 방법 : 클래스의 클라이언트가 내부 데이터를 유효하지 않거나 일관성이 없는 상태로 설정하지 못하도록 방지하여 수행됩니다.
  - Information hding : ?데이터 변경을 위한 노출을 최소화한다.
  - Bundling data and operations together : 더 적은 수의 개념으로 처리한다.
- Encapsulation 없을 때 문제점
  - it is hard to maintain code correctness(정확성).
