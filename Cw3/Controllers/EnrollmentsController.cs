using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        public IActionResult EnrollStudent(Student Student, [FromServices] IStudentsDbService isdbs)
        {
            if (!Student.IsComplete())
            {
                return BadRequest("działam");
            }

            return isdbs.RegisterStudent(Student);
        }
            
            [HttpPost("promotions")]
            [Authorize(Roles = "employee")]
            public IActionResult PromoteSemester([FromBody] StudiesInfo Studies, [FromServices] IStudentsDbService isdbs)
             {
            return Ok();
             }


        [HttpGet]
        [Authorize(Roles = "employee")]
        public IActionResult DummyCheck()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Login(LoginRequestDto request,[FromServices] IStudentsDbService isdbs)
        {
            var salt = isdbs.getSalt(request.Eska);
            var passw = HashHandler.CreateHash(request.Haslo, salt);


            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18309;Integrated Security=True"))
            using (var com = new SqlCommand())
            {



                com.Connection = con;
                com.CommandText = ("select 1  from Student where IndexNumber = @index AND Password = @Pass");
                com.Parameters.AddWithValue("Pass", passw);
                com.Parameters.AddWithValue("index", request.Eska);
                

                con.Open();

                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    return BadRequest("Wrong login or password");
                }
            }

            //=-----------------------------------------------------------------------------
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Role, "employee"),
            new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DefinietlyNotASecretKeyasd213qwsdeq234123saw"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );
            var refreshTokenik = Guid.NewGuid();
            isdbs.SetREFRESHTOKEN(request.Eska, refreshTokenik.ToString());
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshTokenik

            }) ;
        }


        [HttpPost]
        [Route("ref/{refreshtoken}")]
        public IActionResult sprawdzIWygenerujREFRESHTOKEN( [FromRoute] string refreshtoken ,[FromServices] IStudentsDbService isdbs)
        {
            var index = isdbs.getFromREFRESHTOKEN(refreshtoken);

            if (index.Equals(String.Empty))
            {
                return BadRequest("nie ma");
            }

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Role, "employee"),
            new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DefinietlyNotASecretKeyasd213qwsdeq234123saw"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );
            var refreshTokenik = Guid.NewGuid();
            isdbs.SetREFRESHTOKEN(index, refreshTokenik.ToString());
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshTokenik

            });

        }

        
    }


}