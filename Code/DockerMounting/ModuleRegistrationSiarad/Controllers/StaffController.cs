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
    public class StaffController : ControllerBase
    {
        private readonly ModuleRegistrationContext _context;

        public StaffController(ModuleRegistrationContext context)
        {
            _context = context;
        }
        [HttpGet("staff")]
        public ActionResult<string> GetAllStaffMembers()
        {
            var staffId = (from a in _context.staff select a.staff_id.ToUpperInvariant()).Concat(from b in _context.modules select b.coordinator_id.ToUpperInvariant()).Distinct();
            return Ok(staffId);
        }


    }
}