using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cycode.Common.Contracts;

namespace Cycode.Common.Models
{
    public class Student
    {
        public Student()
        {
            
        }

        public Student(StudentContract studentContract)
        {
            FirstName = studentContract.FirstName;
            LastName = studentContract.LastName;
            EmailAddress = studentContract.EmailAddress;
        }

        [Key] public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        [JsonIgnore] public virtual ICollection<StudentInCourse> StudentsInCourses { get; set; }

        public void UpdateFromContract(StudentContract studentContract)
        {
            EmailAddress = studentContract.EmailAddress;
            FirstName = studentContract.FirstName;
            LastName = studentContract.LastName;
        }
    }
}