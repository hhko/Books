# 2장. Introducing an Anemic Domain Model

## Anemic Domain Model
![](./DomainModel.png)

## 문제점
![](./DomainModel_Problems.png)
- `setter`가 노출되어 있어 **데이터 변경(무결성)을 제어할 수 없다.**
- Encapsulation이 필요하다.
  - Encapsulation is an act of protecting the data integrity.
  - 캡슐화는 **데이터 무결성을 보호하는 행위입니다.**  