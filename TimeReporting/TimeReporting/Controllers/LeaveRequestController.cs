using TimeReporting.Core.Entities;
using TimeReporting.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using TimeReporting.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TimeReporting.Controllers
{
    [Authorize(Policy = "UserAndAdmin")] // רק משתמשים ועובדים מורשים
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IUserService _userService;

        public LeaveRequestController(ILeaveRequestService leaveRequest, IUserService userService)
        {
            _leaveRequestService = leaveRequest;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> Get()
        {
            if (User.Claims.Any(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == "admin"))
            {
                List<LeaveRequest> leaveRequests = _leaveRequestService.GetFullLeaveRequests();
                if (leaveRequests != null)
                    return Ok(leaveRequests);
            }
            else
            {
                List<DTOLeaveRequest> leaveRequests = _leaveRequestService.GetDTOLeaveRequestsForEmployee();
                if (leaveRequests != null)
                    return Ok(leaveRequests);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public ActionResult<object> Get(int id)
        {
            LeaveRequest fullLeaveRequest = _leaveRequestService.GetFullLeaveRequestById(id);
            if (fullLeaveRequest != null)
            {
                if (User.Claims.Any(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == "admin"))
                {
                    return Ok(fullLeaveRequest);
                }
                else if (User.FindFirstValue(ClaimTypes.NameIdentifier) == fullLeaveRequest.UserId)
                {
                    return Ok(fullLeaveRequest);
                }
                else
                {
                    var dtoLeaveRequest = _leaveRequestService.GetLeaveRequestById(id);
                    return Ok(dtoLeaveRequest);
                }
            }
            return NotFound("Id LeaveRequest not found");
        }

        [HttpGet("GetByUserID/{id}")]
        public ActionResult<IEnumerable<object>> GetByUserId(string id)
        {
            User user = _userService.GetFullUser(id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            List<LeaveRequest> leaveRequests = _leaveRequestService.GetLeaveRequestByUserId(id);
            if (User.Claims.Any(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value.ToLower() == "admin"))
            {
                return Ok(leaveRequests);
            }
            else
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
                {
                    return Ok(leaveRequests);
                }
                else
                {
                    List<DTOLeaveRequest> dtoLeaveRequests = leaveRequests.Select(lr => new DTOLeaveRequest
                    {
                        Id = lr.Id,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Status = lr.Status,
                        UserId = lr.UserId
                    }).ToList();
                    return Ok(new { Message = "לא הנתונים שלך", Data = dtoLeaveRequests });
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LeaveRequest u)
        {
            User user = _userService.GetFullUser(u.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            u.User = user;
            if (await _leaveRequestService.AddLeaveRequestAsync(u))
                return Ok();
            return BadRequest("Failed to add leave request");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] LeaveRequest l)
        {
            LeaveRequest t = _leaveRequestService.GetFullLeaveRequestById(id);
            User user = _userService.GetFullUser(l.UserId);
            if (t == null)
            {
                return NotFound("Id LeaveRequest not found");
            }
            t.UserId = l.UserId;
            t.Type = l.Type;
            t.StartDate = l.StartDate;
            t.EndDate = l.EndDate;
            t.Status = l.Status;
            t.User = user;
            if (await _leaveRequestService.UpdateLeaveRequestAsync(id, t))
                return Ok();
            return StatusCode(500, "Failed to update leave request");
        }

        [HttpDelete("deleteById/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _leaveRequestService.DeleteLeaveRequestAsyncById(id))
                return Ok();
            return NotFound("Id LeaveRequest not found");
        }
    }
}