using System.Globalization;

namespace System
{
    public static class PersianDateTimeExtensions
    {
        private static readonly PersianCalendar _pc = new PersianCalendar();
        private static readonly string[] _persianMonthNames = {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };
        private static readonly string[] _persianDayNames = {
            "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"
        };
        private static readonly string[] _persianMonthNamesInEnglish = {
            "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar",
            "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand"
        };

        #region Basic Persian Date Properties
        public static int GetPersianYear(this DateTime date) => _pc.GetYear(date);
        public static int GetPersianMonth(this DateTime date) => _pc.GetMonth(date);
        public static int GetPersianDay(this DateTime date) => _pc.GetDayOfMonth(date);
        public static DayOfWeek GetPersianDayOfWeek(this DateTime date) => _pc.GetDayOfWeek(date);
        public static int GetPersianDayOfYear(this DateTime date) => _pc.GetDayOfYear(date);
        #endregion

        #region Persian Names and Text
        public static string GetPersianMonthName(this DateTime date, bool inEnglish = false)
        {
            var monthIndex = _pc.GetMonth(date) - 1;
            return inEnglish ? _persianMonthNamesInEnglish[monthIndex] : _persianMonthNames[monthIndex];
        }

        public static string GetPersianDayName(this DateTime date)
        {
            int dayIndex = ((int)date.DayOfWeek + 1) % 7;
            return _persianDayNames[dayIndex];
        }

        public static string ToPersianDateText(this DateTime date)
        {
            return $"{date.GetPersianDayName()} {date.GetPersianDay()} {date.GetPersianMonthName()} {date.GetPersianYear()}";
        }
        #endregion

        #region Formatting and Parsing
        public static string ToPersian(this DateTime date, string format = "")
        {
            if (string.IsNullOrEmpty(format))
                format = "yyyy/MM/dd";

            try
            {
                var culture = new CultureInfo("fa-IR");
                culture.DateTimeFormat.Calendar = _pc;

                return date.ToString(format, culture);
            }
            catch (Exception ex)
            {
                throw new Exception("Error converting to Persian date: " + ex.Message);
            }
        }

        public static string ToPersianWithTime(this DateTime date, bool includeSeconds = false)
        {
            string format = includeSeconds ? "yyyy/MM/dd HH:mm:ss" : "yyyy/MM/dd HH:mm";
            return date.ToPersian(format);
        }

        public static DateTime ParsePersianDate(this string persianDate)
        {
            try
            {
                string[] parts = persianDate.Split('/', '-');
                if (parts.Length < 3)
                    throw new FormatException("Invalid Persian date format. Use yyyy/MM/dd or yyyy-MM-dd");

                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                return _pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing Persian date: {ex.Message}");
            }
        }
        #endregion

        #region Date Calculations and Validations
        public static bool IsPersianLeapYear(this DateTime date)
        {
            return _pc.IsLeapYear(date.GetPersianYear());
        }

        public static int GetPersianMonthDays(this DateTime date)
        {
            int month = date.GetPersianMonth();
            int year = date.GetPersianYear();

            if (month <= 6)
                return 31;
            else if (month <= 11)
                return 30;
            else
                return _pc.IsLeapYear(year) ? 30 : 29;
        }

        public static DateTime AddPersianYears(this DateTime date, int years)
        {
            return _pc.AddYears(date, years);
        }

        public static DateTime AddPersianMonths(this DateTime date, int months)
        {
            int currentYear = date.GetPersianYear();
            int currentMonth = date.GetPersianMonth();
            int newMonth = currentMonth + months;

            if (newMonth > 12)
            {
                currentYear += newMonth / 12;
                newMonth = newMonth % 12;
            }
            else if (newMonth < 1)
            {
                currentYear += (newMonth - 12) / 12;
                newMonth = 12 + (newMonth % 12);
            }

            // Ensure the day is valid for the new month
            int day = date.GetPersianDay();
            int daysInNewMonth = _pc.GetDaysInMonth(currentYear, newMonth);
            if (day > daysInNewMonth)
            {
                day = daysInNewMonth; // Adjust to the last valid day of the month
            }

            return _pc.ToDateTime(currentYear, newMonth, day, 0, 0, 0, 0);
        }


        public static DateTime AddPersianDays(this DateTime date, int days)
        {
            return _pc.AddDays(date, days);
        }

        public static int GetPersianWeekOfMonth(this DateTime date)
        {
            return (_pc.GetDayOfMonth(date) - 1) / 7 + 1;
        }
        #endregion

        #region Comparison and Range Operations
        public static (DateTime Start, DateTime End) GetPersianMonthRange(this DateTime date)
        {
            int year = date.GetPersianYear();
            int month = date.GetPersianMonth();
            var startDate = _pc.ToDateTime(year, month, 1, 0, 0, 0, 0);
            var endDate = startDate.AddPersianMonths(1).AddDays(-1);
            return (startDate, endDate);
        }

        public static int GetPersianDateDifference(this DateTime date1, DateTime date2)
        {
            return (int)(date2 - date1).TotalDays;
        }

        public static bool IsBetweenPersianDates(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }
        #endregion

        #region Utility Methods
        public static string ToPersianDigits(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input
                .Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string ToEnglishDigits(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9");
        }
        #endregion
    }
}