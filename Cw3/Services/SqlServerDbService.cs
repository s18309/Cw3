using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        private string sqlCon = "Data Source=db-mssql;Initial Catalog=s18309;Integrated Security=True";

        public IActionResult RegisterStudent(Student stud)
        {
            using (var con = new SqlConnection(sqlCon))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select 7 from Studies where Studies.Name = @studiesName";
                com.Parameters.AddWithValue("studiesName", stud.Studies);

                con.Open();

                var dr = com.ExecuteReader();
                bool StudiesExist = dr.Read();
                dr.Close();

                if (!StudiesExist)
                {
                    return new BadRequestResult();
                }
                 
                com.CommandText = "SELECT TOP 1  Enrollment.IdEnrollment FROM Enrollment INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy WHERE Enrollment.Semester = 1 AND Studies.Name = @studiesName ORDER BY IdEnrollment DESC; ";
                dr = com.ExecuteReader();
                bool FirstSemesterExists = dr.Read();                
                int IdEnrollment;

                //dodawanie przedmiotu na pierwszym roku
                if (!FirstSemesterExists)
                {
                    dr.Close();
                    com.CommandText =
                        "BEGIN " +
                        "DECLARE @idStudy int = (SELECT Studies.IdStudy FROM Studies" +
                        "WHERE Studies.Name = @studiesName); " +
                        "DECLARE @idEnrollment int = (SELECT TOP 1 Enrollment.IdEnrollment FROM Enrollment " +
                        "ORDER BY Enrollment.IdEnrollment DESC) + 1; " +
                        "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate)" +
                        "VALUES (@idEnrollment, 1, @idStudy, CURRENT_TIMESTAMP) ; " +
                        "Select @idEnrollment;" +
                        "END";

                    dr = com.ExecuteReader();
                    dr.Read();
                    IdEnrollment = dr.GetInt32(0);
                    dr.Close();
                }
                else
                {
                    IdEnrollment = dr.GetInt32(0);
                    dr.Close();
                }

                com.CommandText = "SELECT 7 FROM Student WHERE Student.IndexNumber = @indexNumber";
                com.Parameters.AddWithValue("indexNumber", stud.IndexNumber);

                dr = com.ExecuteReader();
                bool IndexTaken = dr.Read();
                dr.Close();

                if (IndexTaken)
                {
                    return new BadRequestResult();
                }
                
                com.CommandText = "DECLARE @datetmp date = PARSE(@bdate as date USING 'en-GB'); INSERT INTO Student VALUES(@indexNumber, @fname, @lname, @datetmp, @idEnrollment)";
                com.Parameters.Clear();
                com.Parameters.AddWithValue("fname", stud.FirstName);
                com.Parameters.AddWithValue("indexNumber", stud.IndexNumber);
                com.Parameters.AddWithValue("lname", stud.LastName);
                com.Parameters.AddWithValue("bdate", stud.BirthDate.Replace('.', '-'));
                com.Parameters.AddWithValue("idEnrollment", IdEnrollment);

                com.ExecuteNonQuery();


            }

            return new OkResult();

        }

    
       
    }
}
