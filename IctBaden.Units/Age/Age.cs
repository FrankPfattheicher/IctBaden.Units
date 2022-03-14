using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace IctBaden.Units
{
    public class Age
    {
        public int Years { get; private set; }
        public int Months { get; private set; }
        public int Days { get; private set; }


        public int TotalMonths => Years * 12 + Months;
        

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
            if ((day.Year - birthDay.Year) > 0 ||
                (((day.Year - birthDay.Year) == 0) && ((birthDay.Month < day.Month) ||
                                                    ((birthDay.Month == day.Month) && (birthDay.Day <= day.Day)))))
            {
                var daysInBirthDayMonth = DateTime.DaysInMonth(birthDay.Year, birthDay.Month);
                var daysRemain = day.Day + (daysInBirthDayMonth - birthDay.Day);

                if (day.Month > birthDay.Month)
                {
                    this.Years = day.Year - birthDay.Year;
                    this.Months = day.Month - (birthDay.Month + 1) + Math.Abs(daysRemain / daysInBirthDayMonth);
                    this.Days = (daysRemain % daysInBirthDayMonth + daysInBirthDayMonth) % daysInBirthDayMonth;
                }
                else if (day.Month == birthDay.Month)
                {
                    if (day.Day >= birthDay.Day)
                    {
                        this.Years = day.Year - birthDay.Year;
                        this.Months = 0;
                        this.Days = day.Day - birthDay.Day;
                    }
                    else
                    {
                        this.Years = (day.Year - 1) - birthDay.Year;
                        this.Months = 11;
                        this.Days = DateTime.DaysInMonth(birthDay.Year, birthDay.Month) - (birthDay.Day - day.Day);
                    }
                }
                else
                {
                    this.Years = (day.Year - 1) - birthDay.Year;
                    this.Months = day.Month + (11 - birthDay.Month) + Math.Abs(daysRemain / daysInBirthDayMonth);
                    this.Days = (daysRemain % daysInBirthDayMonth + daysInBirthDayMonth) % daysInBirthDayMonth;
                }
            }
            else
            {
                throw new ArgumentException("Birthday date must be earlier than current date");
            }
            
            return this;
        }
    }
}