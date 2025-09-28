using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Common;

namespace SafeMum.Domain.Entities.AppNotification
{
    public class InAppNotification : BaseEntity
    {
      
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // appointment, reminder, general
  
        public bool IsRead { get; set; }
        public string Data { get; set; } // JSON string for additional data
        public DateTime? ReadAt { get; set; }
    }
}
