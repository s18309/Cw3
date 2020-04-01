using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Models
{
    public class Student
    {
        public string IndexNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public int IdEnrollment { get; set; }

        public string Studies { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public string StartDate { get; set; }

        public bool IsComplete()
        {
            if (this.FirstName == null || this.LastName == null || this.IndexNumber == null
                || this.BirthDate == null || this.Studies == null) return false;

            return true;
        }





    }
}
