using System.Collections.Generic;
using Cycode.Common.Contracts;
using Cycode.Common.Models;

namespace Cycode.Common.Interfaces.BusinessLogic
{
    public interface ICycodeBusinessLogic
    {
        Student TryAddStudent(StudentContract studentContract, out string message);

        IEnumerable<Student> GetAllStudents();

        Student TryGetStudentById(int id, out string message);

        Student TryUpdateStudentDetails(int studentId, StudentContract studentContract, out string message);

        bool TryDeleteStudent(int studentId, out string message);

        Grade TryAddGrade(int studentId, int courseId, GradeContract gradeContract, out string message);

        Grade TryGetGradeOfStudentInCourse(int studentId, int courseId, out string message);

        IEnumerable<Grade> TryGetGradesOfAllStudentInCourse(int courseId, out string message);

        Grade TryUpdateStudentGradeInCourse(int studentId, int courseId, GradeContract gradeContract, out string message);

        bool TryDeleteGrade(int studentId, int courseId, out string message);

        Course TryAddCourse(CourseContract courseContract, out string message);

        IEnumerable<Course> GetAllCourses();

        Course TryGetCourseDetailsById(int id, out string message);

        Course TryUpdateCourseById(int courseId, CourseContract courseContract, out string message);

        bool TryDeleteCourse(int courseId, out string message);

        public Student GetBestStudent();

        public Course GetEasiestCourse();
    }
}