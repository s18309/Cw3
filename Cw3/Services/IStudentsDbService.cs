
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public interface IStudentsDbService
    {

        public IActionResult RegisterStudent(Student stud, string studiesName);
        public IActionResult PromoteStudent(int studiesId, int semester);
        public IEnumerable<Student> getStudentsList();

        public void DeleteStudent(string index);
        public void ModifyStudent(Student stud);

    }
}
