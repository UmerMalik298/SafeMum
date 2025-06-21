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
            var today = DateTime.UtcNow.Date;
            int week = 40 - (int)((edd - today).TotalDays / 7);
            return Math.Clamp(week, 1, 40);
        }

        public int CalculateWeekFromLMP(DateTime lmp)
        {
            var today = DateTime.UtcNow.Date;
            int week = (int)((today - lmp).TotalDays / 7) + 1;
            return Math.Clamp(week, 1, 40);
        }
    }
}
