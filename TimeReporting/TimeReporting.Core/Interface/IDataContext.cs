using TimeReporting.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeReporting.Core.Interface
{
    public interface IDataContext 
    {
        List<User> usersList { get; set; }
        List<WorkLog> workLogsList { get; set; }
        List<LeaveRequest> leaveRequestsList { get; set; }
    }
}
