# [Domain-Driven Design in Practice](https://app.pluralsight.com/library/courses/domain-driven-design-in-practice/table-of-contents?aid=7010a000002LUv7AAG)

## 목차
- [1장. DDD Introduction](./Ch01/README.md)
- [2장. Value Object vs. Entity](./Ch02/README.md)
- [3장. UI & Persistence Layer](./Ch03/README.md)
- [4장. Aggregate](./Ch04/README.md)
- [5장. Repository](./Ch05/README.md)
- [6장. Bounded Context](./Ch06/README.md)

## 요구사항
- SnackMachine | 스낵머신에 돈을 투입한다.
- SnackMachine | 스낵머신에서 스낵을 구매한다.
- SnackMachine | 스낵머신에서 돈을 반환한다.
- SnackMachine | 스낵머신 Slot은 최대 3개이다(Slot 단위로 스낵이 위치한다).  
  - 3 slots of snacks
- SnackMachine | 구매할 수 있는 Snack이 있어야 한다.  
  - Check if the slot isn’t empty.
  - Snack pile is not empty.
- SnackMachine | 스낵 구매를 위한 충분한 돈이 투입되어야 한다.  
  - Check if inserted money is enough.
  - Inserted money is sufficient.
- SnackMachine | 스낵을 구매한 후 잔액을 반환할 수 있다.  
  - Return the change
- SnackMachine | 충분한 잔액이 있어야 한다. 
  - Check if there’s enough change.
  - The amount of money inside is sufficient to return the change.
- ATM | 요청한 현금을 제공한다.
  - Dispense cash
- ATM | 수수료 1%을 부과한다.
  - Charge the user's bank card
- ATM | 청구된 모든 금액을 추적한다.
  - Keep track of all money charged

