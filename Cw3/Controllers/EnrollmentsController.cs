using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3.Models;
using Cw3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnrollStudent(Student Student, [FromServices] IStudentsDbService isdbs)
        {
            if (!Student.IsComplete())
            {
                return BadRequest();
            }

            return isdbs.RegisterStudent(Student);
            

            
        }
    }
}