using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.Repositories;
using Cycode.Common.Models;

namespace Cycode.DAL.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly CycodeContext _cycodeContext;

        public CoursesRepository(CycodeContext cycodeContext)
        {
            _cycodeContext = cycodeContext;
        }
        
        public Course AddCourse(Course course)
        {
            try
            {
                _cycodeContext.Courses.Add(course);
                _cycodeContext.SaveChanges();

                return course;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem adding course to DB", e);
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            try
            {
                return _cycodeContext.Courses.ToList();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting courses from the DB", e);
            }
        }

        public Course GetCourseById(int id)
        {
            try
            {
                return _cycodeContext.Courses.SingleOrDefault(c => c.Id == id);
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting course from the DB", e);
            }
        }

        public Course UpdateCourseDetails(int courseId, Course course)
        {
            try
            {
                var existCourse = _cycodeContext.Courses.Single(c => c.Id == courseId);
                existCourse.Name = course.Name;

                _cycodeContext.SaveChanges();

                return existCourse;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem update course details", e);
            }
        }

        public void DeleteCourse(Course course)
        {
            try
            {
                _cycodeContext.Courses.Remove(course);
                _cycodeContext.SaveChanges();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem deleting course", e);
            }
        }

        public Course GetCourseByName(string courseContractName)
        {
            try
            {
                return _cycodeContext.Courses.SingleOrDefault(c => c.Name.Equals(courseContractName));
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem deleting course", e);
            }
        }
    }
}