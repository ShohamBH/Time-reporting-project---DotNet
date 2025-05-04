using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.Interface
{
    public interface ILeaveRequestService
    {
        public List<LeaveRequest> GetFullLeaveRequests();
        public List<DTOLeaveRequest> GetDTOLeaveRequestsForEmployee();
        public LeaveRequest GetFullLeaveRequestById(int id);
        public DTOLeaveRequest GetLeaveRequestById(int id);
        public List<LeaveRequest> GetLeaveRequestByUserId(string userId);
        public Task<bool> AddLeaveRequestAsync(LeaveRequest l);
        public Task<bool> UpdateLeaveRequestAsync(int id, LeaveRequest l);
        public Task<bool> DeleteLeaveRequestAsyncById(int id);
    }
}
