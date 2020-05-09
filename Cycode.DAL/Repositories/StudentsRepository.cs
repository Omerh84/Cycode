﻿using System.Collections.Generic;
 using System.Data.Common;
 using System.Linq;
 using Cycode.Common.Exceptions;
 using Cycode.Common.Interfaces.Repositories;
using Cycode.Common.Models;
 using Microsoft.EntityFrameworkCore;

 namespace Cycode.DAL.Repositories
{
    public class StudentsRepository : IStudentRepository
    {
        private readonly CycodeContext _cycodeContext;

        public StudentsRepository(CycodeContext cycodeContext)
        {
            _cycodeContext = cycodeContext;
        }
        
        public Student AddStudent(Student student)
        {
            try
            {
                _cycodeContext.Students.Add(student);
                _cycodeContext.SaveChanges();

                return student;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem adding student to DB", e);
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            try
            {
                return _cycodeContext.Students.Include(s => s.StudentsInCourses).ToList();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem fetching all students from DB", e);
            }
        }

        public Student GetStudentById(int id)
        {
            try
            {
                return _cycodeContext.Students.SingleOrDefault(s => s.Id == id);
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem getting student details", e);
            }
        }

        public Student UpdateStudentDetails(int studentId, Student student)
        {
            try
            {
                var existStudent = _cycodeContext.Students.Single(s => s.Id == studentId);
                existStudent.FirstName = student.FirstName;
                existStudent.LastName = student.LastName;
                existStudent.EmailAddress = student.EmailAddress;

                _cycodeContext.SaveChanges();

                return student;
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem update student details", e);
            }
        }

        public void DeleteStudent(Student student)
        {
            try
            {
                _cycodeContext.Students.Remove(student);
                _cycodeContext.SaveChanges();
            }
            catch (DbException e)
            {
                throw new DALException("There was a problem deleting student from DB", e);
            }
        }

        public bool IsStudentWithSameEmailAddressExist(string emailAddress)
        {
            return _cycodeContext.Students.Any(s => s.EmailAddress.Equals(emailAddress));
        }
    }
}