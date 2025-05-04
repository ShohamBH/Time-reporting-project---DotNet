using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.DTOs
{
    public class DTOLeaveRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; } // העובד שהגיש את הבקשה
        //public LeaveType Type { get; set; } // Enum - SickLeave, Vacation, Other //הסיבה
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } // Enum - Pending, Approved, Rejected

        public DTOUser User { get; set; } // ניווט לעובד

      
        //public DTOLeaveRequest()
        //{

        //}
    }
}
