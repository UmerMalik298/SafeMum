using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;

namespace SafeMum.Infrastructure.Services
{
    public class PregnancyTrackerService : IPregnancyTrackerService
    {
        public int CalculateWeekFromEDD(DateTime edd)
        {
            var weeksRemaining = (edd - DateTime.UtcNow.Date).TotalDays / 7;
            var currentWeek = 40 - (int)weeksRemaining;
            return Math.Clamp(currentWeek, 1, 40);
        }



        public int CalculateWeekFromLMP(DateTime lmp)
        {
            if (lmp > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("LMP cannot be in the future");
            }

            var totalDays = (DateTime.UtcNow.Date - lmp).TotalDays;
            if (totalDays < 0) return 1;

            var week = (int)(totalDays / 7) + 1;
            return Math.Clamp(week, 1, 40);
        }
    }
}
