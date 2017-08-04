using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TryIterators
{
    public class DateRange: IEnumerable<DateTime>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public IEnumerable<DateTime> GetDateRange()
        {
            for (var day = StartDate; day <= EndDate; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        /// <summary>
        /// Если хотим обратиться к методу напрямую
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DateTime> GetEnumerator()
        {
            for (var day = StartDate; day <= EndDate; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
