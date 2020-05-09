using System.Collections.Generic;
using Cycode.Common.Models;

namespace Cycode.Common.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Student AddStudent(Student student);
        
        IEnumerable<Student> GetAllStudents();

        Student GetStudentById(int id);

        Student UpdateStudentDetails(int studentId, Student student);

        void DeleteStudent(Student student);
        bool IsStudentWithSameEmailAddressExist(string emailAddress);
    }
}