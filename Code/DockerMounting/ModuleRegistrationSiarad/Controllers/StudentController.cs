using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuleRegistrationSiarad.Models;

namespace ModuleRegistrationSiarad.Controllers
{
    [Route("")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ModuleRegistrationContext _context;

        public StudentController(ModuleRegistrationContext context)
        {
            _context = context;
        }

        /**
        * Returns the student ID of all registered students
        */
        [HttpGet("students")]
        public ActionResult<string> GetAllRegisteredStudents()
        {
            // var studentId = _context.registered_students.Select(r => new { r.user_id });
            var studentId = (from a in _context.registered_students select a.user_id.ToUpperInvariant()).Distinct();
            return Ok(studentId);
        }
    }
}