using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut10.Entities;
using tut10.Models;
using tut10.Services;

namespace tut10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _dbService.GetStudents();

            if (students is null) return BadRequest();
            else return Ok(students);
        }

        [Route("add")]
        [HttpPut]
        public IActionResult AddStudent(Student student)
        {
            var students = _dbService.AddStudent(student);

            if (students is null) return BadRequest();
            else return Ok(students);
        }

        [Route("change")]
        [HttpPost]
        public IActionResult ChangeStudent(Student student)
        {
            var students = _dbService.ChangeStudent(student);

            if (students is null) return BadRequest();
            else return Ok(students);
        }
        [Route("delete")]
        [HttpDelete("{@index}")]
        public IActionResult DeleteStudent(string index)
        {
            var students = _dbService.DeleteStudent(index);

            if (students is null) return BadRequest();
            else return Ok(students);
        }

        [Route("promote")]
        [HttpPost]
        public IActionResult PromoteStudents(PromotionData data)
        {
            var enrollment = _dbService.PromoteStudents(data);

            if (enrollment is null) return BadRequest();
            else return Ok(enrollment);
        }

        [Route("enroll")]
        [HttpPost]
        public IActionResult EnrollStudent(EnrollmentRequest student)
        {
            var enrollment = _dbService.EnrollStudent(student);

            if (enrollment is null) return BadRequest();
            else return Ok(enrollment);
        }
    }
}