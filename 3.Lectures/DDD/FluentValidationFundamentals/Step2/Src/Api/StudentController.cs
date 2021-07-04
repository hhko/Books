using System;
using System.Linq;
using System.Text.RegularExpressions;
using DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
			/*
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
			*/

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

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] EditPersonalInfoRequest request)
        {
            Student student = _studentRepository.GetById(id);

            student.EditPersonalInfo(request.Name, request.Address);
            _studentRepository.Save(student);

            return Ok();
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] EnrollRequest request)
        {
            Student student = _studentRepository.GetById(id);

            foreach (CourseEnrollmentDto enrollmentDto in request.Enrollments)
            {
                Course course = _courseRepository.GetByName(enrollmentDto.Course);
                var grade = Enum.Parse<Grade>(enrollmentDto.Grade);
                
                student.Enroll(course, grade);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Student student = _studentRepository.GetById(id);

            var resonse = new GetResonse
            {
                Address = student.Address,
                Email = student.Email,
                Name = student.Name,
                Enrollments = student.Enrollments.Select(x => new CourseEnrollmentDto
                {
                    Course = x.Course.Name,
                    Grade = x.Grade.ToString()
                }).ToArray()
            };
            return Ok(resonse);
        }
    }
}
