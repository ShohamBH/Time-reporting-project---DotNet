using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeReporting.Core.DTOs;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.Interface
{
    public interface IWorkLogService
    {
        public List<WorkLog> GetWorkLogs();
        public WorkLog GetWorkLogById(int id);
        public List<WorkLog> GetWorkLogsByUserId(string userId);

        public Task<bool> AddWorkLogAsync(WorkLog workLog);
        public Task<bool> UpdateWorkLogAsync(int id, WorkLog w);
        public Task<bool> DeleteWorkLogAsyncById(int id);
    }
}
