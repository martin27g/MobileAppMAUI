using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppMAUI.Helpers
{
    public static class DateHelper
    {
        public static bool IsDateInCurrentWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - startOfWeek)) % 7;
            DateTime weekStart = today.AddDays(-diff).Date;
            DateTime weekEnd = weekStart.AddDays(6).Date;

            return date.Date >= weekStart && date.Date <= weekEnd;
        }
        public static bool IsDateInCurrentYear(DateTime date)
        {
            return date.Date.Year == DateTime.Today.Year;
        }

        public static bool IsDateInCurrentMonth(DateTime date)
        {
            var today = DateTime.Today;
            return date.Year == today.Year && date.Month == today.Month;
        }

    }
}

