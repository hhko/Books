## 기본 정보
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