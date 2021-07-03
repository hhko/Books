# FluentValidation Fundamentals
- [Fluentvalidation fundamentals 강의](https://www.pluralsight.com/courses/fluentvalidation-fundamentals)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/installation.html)

## 목차
- [Step 1](./Step1) : 예제 프로그램 소개
- [Step 2](./Step2) : 

## VS Code 명령어
- 솔루션 
  - `dotnet new sln -n [솔루션명]`
  - `dotnet sln [솔루션명].sln add [프로젝트명(상대경로)].csproj`
  - `dotnet sln [솔루션명].sln remove [프로젝트명(상대경로)].csproj`
  - `dotnet sln [솔루션명].sln list`
- 프로젝트 : reference/package
  - `dotnet new classlib -n [프로젝트명] -o [프로젝트명]` : -o 생략 가능(-n와 동일)
  - `dotnet add [프로젝트명].csproj reference [프로젝트명(상대경로)].csproj` : 프로젝트 경로에서는 [프로젝트명].csproj을 생략할 수 있다.
  - `dotnet remove [프로젝트명].csproj reference [프로젝트명(상대경로)].csproj`
  - `dotnet list [프로젝트명].csproj reference`
  - `dotnet add [프로젝트명].csproj package [패키지명] -v [버전]` : 프로젝트 경로에서는 [프로젝트명].csproj을 생략할 수 있다.
  - `dotnet remove [프로젝트명].csproj package [패키지명]`
  - `dotnet list [프로젝트명].csproj package`
    - `--outdated`
    - `--include-prerelease`

## 참고 자료
- [dotnet sln](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-sln)
- [dotnet new](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-new)
- [dotnet add reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-add-reference)
- [dotnet remove reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-remove-reference)
- [dotnet list reference](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-list-reference)
- [dotnet add package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-add-package)
- [dotnet remove package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-remove-package)
- [dotnet list package](https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet-list-package)