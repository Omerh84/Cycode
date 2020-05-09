using System.Collections.Generic;
using Cycode.Common.Models;

namespace Cycode.Common.Interfaces.Repositories
{
    public interface IGradesRepository
    {
        Grade AddGrade(Grade grade);

        Grade GetGradeOfStudentInCourse(int studentId, int courseId);

        IEnumerable<Grade> GetGradesOfAllStudentInCourse(int courseId);

        Grade UpdateStudentGradeInCourse(int gradeId, Grade grade);

        void DeleteStudentGradeInCourse(Grade grade);
    }
}