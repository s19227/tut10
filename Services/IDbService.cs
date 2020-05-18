using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut10.Entities;
using tut10.Models;

namespace tut10.Services
{
    public interface IDbService
    {
        public List<Student> GetStudents();
        public List<Student> AddStudent(Student student);
        public List<Student> ChangeStudent(Student student);
        public List<Student> DeleteStudent(string index);

        public Enrollment PromoteStudents(PromotionData data);
        public Enrollment EnrollStudent(EnrollmentRequest student);
    }
}
