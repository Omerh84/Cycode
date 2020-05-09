using System;
using System.Collections.Generic;
using Cycode.Common.Contracts;
using Cycode.Common.Exceptions;
using Cycode.Common.Interfaces.BusinessLogic;
using Cycode.Common.Interfaces.Repositories;
using Cycode.Common.Models;

namespace Cycode.BL
{
    public class CycodeBusinessLogic : ICycodeBusinessLogic
    {
        private readonly IGradesRepository _gradesRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentInCourseRepository _studentInCourseRepository;

        public CycodeBusinessLogic(IGradesRepository gradesRepository, ICoursesRepository coursesRepository,
            IStudentRepository studentRepository, IStudentInCourseRepository studentInCourseRepository)
        {
            _gradesRepository = gradesRepository;
            _coursesRepository = coursesRepository;
            _studentRepository = studentRepository;
            _studentInCourseRepository = studentInCourseRepository;
        }

        public Student TryAddStudent(StudentContract studentContract, out string message)
        {
            if (!Utilities.IsValidStudentContract(studentContract, out message))
            {
                return null;
            }

            try
            {
                // Assuming 2 people can have the exact same name, but not the same email address
                if (_studentRepository.IsStudentWithSameEmailAddressExist(studentContract.EmailAddress))
                {
                    message = "Student with the same email address is already exist";
                    return null;
                }

                var student = new Student(studentContract);
                return _studentRepository.AddStudent(student);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem adding new student", e);
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            try
            {
                return _studentRepository.GetAllStudents();
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem getting all students", e);
            }
        }

        public Student TryGetStudentById(int id, out string message)
        {
            try
            {
                var student = _studentRepository.GetStudentById(id);
                if (student == null)
                {
                    message = "Can't find student with matching id";
                    return null;
                }

                message = String.Empty;
                return student;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem getting student by id", e);
            }
        }

        public Student TryUpdateStudentDetails(int studentId, StudentContract studentContract, out string message)
        {
            if (!Utilities.IsValidStudentContract(studentContract, out message))
            {
                return null;
            }

            try
            {
                var student = _studentRepository.GetStudentById(studentId);
                if (student == null)
                {
                    message = "Student with matching id doesn't exist";
                    return null;
                }

                student.UpdateFromContract(studentContract);

                return _studentRepository.UpdateStudentDetails(studentId, student);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem updating student details", e);
            }
        }

        public bool TryDeleteStudent(int studentId, out string message)
        {
            try
            {
                var student = _studentRepository.GetStudentById(studentId);
                if (student == null)
                {
                    message = "Student with matching id doesn't exist";
                    return false;
                }

                _studentRepository.DeleteStudent(student);

                message = string.Empty;
                return true;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem deleting student", e);
            }
        }

        public Grade TryAddGrade(int studentId, int courseId, GradeContract gradeContract, out string message)
        {
            if (!Utilities.IsValidGradeContract(gradeContract, out message))
            {
                return null;
            }

            try
            {
                var student = _studentRepository.GetStudentById(studentId);
                if (student == null)
                {
                    message = "Student with matching id doesn't exist";
                    return null;
                }

                var course = _coursesRepository.GetCourseById(courseId);
                if (course == null)
                {
                    message = "Course with matching id doesn't exist";
                    return null;
                }

                var studentInCourse = _studentInCourseRepository.GetOrCreateStudentInCourse(studentId, courseId);

                var grade = new Grade
                {
                    TestGrade = gradeContract.Grade,
                    StudentInCourseId = studentInCourse.Id
                };


                message = string.Empty;
                return _gradesRepository.AddGrade(grade);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem adding new grade", e);
            }
        }

        public Grade TryGetGradeOfStudentInCourse(int studentId, int courseId, out string message)
        {
            try
            {
                var student = _studentRepository.GetStudentById(studentId);
                if (student == null)
                {
                    message = "Student with matching id doesn't exist";
                    return null;
                }

                var course = _coursesRepository.GetCourseById(courseId);
                if (course == null)
                {
                    message = "Course with matching id doesn't exist";
                    return null;
                }

                var grade = _gradesRepository.GetGradeOfStudentInCourse(studentId, courseId);
                if (grade == null)
                {
                    message = "Student doesn't take place in this class";
                    return null;
                }

                message = String.Empty;
                return grade;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("Error while trying to get grade of student in course", e);
            }
        }

        public IEnumerable<Grade> TryGetGradesOfAllStudentInCourse(int courseId, out string message)
        {
            try
            {
                var course = _coursesRepository.GetCourseById(courseId);
                if (course == null)
                {
                    message = "Course with matching id doesn't exist";
                    return null;
                }

                message = String.Empty;

                var grades = _gradesRepository.GetGradesOfAllStudentInCourse(courseId);

                if (grades == null)
                {
                    message = "There are no grades for this this course";
                }

                return grades;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("Error while trying to get grades of all students in course", e);
            }
        }

        public Grade TryUpdateStudentGradeInCourse(int studentId, int courseId, GradeContract gradeContract,
            out string message)
        {
            if (!Utilities.IsValidGradeContract(gradeContract, out message))
            {
                return null;
            }

            try
            {
                var grade = _gradesRepository.GetGradeOfStudentInCourse(studentId, courseId);
                if (grade == null)
                {
                    message = "Student with matching id doesn't exist";
                    return null;
                }

                grade.TestGrade = gradeContract.Grade;

                return _gradesRepository.UpdateStudentGradeInCourse(grade.Id, grade);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem adding new student", e);
            }
        }

        public bool TryDeleteGrade(int studentId, int courseId, out string message)
        {
            try
            {
                var grade = _gradesRepository.GetGradeOfStudentInCourse(studentId, courseId);
                if (grade == null)
                {
                    message = "Student doesn't have grade in this class";
                    return false;
                }

                _gradesRepository.DeleteStudentGradeInCourse(grade);

                message = string.Empty;
                return true;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem deleting grade", e);
            }
        }

        public Course TryAddCourse(CourseContract courseContract, out string message)
        {
            if (!Utilities.IsValidCourseContract(courseContract, out message))
            {
                return null;
            }

            try
            {
                if (_coursesRepository.GetCourseByName(courseContract.Name) != null)
                {
                    message = "Course with the same name is already exist";
                    return null;
                }

                var course = new Course(courseContract);
                return _coursesRepository.AddCourse(course);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem adding new student", e);
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            try
            {
                return _coursesRepository.GetAllCourses();
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem getting all students", e);
            }
        }

        public Course TryGetCourseDetailsById(int id, out string message)
        {
            try
            {
                var course = _coursesRepository.GetCourseById(id);
                if (course == null)
                {
                    message = "Course with matching id doesn't exist";
                    return null;
                }

                message = string.Empty;
                return course;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a getting course details", e);
            }
        }

        public Course TryUpdateCourseById(int courseId, CourseContract courseContract, out string message)
        {
            try
            {
                var course = _coursesRepository.GetCourseById(courseId);
                if (course == null)
                {
                    message = "Course with matching id doesn't exist";
                    return null;
                }

                course.UpdateFromContract(courseContract);

                message = string.Empty;
                return _coursesRepository.UpdateCourseDetails(courseId, course);
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a getting course details", e);
            }
        }

        public bool TryDeleteCourse(int courseId, out string message)
        {
            try
            {
                var course = _coursesRepository.GetCourseById(courseId);
                if (course == null)
                {
                    message = "Student doesn't have grade in this class";
                    return false;
                }

                _coursesRepository.DeleteCourse(course);

                message = string.Empty;
                return true;
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem deleting grade", e);
            }
        }

        public Student GetBestStudent()
        {
            try
            {
                return _studentInCourseRepository.GetBestStudent();
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem getting best student", e);
            }
        }

        public Course GetEasiestCourse()
        {
            try
            {
                return _studentInCourseRepository.GetEasiestCourse();
            }
            catch (DALException e)
            {
                // LOG
                throw new BLException("There was a problem getting easiest course", e);
            }
        }
    }
}