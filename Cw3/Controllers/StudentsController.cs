using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

      //  private readonly IDbService _dbService;
/*
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        */

        [HttpGet]
        public IActionResult GetStudent()
        {
            //return Ok(_dbService.GetStudents());
            List<Student> _students = new List<Student>();

            using (var con  = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18309;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student inner join Enrollment on Student.IdEnrollment = Enrollment.IdEnrollment inner join Studies on Studies.IdStudy = Enrollment.IdStudy";

                con.Open();

                var dr = com.ExecuteReader();
                //IEnumerable<Student> _students = new List<Student>();

                while (dr.Read())
                {
                    var st = new Student();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Studies = dr["Name"].ToString();
                    st.Semester = Convert.ToInt32(dr["Semester"]);
                    _students.Add(st);
                }

                return Ok(_students);

            }
        }


        [HttpGet("{id}")]
        public IActionResult GetStudent(String id)
        {
            List<Student> _students = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18309;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = ("select *  from Enrollment inner join Student on Student.IdEnrollment = Enrollment.IdEnrollment where IndexNumber =" + @id);
                com.Parameters.AddWithValue("id", id);

                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();                    
                    st.Semester = Convert.ToInt32(dr["Semester"]);
                    st.IdStudy = Convert.ToInt32(dr["IdStudy"]);
                    st.IdEnrollment = Convert.ToInt32(dr["IdEnrollment"]);
                    st.StartDate = dr["StartDate"].ToString();
                    _students.Add(st);
                }

                return Ok(_students);
            }
            }


        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }


        [HttpPut("{id}")]
        public IActionResult AlterStudent(int id)
        {
            return Ok("Aktualizacja dokonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DropStudent(int id)
        {
            return Ok("Usuwanie ukonczone");
        }


    }


}