# Module 2. Fluent Validation 패키지 이해

## 목차
- ...

## 목표
- ...

- Validates request data, not the domain class
  - Data Contract Validation vs. DomainModel Validation
    - Data Contract : public interface
    - DomainModel : Modeling the problem domain
  - Domain class ≠ Data contracts
    - Always keep the domain model separate from data contracts
- Register **Request** Validator vs. Register Validator
  - Register **Request** Validator : Registration input data
  - Register Validator : Registration functionality, Unit tests

## Data Contract vs. Domain Model
- DTO : Data Contract
  - 인터페이스

## Fluent Validation
- Simple properties
- Comple properties 
  - Inline validation rules : x(Code duplication, Verboseness)
  - Separate validator :  
- Collection
- ~~Inheritance~~ -> DomainModel
  - Setting up rules polymorphically : o
  - Only applicable to domain class : x
- ~~Rule seet(CRUD-based)~~ -> Task-based
  - Reusing validators : o
  - Don't reuese data contracts : x
- Throw exception
  - Validations ≠ Exceptional situation