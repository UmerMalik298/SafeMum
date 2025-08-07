using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Interfaces
{
    public interface IReminderJob
    {
        Task SendAppointmentRemindersAsync(Guid appointmentId);
        Task ExecuteAsync();
    }
}
