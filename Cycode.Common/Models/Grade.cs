using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cycode.Common.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        
        public int TestGrade { get; set; }
        
        public int StudentInCourseId { get; set; }
        
        [JsonIgnore]
        public StudentInCourse StudentInCourse { get; set; }
    }
}