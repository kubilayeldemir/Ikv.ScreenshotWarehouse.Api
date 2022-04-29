using System;
using System.Collections.Generic;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public static class TurkishMonthsHelper
    {
        static Dictionary<string, int> _turkishMonths = new Dictionary<string, int>
        {
            {"Oca", 1},
            {"Şub", 2},
            {"Mar", 3},
            {"Nis", 4},
            {"May", 5},
            {"Haz", 6},
            {"Tem", 7},
            {"Ağu", 8},
            {"Eyl", 9},
            {"Eki", 10},
            {"Kas", 11},
            {"Ara", 12}
        };

        public static int GetMonthNumberFromMonthTurkishAbbreviation(string monthAbbreviation)
        {
            return _turkishMonths[monthAbbreviation];
        }

        public static DateTime ParseTurkishDatetimeString(string datetimeString)
        {
            var timestamp = new DateTime();
            var dateSplitted = datetimeString.Split('-');

            timestamp = timestamp.AddDays(int.Parse(dateSplitted[0]) - 1);
            timestamp = timestamp.AddMonths(
                TurkishMonthsHelper.GetMonthNumberFromMonthTurkishAbbreviation(dateSplitted[1]) - 1);
            timestamp = timestamp.AddYears(int.Parse(dateSplitted[2]) - 1);
            timestamp = timestamp.AddHours(int.Parse(dateSplitted[3]));
            timestamp = timestamp.AddMinutes(int.Parse(dateSplitted[4]));
            return timestamp;
        }
    }
}