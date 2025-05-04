using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeReporting.Core.Entities
{
    public class WorkLog
    {

        public int Id { get; set; }
        public string UserId { get; set; } // מקושר לעובד
        public DateTime EntryTime { get; set; } // זמן כניסה
        public DateTime? ExitTime { get; set; } // זמן יציאה (יכול להיות ריק אם עוד לא יצא)

        public User User { get; set; } // ניווט לעובד

        public WorkLog(int id, string userId, DateTime entryTime, DateTime? exitTime, User user)
        {
            Id = id;
            UserId = userId;
            EntryTime = entryTime;
            ExitTime = exitTime;
            User = user;
        }
        public WorkLog() { }
    }
}
