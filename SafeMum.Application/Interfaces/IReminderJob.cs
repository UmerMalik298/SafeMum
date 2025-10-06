using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace SafeMum.Application.Interfaces
    {
        public interface IReminderJob
        {
        Task ExecuteAsync();

        // Send all reminders for a single appointment (generic)
        Task SendAppointmentRemindersAsync(Guid appointmentId);

        // Granular reminder methods (used by your scheduled jobs)
        Task Send24HourReminderAsync(Guid appointmentId);
        Task Send1HourReminderAsync(Guid appointmentId);
        Task Send15MinuteReminderAsync(Guid appointmentId);
    }
    }
