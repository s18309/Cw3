using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cw3.DTOs;
using Cw3.Services;
using Cw3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStudentsList([FromServices] IStudentsDbService db)
        {
            return Ok(db.getStudentsList());

        }
    


    [HttpPost("modStudent")]
    public IActionResult ModifyStudent(Student stud, [FromServices] IStudentsDbService db)
    {
            db.ModifyStudent(stud);
            return Ok();
    }


    [HttpPost("delStudent/{id}")]
    public IActionResult RemoveStudent(string @id,[FromServices] IStudentsDbService db)
    {
            db.DeleteStudent(id);
            return Ok();
    }
}

}