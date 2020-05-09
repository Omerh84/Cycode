using Cycode.Common.Models;

namespace Cycode.Common.Interfaces.Repositories
{
    public interface IStudentInCourseRepository
    {
        StudentInCourse GetOrCreateStudentInCourse(int studentId, int courseId);

        Student GetBestStudent();

        Course GetEasiestCourse();
    }
}