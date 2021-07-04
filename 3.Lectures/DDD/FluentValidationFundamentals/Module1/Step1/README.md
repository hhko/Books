# Module 1. |> Step 1. 예제 프로그램 이해하기

## 목차
- 목표
- 실행 정보
- Validation 설계 필요성
- API
- 폴더 구성

## 목표
- Validation 설계 필요성 인식
  - "Data Contract Validation 코드 > Production 코드" 부작용
  - Validation 실패 정보(첫 번째만, 모든 실패)
  - `Data Contract` Validation과 `DomainModel` Validation 구분 필요  
- 예제 프로그램 구성 이해
  - DomainModel : `Enrollment`, `Student`, `Course`, `Grade`
  - API  

## 실행 정보
- http://localhost:5000/swagger  
  <img src="./Doc/Swagger.png" width="600"/>

## Validation 설계 필요성
- Data Contract Validation이 **없는** 구현 : Production 코드
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
- Data Contract Validation이 **있는** 구현 : Data Contract Valiation 코드 + Production 코드
  ```cs
  [HttpPost]
  public IActionResult Register([FromBody] RegisterRequest request)
  {
      //
      // Data Contract Valiation 코드
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
- Data Contract Validation이 **없는** vs. **있는** 구현 
  1. Data Contract Validation 코드가 Production 코드 보다 많다.
  1. Validation 실패는 첫 실패 또는 전체 실패 정보를 전달할 수 있어야 한다.  
     ※ 현재 Data Contract Validation 코드는 첫 실패 정보만 제공한다.  
  1. Validation 대상은 Data Contract과 DomainModel으로 구분할 수 있어야 한다.
     - Data Contract Validation
       ```cs
       // Name 데이터는 NULL 또는 공백이면 안된다.
       if (string.IsNullOrWhiteSpace(request.Name))
           return BadRequest("Name cannot be empty");
	   
       // Name 길이는 200을 초과해서는 안된다.
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

## Domain Model
<img src="./Doc/DomainModel.png"/>

## API
- 학생 조회 : `GET ​/api​/students​/{id}`, 예. `GET ​/api​/students​/1`
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
- 학생 변경 : `PUT ​/api​/students​/{id}`, 예. `PUT ​/api​/students​/3`
  ```json
  {
      "name": "Carl Carlson Jr",
      "address": "3456 3rd St, Carlington, VA, 22203"
  }
  ```

- 학생 변경(등록) : `POST ​/api​/students​/{id}​/enrollments`, 예. `POST ​/api​/students​/3​/enrollments`
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


  
