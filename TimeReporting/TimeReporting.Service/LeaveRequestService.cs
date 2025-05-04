using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;
using TimeReporting.Core.Interface;
using TimeReporting.Data;

namespace TimeReporting.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public LeaveRequestService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public List<LeaveRequest> GetFullLeaveRequests()
        {
            return _dataContext.LeaveRequests
                .Include(lr => lr.User)
                .ToList();
        }

        public List<DTOLeaveRequest> GetDTOLeaveRequestsForEmployee()
        {
            var leaveRequests = _dataContext.LeaveRequests
                .Include(lr => lr.User)
                .ToList();

            return _mapper.Map<List<DTOLeaveRequest>>(leaveRequests);
        }

        public LeaveRequest GetFullLeaveRequestById(int id)
        {
            return _dataContext.LeaveRequests
                .Include(lr => lr.User)
                .FirstOrDefault(i => i.Id == id);
        }

        public DTOLeaveRequest GetLeaveRequestById(int id)
        {
            LeaveRequest l = _dataContext.LeaveRequests.FirstOrDefault(i => i.Id == id);
            if (l != null)
                return _mapper.Map<DTOLeaveRequest>(l);

            return null;
        }

        public List<LeaveRequest> GetLeaveRequestByUserId(string userId)
        {
            return _dataContext.LeaveRequests
                .Include(lr => lr.User)
                .Where(lr => lr.UserId == userId)
                .ToList();
        }

        public async Task<bool> AddLeaveRequestAsync(LeaveRequest l)
        {
            if (l == null)
                return false;
            await _dataContext.LeaveRequests.AddAsync(l);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLeaveRequestAsync(int id, LeaveRequest l)
        {
            var existingL = await _dataContext.LeaveRequests.FindAsync(id);

            if (existingL == null)
                return false;

            existingL.UserId = l.UserId;
            existingL.Type = l.Type;
            existingL.StartDate = l.StartDate;
            existingL.EndDate = l.EndDate;
            existingL.Status = l.Status;
            existingL.User = l.User;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLeaveRequestAsyncById(int id)
        {
            var l = await _dataContext.LeaveRequests.FindAsync(id);

            if (l == null)
                return false;

            _dataContext.LeaveRequests.Remove(l);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}