using Cycode.Common.Contracts;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cycode.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICycodeBusinessLogic _businessLogic;


        public CoursesController(ICycodeBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllCourses()
        {
            return Ok(_businessLogic.GetAllCourses());
        }

        [HttpGet]
        [Route("{courseId}")]
        public IActionResult GetCourseDetails(int courseId)
        {
            try
            {
                var course = _businessLogic.TryGetCourseDetailsById(courseId, out var message);
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
        
        [HttpGet]
        [Route("{courseId}/grades")]
        public IActionResult GetGradesOfStudentsInCourse(int courseId)
        {
            try
            {
                var grades = _businessLogic.TryGetGradesOfAllStudentInCourse(courseId, out var message);
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
        
        [HttpGet]
        [Route("easiest")]
        public IActionResult GetEasiestCourse()
        {
            return Ok(_businessLogic.GetBestStudent());
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddCourse([FromBody] CourseContract courseContract)
        {
            try
            {
                var course = _businessLogic.TryAddCourse(courseContract, out var message);
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
        [Route("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] CourseContract courseContract)
        {
            try
            {
                var course = _businessLogic.TryUpdateCourseById(id, courseContract, out var message);
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

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            try
            {
                if (!_businessLogic.TryDeleteCourse(id, out var message))
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