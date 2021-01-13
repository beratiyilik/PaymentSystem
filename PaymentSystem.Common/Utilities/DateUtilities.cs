using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Utilities
{
    public static class DateUtilities
    {
        public static DateTime GetFirstDayOfPreviousMonth()
        {
            DateTime reference = DateTime.Today;
            var month = new DateTime(reference.Year, reference.Month, 1);
            var first = month.AddMonths(-1);
            return first;
        }

        public static DateTime GetLastDayOfPreviousMonth()
        {
            DateTime reference = DateTime.Today;
            var month = new DateTime(reference.Year, reference.Month, 1);
            var last = month.AddDays(-1);
            return last;
        }

        public static DateTime GetFirstDayOfCurrentMonth()
        {
            DateTime reference = DateTime.Now;
            var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
            return firstDayOfMonth;
        }

        public static DateTime GetLastDayOfCurrentMonth()
        {
            DateTime reference = DateTime.Now;
            var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return lastDayOfMonth;
        }

        public static DateTime GetFirstDayOfNextMonth()
        {
            DateTime reference = DateTime.Now;
            DateTime result = new DateTime(reference.AddMonths(1).Year, reference.AddMonths(1).Month, 1);
            return result;
        }

        public static DateTime GetLastDayOfNextMonth()
        {
            DateTime reference = DateTime.Now;
            DateTime firstDayThisMonth = new DateTime(reference.Year, reference.Month, 1);
            DateTime firstDayPlusTwoMonths = firstDayThisMonth.AddMonths(2);
            DateTime lastDayNextMonth = firstDayPlusTwoMonths.AddDays(-1);
            DateTime endOfLastDayNextMonth = firstDayPlusTwoMonths.AddTicks(-1);
            return endOfLastDayNextMonth;
        }
    }
}
