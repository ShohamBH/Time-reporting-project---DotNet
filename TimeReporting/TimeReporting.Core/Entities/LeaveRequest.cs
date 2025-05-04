using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeReporting.Core.Entities
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; } // העובד שהגיש את הבקשה
        public LeaveType Type { get; set; } // Enum - SickLeave, Vacation, Other //הסיבה
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } // Enum - Pending, Approved, Rejected

        public User User { get; set; } // ניווט לעובד

        public LeaveRequest(int id, string userId, LeaveType type, DateTime startDate, DateTime endDate, LeaveStatus status, User user)
        {
            Id = id;
            UserId = userId;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            User = user;
        }
        public LeaveRequest()
        {

        }
    }

}
