using System;
using System.Collections.Generic;

namespace tut10.Entities
{
    public partial class Enrollment
    {
        public Enrollment(int idEnrollment, int semester, int idStudy, DateTime startDate)
        {
            IdEnrollment = idEnrollment;
            Semester = semester;
            IdStudy = idStudy;
            StartDate = startDate;
        }

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Studies IdStudyNavigation { get; set; }
    }
}
