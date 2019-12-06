using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuleRegistrationSiarad.Models;

namespace ModuleRegistrationSiarad.Controllers
{
    [Route("")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ModuleRegistrationContext _context;

        public FileController(ModuleRegistrationContext context)
        {
            _context = context;
        }
        //POST data/modules
        [HttpPost("data/modules")]
        public async Task<ActionResult<Module>> PostCsvFile(IFormFile test)
        {
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE modules");
            var file = Request.Form.Files.First();
            StringBuilder content = new StringBuilder();
            StreamReader sr = new StreamReader(file.OpenReadStream());
            while (!sr.EndOfStream)
            {
                String s = sr.ReadLine();
                string[] sa = Regex.Split(s, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                for (int i = 0; i < sa.Length; i++)
                {
                    sa[i].Replace("\"", "");
                    sa[i] = sa[i] + "&#£!*"; //Unique enough key
                    content.Append(sa[i]);
                }
            }
            List<Tuple<String, String, String>> keySet = new List<Tuple<String, String, String>>();
            String[] splitCsv = content.ToString().Split("&#£!*");
            const int yearCell = 0;
            const int codeCell = 3;
            const int nameCell = 5;
            const int class_codeCell = 9;
            const int coordinatorCell = 13;
            const int endCell = 15;
            String year = ""; String module_id = ""; String module_title = ""; String coordinator_id = ""; String class_code = "";
            for (int i = 16; i < splitCsv.Length - 1; i++)
            {
                if (i % 16 == yearCell)
                {
                    year = splitCsv[i];
                }
                else if (i % 16 == codeCell)
                {
                    module_id = splitCsv[i];
                }
                else if (i % 16 == nameCell)
                {
                    module_title = splitCsv[i];
                }
                else if (i % 16 == class_codeCell)
                {
                    class_code = splitCsv[i];
                }
                else if (i % 16 == coordinatorCell)
                {
                    coordinator_id = splitCsv[i];
                }
                if (i % 16 == endCell)
                {
                    Module module = new Module();
                    module.year = year;
                    module.module_id = module_id;
                    module.class_code = class_code;
                    module.module_title = module_title;
                    module.coordinator_id = coordinator_id;
                    if (_context.modules.Find(module_id, year, class_code) == null)
                    { //Checks for pre-existing data prior to this request
                        if (keySet.Any(r => r.Item1.Equals(year) && r.Item2.Equals(module_id) && r.Item3.Equals(class_code)) == false) //Checks for duplications during this request
                        {
                            _context.Add(module);
                            keySet.Add(new Tuple<String, String, String>(year, module_id, class_code));
                        }
                    }
                    year = "";
                    module_id = "";
                    class_code = "";
                    module_title = "";
                    coordinator_id = "";
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        //POST data/students
        [HttpPost("data/students")]
        public async Task<ActionResult<Module>> PostStudentFile(IFormFile test)
        {
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE students");
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE registered_students");
            var file = Request.Form.Files.First();
            StringBuilder content = new StringBuilder();
            List<String> keySet = new List<String>();
            StreamReader sr = new StreamReader(file.OpenReadStream());
            while (!sr.EndOfStream)
            {
                String s = sr.ReadLine();
                string[] sa = Regex.Split(s, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                for (int i = 0; i < sa.Length; i++)
                {
                    sa[i] = sa[i] + "&#£!*"; //Unique enough key
                    content.Append(sa[i]);
                }
            }
            String[] splitCsv = content.ToString().Split("&#£!*");
            const int codeCell = 0;
            const int yearCell = 2;
            const int emailCell = 7;
            const int nameCell = 10;
            const int endCell = 10;
            String module_code = ""; String year = ""; String user_id = ""; String name = "";
            for (int i = 11; i < splitCsv.Length - 1; i++)
            {
                if (i % 11 == codeCell)
                {
                    module_code = splitCsv[i];
                }
                else if (i % 11 == yearCell)
                {
                    year = splitCsv[i];
                }
                else if (i % 11 == emailCell)
                {
                    user_id = splitCsv[i];
                }
                else if (i % 11 == nameCell)
                {
                    name = splitCsv[i];
                }
                if (i % 11 == endCell)
                {
                    name = Regex.Replace(name, @"(\[|""|\])", "");
                    String[] nameSplit = name.Split(',');
                    nameSplit[nameSplit.Length - 1] = nameSplit[nameSplit.Length - 1].Remove(0, 1);
                    Student student = new Student();
                    student.user_id = user_id;
                    student.forename = nameSplit[nameSplit.Length - 1];
                    student.surname = nameSplit[0];
                    if (_context.students.Find(user_id) == null)
                    {
                        if (!keySet.Contains(user_id))
                        {
                            _context.Add(student);
                            keySet.Add(user_id);
                        }
                    }


                    StudentsRegistreredForSpecificModules registerStudent = new StudentsRegistreredForSpecificModules();
                    registerStudent.module_id = module_code;
                    registerStudent.user_id = user_id;
                    registerStudent.year = year;
                    _context.Add(registerStudent);

                    module_code = "";
                    year = "";
                    user_id = "";
                    name = "";
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Until Neil or Nigel sorts out the CSV file to contain year as a field, we'll take it manually
        //POST data/staff/2020
        [HttpPost("data/staff/{year}")]
        public async Task<ActionResult<Module>> PostStaffFile(IFormFile test, String year)
        {
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE staff");
            var file = Request.Form.Files.First();
            StringBuilder content = new StringBuilder();
            StreamReader sr = new StreamReader(file.OpenReadStream());
            while (!sr.EndOfStream)
            {
                String s = sr.ReadLine();
                string[] sa = Regex.Split(s, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                for (int i = 0; i < sa.Length; i++)
                {
                    sa[i] = sa[i] + "&#£!*"; //Unique enough key
                    content.Append(sa[i]);
                }
            }
            String[] splitCsv = content.ToString().Split("&#£!*");
            const int codeCell = 0;
            const int staff_idCell = 3;
            const int endCell = 3;
            String module_code = ""; String staff_id = "";
            for (int i = 4; i < splitCsv.Length - 1; i++)
            {
                if (i % 4 == codeCell)
                {
                    module_code = splitCsv[i];
                }
                else if (i % 4 == staff_idCell)
                {
                    staff_id = splitCsv[i];
                }
                if (i % 4 == endCell)
                {
                    Staff staff = new Staff();
                    staff.module_id = module_code;
                    staff.year = year;
                    staff.staff_id = staff_id;
                    _context.Add(staff);

                    module_code = "";
                    staff_id = "";
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}