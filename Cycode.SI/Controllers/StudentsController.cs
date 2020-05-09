using Cycode.Common.Contracts;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cycode.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ICycodeBusinessLogic _businessLogic;


        public StudentsController(ICycodeBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllStudents()
        {
            return Ok(_businessLogic.GetAllStudents());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var student = _businessLogic.TryGetStudentById(id, out var message);
                if (student == null)
                {
                    return BadRequest(message);
                }

                return Ok(student);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpGet]
        [Route("{studentId}/grade/{courseId}")]
        public IActionResult GetGradeOfStudentInCourse(int studentId, int courseId)
        {
            try
            {
                var grade = _businessLogic.TryGetGradeOfStudentInCourse(studentId, courseId, out var message);
                if (grade == null)
                {
                    return BadRequest(message);
                }

                return Ok(grade);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpGet]
        [Route("best")]
        public IActionResult GetBestStudentsInAllCourses()
        {
            return Ok(_businessLogic.GetBestStudent());
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddStudent([FromBody] StudentContract studentContract)
        {
            try
            {
                var student = _businessLogic.TryAddStudent(studentContract, out var message);
                if (student == null)
                {
                    return BadRequest(message);
                }

                return Ok(student);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPost]
        [Route("{studentId}/grade/{courseId}")]
        public IActionResult AddGrade(int studentId, int courseId, [FromBody] GradeContract gradeContract)
        {
            try
            {
                var grade = _businessLogic.TryAddGrade(studentId, courseId, gradeContract, out var message);
                if (grade == null)
                {
                    return BadRequest(message);
                }

                return Ok(grade);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateStudentDetails(int id, [FromBody] StudentContract studentContract)
        {
            try
            {
                var course = _businessLogic.TryUpdateStudentDetails(id, studentContract, out var message);
                if (course == null)
                {
                    return BadRequest(message);
                }

                return Ok(course);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPut]
        [Route("{studentId}/grade/{courseId}")]
        public IActionResult UpdateStudentGrade(int studentId, int courseId, [FromBody] GradeContract gradeContract)
        {
            try
            {
                var grades =
                    _businessLogic.TryUpdateStudentGradeInCourse(studentId, courseId, gradeContract, out var message);
                if (grades == null)
                {
                    return BadRequest(message);
                }

                return Ok(grades);
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                if (_businessLogic.TryDeleteStudent(id, out var message))
                {
                    return BadRequest(message);
                }

                return Ok();
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("{studentId}/grade/{courseId}")]
        public IActionResult DeleteGrade(int studentId, int courseId)
        {
            try
            {
                if (!_businessLogic.TryDeleteGrade(studentId, courseId, out var message))
                {
                    return BadRequest(message);
                }

                return Ok();
            }
            catch (BLException e)
            {
                // Log here
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}