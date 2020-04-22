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

        public IActionResult RegisterStudent(Student stud);

        public bool CheckStudent(string index);

        public void SetREFRESHTOKEN(string index, string token);
        public string getFromREFRESHTOKEN(string REFRESHTOKEN);

        public string getSalt(string index);
        public void SetSalt(string index,string Salt);
        public void SetPassword(string index,string Pssw);
    }
}
