# 6장. Organizing the Application Services Layer

## Application Service 정의
<img src="./ApplicationService.png.png" width="90%"/>

## Repositories and Unit of Work
<img src="./Repositories_and_UnitOfWork.png.png" width="90%"/>

- `Commit` 책임은 `Unit of Work`이다.

## Global Unhanded Exceptions
<img src="./UnhandedExceptions.png" width="90%"/>

- 출력 타입 표준화 : `Envelope`
- 예외 처리 Middleware : `ExceptionHandler`
- Unit of Work 통합 : `BaseController`

## 솔루션 탐색기 Layout
<img src="./Layout.png" width="90%"/>  

