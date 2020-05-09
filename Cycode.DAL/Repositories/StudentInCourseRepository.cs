using System.Data.Common;
using System.Linq;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.Repositories;
using Cycode.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Cycode.DAL.Repositories
{
    public class StudentInCourseRepository : IStudentInCourseRepository
    {
        private readonly CycodeContext _cycodeContext;

        public StudentInCourseRepository(CycodeContext cycodeContext)
        {
            _cycodeContext = cycodeContext;
        }

        public StudentInCourse GetOrCreateStudentInCourse(int studentId, int courseId)
        {
            try
            {
                var studentInCourse = _cycodeContext.StudentInCourses.SingleOrDefault(sic =>
                    sic.CourseId == courseId && sic.StudentId == studentId);
                if (studentInCourse == null)
                {
                    studentInCourse = new StudentInCourse
                    {
                        StudentId = studentId,
                        CourseId = courseId
                    };

                    _cycodeContext.StudentInCourses.Add(studentInCourse);
                    _cycodeContext.SaveChanges();
                }
                
                return studentInCourse;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting or creating student in course", e);
            }
        }

        public Student GetBestStudent()
        {
            try
            {
                var studentId = _cycodeContext.StudentInCourses.Select(s => new {studentId = s.StudentId, grade = s.Grade.TestGrade})
                    .GroupBy(i => i.studentId,
                        (i, courses) => new {studentId = i, courseAvg = courses.Average(c => c.grade)})
                    .OrderByDescending(i => i.courseAvg).FirstOrDefault()?.studentId;

                return studentId == null ? null : _cycodeContext.Students.Single(s => s.Id == studentId);
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting best student", e);
            }
        }

        public Course GetEasiestCourse()
        {
            try
            {
                var courseId = _cycodeContext.StudentInCourses
                    .GroupBy(s => s.CourseId,
                        (i, courses) => new {courseId = i, courseAvg = courses.Average(c => c.Grade.TestGrade)})
                    .OrderByDescending(i => i.courseAvg).FirstOrDefault()?.courseId;

                return courseId == null ? null : _cycodeContext.Courses.Single(s => s.Id == courseId);
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting easiest course", e);
            }
        }
    }
}