using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ModuleRegistrationSiarad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleRegistrationSiarad.Controllers
{

    /*
     * curl --header "Content-Type:application/json" --header "Accept:application/json" --request PUT https://localhost:44377/api/modules/SEM5641 -d {"ModuleId":'SEM5645',"StaffId":'dop2',"ModuleTitle":'Testttt'}
     */
    [Route("")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ModuleRegistrationContext _context;

        public ModulesController(ModuleRegistrationContext context)
        {
            _context = context;
        }
        /**
         * Gets all modules for a specific year
         */
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
        /**
         * Gets the registered students for a specific module
         */
        // GET {module_id}
        [HttpGet("{module_id}")]
        public ActionResult<Module> GetRegisteredStudentsForSpecificModule(String module_id)
        {
            if(module_id == null)
            {
                return NotFound();
            }
            var module = _context.registered_students.Where(m => m.module_id.Equals(module_id));
            if(module == null)
            {
                return NotFound();
            }
            return Ok(module);
        }
        /**
         * Gets all the data within modules
         */
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
        
        // POST year/{year}/{module}
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
        // POST 
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

       

        [HttpPut("year/{year}/{id}")]
//Doesn't like the anti-forgery token, not sure why, need to clarify with Neil
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
            if (!ModuleExists(module.module_id) || !ModuleExists(module.year)) //Primary key modification error - Should be 1 method taking in all primary key params
            {
                return BadRequest();
            }
            throw;
        }
    }
    return Ok(await _context.modules.ToListAsync());
}

//DELETE year/{year}/{module}
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
// DELETE {id}
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
