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
      
        public void DeleteStudent(string index)
        {
            var db = new s18309Context();
            var s = db.Student.Where(student => student.IndexNumber == index).First();
            db.Student.Remove(s);
            db.SaveChanges();
            
            
        }


        public IEnumerable<Student> getStudentsList()
        {
            var db = new s18309Context();
            var res = db.Student.ToList();

            return res;
        }

        public void ModifyStudent(Student stud)
        {
            var db = new s18309Context();
            var student = db.Student.Where(s => s.IndexNumber.Equals(stud.IndexNumber)).First();

            if (stud.FirstName != null && !stud.FirstName.Equals(student.FirstName))
            {
                student.FirstName = stud.FirstName;
            }

            if (stud.LastName != null && !stud.LastName.Equals(student.LastName))
            {
                student.LastName = stud.LastName;
            }

            if (stud.IdEnrollment != 0 && stud.IdEnrollment != student.IdEnrollment)
            {
                student.IdEnrollment = stud.IdEnrollment;
            }

            if (stud.BirthDate != null && !stud.BirthDate.Equals(student.BirthDate))
            {
                student.BirthDate = stud.BirthDate;
            }

            db.SaveChanges();

        }

        public IActionResult PromoteStudent(int studiesId, int semester)
        {
            var db = new s18309Context();
            var studiesToPromote = db.Studies.Where(s => s.IdStudy == studiesId).First();

            var enrollmentToPromote =
                db.Enrollment.Where(e => e.IdStudy == studiesToPromote.IdStudy && e.Semester == semester).First();

                var afterPromoteEnrollment = new Enrollment
                {
                    IdEnrollment = db.Enrollment.Max(e => e.IdEnrollment) + 1,
                    Semester = semester + 1,
                    IdStudy = studiesToPromote.IdStudy,
                    StartDate = DateTime.Now
                };
                db.Enrollment.Add(afterPromoteEnrollment);
                db.SaveChanges();
            

            var studentsToUpdate = db.Student.Where(s => s.IdEnrollment == enrollmentToPromote.IdEnrollment)
                .ToList();

            foreach (Student stud in studentsToUpdate)
            {
                stud.IdEnrollment = afterPromoteEnrollment.IdEnrollment;
            }

            db.SaveChanges();

            return new OkResult();
        }

        public IActionResult RegisterStudent(Student stud, string studiesName )
        {
            var db = new s18309Context();
            
               
                var doesExist = db.Studies.Any(s => s.Name.Equals(studiesName));
                if (!doesExist)
                {
                    return new BadRequestResult();
                }

                var idTaken = db.Student.Any(s => s.IndexNumber.Equals(stud.IndexNumber));

                if (idTaken)
                {
                    return new BadRequestResult();
                }

                Student enrolledStud = new Student
                {
                    IndexNumber = stud.IndexNumber,
                    LastName = stud.LastName,
                    BirthDate = stud.BirthDate,
                    FirstName = stud.FirstName,
                    IdEnrollment = 0 //just for a moment tho
                };

                db.Student.Add(enrolledStud);
                db.SaveChanges();

                int idStudy = db.Studies.Where(s => s.Name.Equals(studiesName)).First().IdStudy;
                int lastidEnrollment = db.Enrollment.Where(e => e.Semester == 1 && e.IdStudy == idStudy)
                    .OrderByDescending(e => e.StartDate).First().IdEnrollment;

              
                    Enrollment newEnrollment = new Enrollment
                    {
                        IdEnrollment = lastidEnrollment + 1,
                        Semester = 1,
                        IdStudy = idStudy,
                        StartDate = DateTime.Now
                    };
                    db.Enrollment.Add(newEnrollment);
                    db.SaveChanges();
                

               enrolledStud.IdEnrollment = lastidEnrollment + 1;
               db.SaveChanges();

            return new OkResult();
            }
           
        }
    }

   