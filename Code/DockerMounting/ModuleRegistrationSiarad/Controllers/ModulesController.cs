using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Protocols;
using ModuleRegistrationSiarad.Models;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleRegistrationSiarad.Controllers
{

    /*
     * curl --header "Content-Type:application/json" --header "Accept:application/json" --request PUT https://localhost:44377/api/modules/SEM5641 -d {"ModuleId":'SEM5645',"StaffId":'dop2',"ModuleTitle":'Testttt'}
     */
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ModuleRegistrationContext _context;

        public ModulesController(ModuleRegistrationContext context)
        {
            _context = context;
        }
        [HttpGet("year/{year}")]
        public ActionResult<Module> GetModulesForSpecificYear(String year)
        {
            if (year == null)
            {
                return NotFound();
            }
            var moduleList = _context.modules.Where(m => m.year.Equals(year));
            if (moduleList == null)
            {
                return NotFound();
            }
            return Ok(moduleList);
        }
        // GET api/modules/{uid}
        [HttpGet("{uid}")]
        public ActionResult<Module> Get(String uid)
        {
            if(uid == null)
            {
                return NotFound();
            }
            var module = _context.registered_students.Where(m => m.module_id.Equals(uid));
            if(module == null)
            {
                return NotFound();
            }
            return Ok(module);
        }

        // GET api/modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> Get()
        {
            return Ok(await _context.modules.ToListAsync());
        }

        /**
         * Returns a list of modules from a given year that is associated with a specific user.
         */
        // GET api/modules/year/{year}/{uid}
        [HttpGet("year/{year}/{uid}")]
        public ActionResult<string> GetModulesForSpecificYearForSpecificUser(String year, String uid)
        {
            if(year == null || uid == null)
            {
                return NotFound();
            }
            var studentModules = _context.registered_students.Where(r => r.user_id.Equals(uid) && r.year.Equals(year));
            if (studentModules == null)
            {
                return NotFound();
            }
            return Ok(studentModules);
        }
        //Returns a specific module for a specific year

        //Get api/modules/year/{year}/modules/{mid}
        [HttpGet("year/{year}/modules/{mid}")]
        public ActionResult<string> GetSpecificModuleForSpecificYear(String year, String mid)
        {
            if(year == null || mid == null)
            {
                return NotFound();
            }
            var specificModule = _context.modules.Where(r => r.year.Equals(year) && r.module_id.Equals(mid));
            if(specificModule == null)
            {
                return NotFound();
            }
            return Ok(specificModule);
        }

      

        // POST api/modules/year/{year}/{module}
        [HttpPost("year/{year}/{module_id}")]
        public async Task<ActionResult<Module>> PostToSpecificYear([FromBody] Module module, string year, String module_id)
        {
            var mod = await _context.modules.FirstOrDefaultAsync(m => m.module_id.Equals(module_id) && m.year.Equals(year));
            if(mod != null)
            {
                return BadRequest();
            }
            _context.Add(module);
            await _context.SaveChangesAsync();
            return Ok(await _context.modules.ToListAsync());
        }
        // POST api/modules
        [HttpPost]
        public async Task<ActionResult<Module>> Post([FromBody]Module module)
        {
            var mod = await _context.modules.FirstOrDefaultAsync(m => m.module_id.Equals(module.module_id));
            if (mod != null)
            {
                return BadRequest();
            }
            
            _context.Add(module);
                await _context.SaveChangesAsync();
                return Ok(await _context.modules.ToListAsync());
        }

        //POST api/modules/data/modules
        [HttpPost("data/modules")]
        public async Task<ActionResult<Module>> PostCsvFile(IFormFile test)
        {
            var file = Request.Form.Files.First();
            StringBuilder content = new StringBuilder();
                StreamReader sr = new StreamReader(file.OpenReadStream());
                while (!sr.EndOfStream)
                {
                String s = sr.ReadLine();
                string[] sa = Regex.Split(s, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                for(int i=0; i<sa.Length; i++)
                {
                    sa[i].Replace("\"", "");
                    sa[i] = sa[i] + "&#£!*"; //Unique enough key
                    content.Append(sa[i]);
                }
                }
            List<Tuple<String, String, String>> keySet = new List<Tuple<String, String, String>>();
            String[] splitCsv = content.ToString().Split("&#£!*");
            //Year = 0
            //Code = 3
            //Name = 5
            //Class_code = 9
            //Coordinator = 13
            String year = ""; String module_id = ""; String module_title = ""; String coordinator_id = ""; String class_code = "";
            for(int i =16; i<splitCsv.Length-1; i++)
            {
                if (i % 16 == 0)
                {
                    year = splitCsv[i];
                }
                else if (i % 16 == 3)
                {
                    module_id = splitCsv[i];
                }
                else if (i % 16 == 5)
                {
                    module_title = splitCsv[i];
                }
                else if(i % 16 == 9)
                {
                    class_code = splitCsv[i];
                }
                else if (i % 16 == 13)
                {
                    coordinator_id = splitCsv[i];
                }
                if (i % 16 == 15)
                {
                    Module module = new Module();
                    module.year = year;
                    module.module_id = module_id;
                    module.class_code = class_code;
                    module.module_title = module_title;
                    module.coordinator_id = coordinator_id;
                    if (_context.modules.Find(module_id, year, class_code)== null){ //Checks for pre-existing data prior to this request
                        if (keySet.Any(r => r.Item1.Equals(year) && r.Item2.Equals(module_id)  && r.Item3.Equals(class_code))==false) //Checks for duplications during this request
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

        //POST api/modules/data/students
        [HttpPost("data/students")]
        public async Task<ActionResult<Module>> PostStudentFile(IFormFile test)
        {
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
            //Code = 0
            //Year = 2
            //Email = 7
            //Name = 10
            String module_code = ""; String year = ""; String user_id = ""; String name = "";
            for (int i = 11; i < splitCsv.Length - 1; i++)
            {
                if (i % 11 == 0)
                {
                    module_code = splitCsv[i];
                }
                else if (i % 11 == 2)
                {
                    year = splitCsv[i];
                }
                else if (i % 11 == 7)
                {
                    user_id = splitCsv[i];
                }
                else if (i % 11 == 10)
                {
                    name = splitCsv[i];
                }
                if (i % 11 == 10)
                {
                    name = Regex.Replace(name, @"(\[|""|\])", "");
                    String[] nameSplit = name.Split(',');
                    nameSplit[nameSplit.Length-1] = nameSplit[nameSplit.Length - 1].Remove(0,1);
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
        //POST api/modules/data/staff/2020
        [HttpPost("data/staff/{year}")]
        public async Task<ActionResult<Module>> PostStaffFile(IFormFile test, String year)
        {
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
            //Code = 0
            //staff_id = 3
            String module_code = ""; String staff_id = "";
            for (int i = 4; i < splitCsv.Length - 1; i++)
            {
                if (i % 4 == 0)
                {
                    module_code = splitCsv[i];
                }
                else if (i % 4 == 3)
                {
                    staff_id = splitCsv[i];
                }
                if (i % 4 == 3)
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


        [HttpPut("year/{year}/{id}")]
//Doesn't like the anti-forgery token, not sure why, need to clarify with Neal
//Cannot use a put method to edit the primary key -> could be forced with delete/post methods but in that case, just use delete/post methods...
//BadRequest should be given to the user if they attempt this.
public async Task<ActionResult> Put(String id, [Bind("module_title, year, module_id, coordinator_id")] Module module)
{
    if (!id.Equals(module.module_id) && !id.Equals(module.year)){
        return NotFound();
    }
    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(module);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ModuleExists(module.module_id) || !ModuleExists(module.year)) //Primary key modification error
            {
                return BadRequest();
            }
            throw;
        }
    }
    return Ok(await _context.modules.ToListAsync());
}

//DELETE api/modules/year/{year}/{module}
[HttpDelete("year/{year}/{module_id}")]
public async Task<ActionResult> DeleteFromSpecificYear(String year, String module_id)
{
    if(year == null || module_id == null)
    {
        return NotFound();
    }
    var module = await _context.modules.FirstOrDefaultAsync(m => m.module_id.Equals(module_id) && m.year.Equals(year));
    if(module==null){
        return NotFound();
    }
    try
    {
        _context.modules.Remove(module);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return BadRequest();
    }
    return Ok(await _context.modules.ToListAsync());
}
// DELETE api/modules/{id}
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(String id)
{
    if(id == null)
    {
        return NotFound();
    }
    var module = await _context.modules.FirstOrDefaultAsync(m => m.module_id.Equals(id));
    if(module == null)
    {
        return NotFound();
    }
    try
    {
        _context.modules.Remove(module);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return BadRequest();
    }
    return Ok(await _context.modules.ToListAsync());
}

private bool ModuleExists(String id)
{
    return _context.modules.Any(m => m.module_id.Equals(id));
}
}
}
