using System.Collections.Generic;
using Cycode.Common.Models;

namespace Cycode.Common.Interfaces.Repositories
{
    public interface ICoursesRepository
    {
        Course AddCourse(Course course);
        
        IEnumerable<Course> GetAllCourses();

        Course GetCourseById(int id);

        Course UpdateCourseDetails(int courseId, Course course);

        void DeleteCourse(Course course);
        Course GetCourseByName(string courseContractName);
    }
}