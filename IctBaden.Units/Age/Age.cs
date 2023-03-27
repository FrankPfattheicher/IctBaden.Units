using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace IctBaden.Units
{
    public class Age
    {
        public bool IsNegative { get; private set; }
        public int Years { get; private set; }
        public int Months { get; private set; }
        public int Days { get; private set; }


        public int TotalMonths => (IsNegative ? -1 : 1) * Years * 12 + Months;
        

        public Age(DateTime birthDay)
        {
            Count(birthDay);
        }

        public Age(DateTime birthDay, DateTime day)
        {
            Count(birthDay, day);
        }

        public Age Count(DateTime birthDay)
        {
            return Count(birthDay, DateTime.Today);
        }

        public Age Count(DateTime birthDay, DateTime day)
        {
            IsNegative = birthDay > day;
            if (IsNegative)
            {
                (birthDay, day) = (day, birthDay);
            }
            if ((day.Year - birthDay.Year) > 0 ||
                (((day.Year - birthDay.Year) == 0) && ((birthDay.Month < day.Month) ||
                                                    ((birthDay.Month == day.Month) && (birthDay.Day <= day.Day)))))
            {
                var daysInBirthDayMonth = DateTime.DaysInMonth(birthDay.Year, birthDay.Month);
                var daysRemain = day.Day + (daysInBirthDayMonth - birthDay.Day);

                if (day.Month > birthDay.Month)
                {
                    Years = day.Year - birthDay.Year;
                    Months = day.Month - (birthDay.Month + 1) + Math.Abs(daysRemain / daysInBirthDayMonth);
                    Days = (daysRemain % daysInBirthDayMonth + daysInBirthDayMonth) % daysInBirthDayMonth;
                }
                else if (day.Month == birthDay.Month)
                {
                    if (day.Day >= birthDay.Day)
                    {
                        Years = day.Year - birthDay.Year;
                        Months = 0;
                        Days = day.Day - birthDay.Day;
                    }
                    else
                    {
                        Years = (day.Year - 1) - birthDay.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(birthDay.Year, birthDay.Month) - (birthDay.Day - day.Day);
                    }
                }
                else
                {
                    Years = (day.Year - 1) - birthDay.Year;
                    Months = day.Month + (11 - birthDay.Month) + Math.Abs(daysRemain / daysInBirthDayMonth);
                    Days = (daysRemain % daysInBirthDayMonth + daysInBirthDayMonth) % daysInBirthDayMonth;
                }
            }
            
            return this;
        }
    }
}