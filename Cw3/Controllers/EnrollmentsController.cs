
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cw3.DTOs;
using Cw3.DTOs;
using Cw3.Handlers;
using Cw3.Models;
using Cw3.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost("enroll")]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent([FromBody]EnrollmentDTO dto, [FromServices]IStudentsDbService db)
        {

            Student stud = new Student
            {
                IndexNumber = dto.IndexNumber,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                BirthDate = dto.BirthDate
            };

            return db.RegisterStudent(stud, dto.StudyName);
        }
            
    [HttpPost("promotions")]
   
    public IActionResult PromoteSemester([FromBody] PromotionDTO dto, [FromServices] IStudentsDbService db)
    {
            return db.PromoteStudent(dto.StudiesId, dto.Semester); ;
    }


    


        
    }


}
