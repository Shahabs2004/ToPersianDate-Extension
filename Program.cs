using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToPersianDate_Extension
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var date = DateTime.Now;

            // Basic properties
            int persianYear = date.GetPersianYear();    // e.g., 1403
            int persianMonth = date.GetPersianMonth();  // 1-12
            int persianDay = date.GetPersianDay();      // 1-31
            int dayOfYear = date.GetPersianDayOfYear(); // 1-366

            // Names and text
            string monthName = date.GetPersianMonthName();       // e.g., "اردیبهشت"
            string monthNameEn = date.GetPersianMonthName(true); // e.g., "Ordibehesht"
            string dayName = date.GetPersianDayName();           // e.g., "دوشنبه"
            string fullText = date.ToPersianDateText();          // e.g., "دوشنبه 16 اردیبهشت 1403"

            // Formatting
            string simpleDate = date.ToPersian();                  // e.g., "1403/02/16"
            string withTime = date.ToPersianWithTime();            // e.g., "1403/02/16 14:30"
            string withSeconds = date.ToPersianWithTime(true);     // e.g., "1403/02/16 14:30:45"
            string custom = date.ToPersian("yyyy/MM/dd HH:mm:ss"); // Custom format

            // Parsing
            DateTime parsedDate = "1403/02/16".ParsePersianDate();

            // Date calculations
            bool isLeap = date.IsPersianLeapYear();
            int daysInMonth = date.GetPersianMonthDays();
            int weekOfMonth = date.GetPersianWeekOfMonth();

            // Date manipulation
            DateTime nextYear = date.AddPersianYears(1);
            DateTime nextMonth = date.AddPersianMonths(1);
            DateTime nextWeek = date.AddPersianDays(7);

            // Range operations
            var (monthStart, monthEnd) = date.GetPersianMonthRange();
            int daysDifference = date.GetPersianDateDifference(nextMonth);
            bool isBetween = date.IsBetweenPersianDates(monthStart, monthEnd);

            // Number conversion
            string persianNums = "1234".ToPersianDigits(); // "۱۲۳۴"
            string englishNums = "۱۲۳۴".ToEnglishDigits(); // "1234"
            Console.ReadLine();
        }
    }
}
