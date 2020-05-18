using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tut10.Entities;
using tut10.Models;
using tut10.Services;

namespace tut10.DAL
{
    public class DbService : IDbService
    {
        private readonly StudentContext _context;

        public DbService(StudentContext context)
        {
            _context = context;
        }

        public List<Student> AddStudent(Student student)
        {
            /* Check if such index already exists */
            if (_context.Student.Any(s => s.IndexNumber.Equals(student.IndexNumber))) 
                return null;

            /* Add student */
            _context.Student.Add(student);
            _context.SaveChanges();

            /* Return list of students */
            return GetStudents();
        }

        public List<Student> ChangeStudent(Student student)
        {
            /* check if such student exists */
            if (!_context.Student.Any(s => s.IndexNumber.Equals(student.IndexNumber)))
                return null;

            /* Get the student from the database */
            Student toChange = _context.Student.First(s => s.IndexNumber.Equals(student.IndexNumber));

            /* Change the data */
            _context.Remove(toChange);
            _context.Add(student);
            _context.SaveChanges();

            /* Return list of students */
            return GetStudents();
        }

        public List<Student> DeleteStudent(string index)
        {
            /* check if such student exists */
            if (!_context.Student.Any(s => s.IndexNumber.Equals(index)))
                return null;

            /* Get the student from the database */
            Student toDelete = _context.Student.First(s => s.IndexNumber.Equals(index));

            /* Perform deletion */
            _context.Remove(toDelete);
            _context.SaveChanges();

            /* Return list of students */
            return GetStudents();
        }

        public Enrollment EnrollStudent(EnrollmentRequest student)
        {
            /* Check if all the required data has been delivered */
            if (
                student.IndexNumber is null ||
                student.IndexNumber.Equals(string.Empty) ||
                student.FirstName is null ||
                student.FirstName.Equals(string.Empty) ||
                student.LastName is null ||
                student.LastName.Equals(string.Empty) ||
                student.BirthDate is null ||
                student.BirthDate.Equals(string.Empty) ||
                student.Studies is null ||
                student.Studies.Equals(string.Empty)
            ) { return null; }

            /* Check if provided studies exists */
            if (!_context.Studies.Any(s => s.Name.Equals(student.Studies))) 
                return null;

            /* Check if index number is not occupied */
            if (_context.Student.Any(s => s.IndexNumber.Equals(student.IndexNumber)))
                return null;

            /* Check if such enrollment exists */
            Enrollment enrollment = null;

            if (!_context.Enrollment.Any(e => e.Semester == 1 && e.IdStudyNavigation.Name.Equals(student.Studies)))
            {
                /* If not, create it */
                int newId = _context.Enrollment.Max(e => e.IdEnrollment) + 1;
                int newIdStudies = _context.Studies.First(s => s.Name == student.Studies).IdStudy;

                enrollment = new Enrollment(newId, 1, newIdStudies, DateTime.Now.Date);
                _context.Enrollment.Add(enrollment);
                _context.SaveChanges();
            }
            else
            {
                enrollment = _context.Enrollment.First(e => e.Semester == 1 && e.IdStudyNavigation.Name.Equals(student.Studies));
            }

            /* Create new student */
            AddStudent(new Student {
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = DateTime.Parse(student.BirthDate),
                IdEnrollment = enrollment.IdEnrollment,
                IndexNumber = student.IndexNumber
            });

            return enrollment;
        }

        public List<Student> GetStudents()
        {
            return _context.Student.ToList();
        }

        public Enrollment PromoteStudents(PromotionData data)
        {
            /* Check if old enrollment exists */
            if (!_context.Enrollment.Any(e => e.Semester == Convert.ToInt32(data.Semester) && e.IdStudyNavigation.Name.Equals(data.Name)))
                return null;

            /* Get old enrollment */
            Enrollment oldEnrollment = _context.Enrollment
                .First(e => e.Semester == Convert.ToInt32(data.Semester) && e.IdStudyNavigation.Name.Equals(data.Name));

            Enrollment newEnrollment = null;

            /* Check if new enrollment exists */
            if (!_context.Enrollment.Any(e => e.IdStudy == oldEnrollment.IdStudy && e.Semester == Convert.ToInt32(data.Semester) + 1))
            {
                int newId = _context.Enrollment.Max(e => e.IdEnrollment) + 1;
                newEnrollment = new Enrollment(newId, oldEnrollment.Semester + 1, oldEnrollment.IdStudy, DateTime.Now.Date);

                _context.Enrollment.Add(newEnrollment);
                _context.SaveChanges();
            }
            else
            {
                newEnrollment = _context.Enrollment.First(e => e.IdStudy == oldEnrollment.IdStudy && e.Semester == Convert.ToInt32(data.Semester) + 1);
            }

            /* Update students */
            _context.Student
                .Where(s => s.IdEnrollment == oldEnrollment.IdEnrollment)
                .ToList()
                .ForEach(s => s.IdEnrollment = newEnrollment.IdEnrollment);
            _context.SaveChanges();

            return newEnrollment;
        }
    }
}
