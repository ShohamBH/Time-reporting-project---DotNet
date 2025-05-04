using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;
using TimeReporting.Core.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeReporting.Service;

namespace TimeReporting.Controllers
{
    [Authorize(Policy = "UserAndAdmin")] // רק משתמשים ועובדים מורשים
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;
        private readonly IUserService _userService;

        public WorkLogController(IWorkLogService workLogService, IUserService userService)
        {
            _workLogService = workLogService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WorkLog>> Get()
        {
            List<WorkLog> w = _workLogService.GetWorkLogs();
            if (w == null)
                return NotFound();
            return Ok(w);
        }


        [HttpGet("{id}")]
        public ActionResult<WorkLog> Get(int id)
        {
            WorkLog workLog = _workLogService.GetWorkLogById(id);
            if (workLog != null)
                return Ok(workLog);
            return NotFound("workLog ID not found");
        }


        [HttpGet("GetByUserID/{id}")]
        public ActionResult<IEnumerable<WorkLog>> GetByUserId(string id)
        {
            User user = _userService.GetFullUser(id);
            if (user == null)
                return NotFound("User ID not found");
            List<WorkLog> workLogs = _workLogService.GetWorkLogsByUserId(id);
            if (workLogs == null)
                return NotFound();
            return Ok(workLogs);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] WorkLog u)
        {

            User user = _userService.GetFullUser(u.UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }
            u.User = user;
            
            if (await _workLogService.AddWorkLogAsync(u))
                return Ok();

            return BadRequest("workLog with the same ID already exists");

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] WorkLog w)
        {
            WorkLog t = _workLogService.GetWorkLogById(id);
            User user = _userService.GetFullUser(w.UserId);

            if (t == null)
            {
                return NotFound("Id LeaveRequest not found");
            }
            
            w.User = user;
            w.Id = id;
            if (await _workLogService.UpdateWorkLogAsync(id, w))
                return Ok();

            return NotFound("Id WorkLog not found");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            if (await _workLogService.DeleteWorkLogAsyncById(id))
                return Ok();

            return NotFound("Id WorkLog not found");
        }
    }
}