# Module 1. 예제 프로그램 소개

## 목차
- 실행 정보
- Validation 설계 필요성
- API
- 폴더 구성
- VS Code 명령어

## 실행 정보
- http://localhost:5000/swagger  
  ![][./Doc/Swagger.png]

## Validation 설계 필요성
- Validation이 **없는** 구현 : Production 코드
  ```cs
  [HttpPost]
  public IActionResult Register([FromBody] RegisterRequest request)
  {
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
- Validation이 **있는** 구현 : Valiation 코드 + Production 코드
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
- Validation이 **없는** vs. **있는** 구현 
  1. Validation 코드가 Production 코드 보다 많다.
  1. Validation 실패는 첫 실패 또는 전체 실패 정보를 전달할 수 있어야 한다(현재 Validation 코드는 첫 실패 정보만 제공한다).
  1. Validation 대상은 Data Contract과 DomainModel으로 구분할 수 있어야 한다.
     - Data Contract Validation
       ```cs
       if (string.IsNullOrWhiteSpace(request.Name))
           return BadRequest("Name cannot be empty");
       if (request.Name.Length > 200)
           return BadRequest("Name is too long");
       ```
     - DomainModel Validation
       ```cs
       // 2개 이상 과목을 수강할 수 없다. 
       if (_enrollments.Count >= 2)
           throw new Exception("Cannot have more than 2 enrollments");
    
       // 수강 과목은 중복될 수 없다.
       if (_enrollments.Any(x => x.Course == course))
           throw new Exception($"Student '{Name}' already enrolled into course '{course.Name}'");
	   ```

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
    └ Doc                  // 문서
    └ Src
       └ Api               // WebApi 프로젝트
       └ DomainModel       // ClassLib 프로젝트
  ```


  