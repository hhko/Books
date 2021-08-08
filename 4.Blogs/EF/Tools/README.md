# EF Tools

## dotnet ef
- 패키지 참저  
  ```
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```
- 도구 설치
  ```
  dotnet tool install --global dotnet-ef
  dotnet tool update --global dotnet-ef
  dotnet ef
  ```
- 도구 실행
  ```
  dotnet ef dbcontext scaffold
  ```
  - DbContext데이터베이스에 대 한 및 엔터티 형식에 대 한 코드를 생성 합니다. 이 명령이 엔터티 형식을 생성 하려면 데이터베이스 테이블에 기본 키가 있어야 합니다.  
    <img src="./dotnet_ef_dbcontext_scaffold.png" width="80%"/>
- 참고 사이트
  - [Entity Framework Core 도구 참조-.NET Core CLI](https://docs.microsoft.com/ko-kr/ef/core/cli/dotnet)
  - [리버스 엔지니어링](https://docs.microsoft.com/ko-kr/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli)
  - [Entity Framework Database First .Net5 (Easy Setup and Usage)](https://www.youtube.com/watch?v=ByWkSHBwnyo)    