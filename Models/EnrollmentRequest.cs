using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut10.Models
{
    public class EnrollmentRequest
    {
        public EnrollmentRequest()
        {

        }

        public EnrollmentRequest(string indexNumber, string firstName, string lastName, string birthDate, string studies, string semester)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Studies = studies;
            Semester = semester;
        }

        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Studies { get; set; }
        public string Semester { get; set; }
    }
}
