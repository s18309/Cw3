using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DTOs
{
    public class EnrollmentDTO
    {
            [Required]
            public string StudyName { get; set; }
            [Required]
            public string IndexNumber { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public DateTime BirthDate { get; set; }
            
            
        
    }
}
