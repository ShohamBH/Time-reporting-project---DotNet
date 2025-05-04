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
    public class WorkLogService :IWorkLogService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public WorkLogService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        //יציג בלי משתמש שלם ופרטיו המלאים
        //public List<DTOWorkLog> GetWorkLogs()
        //{
        //    var workLogs = _dataContext.WorkLogs.ToList();
        //    return _mapper.Map<List<DTOWorkLog>>(workLogs);
        //}
        public List<WorkLog> GetWorkLogs()
        {
            var workLogs = _dataContext.WorkLogs
                .Include(w => w.User) // זה החלק החשוב עמ שיציג משתמש שלם
                .ToList();

            return _mapper.Map<List<WorkLog>>(workLogs);
        }


        public WorkLog GetWorkLogById(int id)
        {
            return _dataContext.WorkLogs.FirstOrDefault(i => i.Id == id);
        }

        public List<WorkLog> GetWorkLogsByUserId(string userId)
        {
            var workLogs = _dataContext.WorkLogs
                .Include(lr => lr.User)
                .Where(lr => lr.UserId == userId)
                .ToList();

            return _mapper.Map<List<WorkLog>>(workLogs);
        }
        public async Task<bool> AddWorkLogAsync(WorkLog w)
        {
            if (w == null)
                return false;
            await _dataContext.WorkLogs.AddAsync(w);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWorkLogAsync(int id, WorkLog w)
        {
            var existingW = await _dataContext.WorkLogs.FindAsync(id);

            if (existingW == null)
                return false;

            existingW.EntryTime = w.EntryTime;
            existingW.UserId = w.UserId;
            existingW.ExitTime = w.ExitTime;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWorkLogAsyncById(int id)
        {
            var w = await _dataContext.WorkLogs.FindAsync(id);

            if (w == null)
                return false;

            _dataContext.WorkLogs.Remove(w);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}