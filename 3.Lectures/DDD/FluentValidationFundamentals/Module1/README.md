# Module 1. 예제 프로그램 소개

## 목차
- Validation 설계 필요성
- 실행 정보
- API
- 폴더 구성
- VS Code 명령어

## Validation 설계 필요성
- Validation 기본 코드
  ```cs
  [HttpPost]
  public IActionResult Register([FromBody] RegisterRequest request)
  {
      //
	  // Valiation 코드
	  //
      if (request == null)
          return BadRequest("Request cannot be null");

      if (string.IsNullOrWhiteSpace(request.Name))
          return BadRequest("Name cannot be empty");
      if (request.Name.Length > 200)
          return BadRequest("Name is too long");

      if (string.IsNullOrWhiteSpace(request.Email))
          return BadRequest("Email cannot be empty");
      if (request.Email.Length > 150)
          return BadRequest("Email is too long");
      if (!Regex.IsMatch(request.Email, @"^(.+)@(.+)$"))
          return BadRequest("Email is invalid");
      // Email should be unique.

      if (string.IsNullOrWhiteSpace(request.Address))
          return BadRequest("Address cannot be empty");
      if (request.Address.Length > 150)
          return BadRequest("Address is too long");

      // Return a list of errors, not just the first one


      //
	  // Production 코드
	  //
      var student = new Student(request.Email, request.Name, request.Address);
      _studentRepository.Save(student);

      var response = new RegisterResponse
      {
          Id = student.Id
      };
      return Ok(response);
  }
  ```
  - Valiation 코드량이 Production 코드량 보다 많다.
  - 모든 Validation 실패 목록을 전달할 수도 있어야 한다(지금은 Valiation 첫 실패 정보다 먼달한다).
  - Biz. Valiation과 구분할 수 있어야 한다.
    ```cs
	// Biz. 규칙
    if (_enrollments.Count >= 2)
        throw new Exception("Cannot have more than 2 enrollments");
    
    if (_enrollments.Any(x => x.Course == course))
        throw new Exception($"Student '{Name}' already enrolled into course '{course.Name}'");
	```

## 실행 정보
- localhost:5000
- http://localhost:5000/swagger

## API
- 학생 조회 : `GET ​/api​/students​/{id}`
  ```json
  {
      "name": "Alice Alison",
      "email": "alice@gmail.com",
      "address": "1234 Main St, Arlington, VA, 22201",
      "enrollments": [
          {
              "course": "Calculus",
              "grade": "A"
          }
      ]
  }
  ```
- 학생 추가 : `POST ​/api​/students`
  ```json
  {
      "email": "carl@gmail.com",
      "name": "Carl Carlson",
      "address": "3456 3rd St, Carlington, VA, 22203"
  }
  ```
- 학생 변경 : `PUT ​/api​/students​/{id}`
  ```json
  {
      "name": "Carl Carlson Jr",
      "address": "3456 3rd St, Carlington, VA, 22203"
  }
  ```

- 학생 변경(등록) : `POST ​/api​/students​/{id}​/enrollments`
  ```json
  {
      "enrollments": [
          {
              "course": "Calculus",
              "grade": "A"
          },
          {
              "course": "Literature",
              "grade": "B"
          }
      ]
  }
  ```
  
## 폴더 구성
- 주요 폴더 구성
  ```
  Module1
    └ .sln
    └ README.md
    └ doc                  // 문서
    └ src
       └ Api               // WebApi 프로젝트
       └ DomainModel       // ClassLib 프로젝트
  ```

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
  