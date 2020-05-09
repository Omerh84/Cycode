using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cycode.Common.Contracts;

namespace Cycode.Common.Models
{
    public class Course
    {
        public Course()
        {
        }

        public Course(CourseContract courseContract)
        {
            Name = courseContract.Name;
        }

        [Key] public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore] public virtual ICollection<StudentInCourse> StudentsInCourses { get; set; }

        public void UpdateFromContract(CourseContract courseContract)
        {
            Name = courseContract.Name;
        }
    }
}