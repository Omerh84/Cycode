using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.Repositories;
using Cycode.Common.Models;

namespace Cycode.DAL.Repositories
{
    public class GradesRepository : IGradesRepository
    {
        private readonly CycodeContext _cycodeContext;

        public GradesRepository(CycodeContext _cycodeContext)
        {
            this._cycodeContext = _cycodeContext;
        }
        
        public Grade AddGrade(Grade grade)
        {
            try
            {
                _cycodeContext.Grades.Add(grade);
                _cycodeContext.SaveChanges();

                return grade;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem adding grade to DB", e);
            }
        }

        public Grade GetGradeOfStudentInCourse(int studentId, int courseId)
        {
            try
            {
                return _cycodeContext.Grades.SingleOrDefault(g =>
                    g.StudentInCourse.CourseId == courseId && g.StudentInCourse.StudentId == studentId);
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting grade from the DB", e);
            }
        }

        public IEnumerable<Grade> GetGradesOfAllStudentInCourse(int courseId)
        {
            try
            {
                return _cycodeContext.Grades.Where(g => g.StudentInCourse.CourseId == courseId).ToList();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting grades of student in course from the DB", e);
            }
        }

        public Grade UpdateStudentGradeInCourse(int gradeId, Grade grade)
        {
            try
            {
                var existGrade = _cycodeContext.Grades.Single(g => g.Id == gradeId);
                existGrade.TestGrade = grade.TestGrade;

                _cycodeContext.SaveChanges();

                return existGrade;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem update student details", e);
            }
        }

        public void DeleteStudentGradeInCourse(Grade grade)
        {
            try
            {
                _cycodeContext.Grades.Remove(grade);
                _cycodeContext.SaveChanges();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem deleting grade from DB", e);
            }
        }
    }
}