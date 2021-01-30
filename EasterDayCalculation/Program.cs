using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasterDayCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please select calender");
            bool? gregorianCalender = null;
            
            while(gregorianCalender== null)
            {
                Console.WriteLine("j = Julian calendar  g = Gregorian calendar ");
                Console.Write("j eller g:");
                var calenderType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(calenderType))
                {
                    if (calenderType.Equals("g", StringComparison.OrdinalIgnoreCase))
                    {
                        gregorianCalender = true;
                    }
                    if (calenderType.Equals("j", StringComparison.OrdinalIgnoreCase))
                    {
                        gregorianCalender = false;
                    }
                }
            }
            
            while (true)
            {
                Console.WriteLine("Please input year (ex: 1991)");
                var year = Console.ReadLine();

                #if DEBUG
                    if (year.StartsWith("t"))
                    {
                        year = year.Substring(1);
                    for (int i = 1900; i < 2100; i++)
                    {
                        try
                        {
                            Console.WriteLine("Easter is : " + CalculateEasterDay(i.ToString(), gregorianCalender).ToShortDateString());
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                #endif


                try{

                    Console.WriteLine("Easter is : " + CalculateEasterDay(year, gregorianCalender).ToShortDateString());
                }
                
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            



        }

        private static DateTime CalculateEasterDay(string year, bool? gregorianCalender)
        {
            DateTime easterDay;
            if (string.IsNullOrWhiteSpace(year))
            {
                throw new Exception("Value is blank or not valid");
            }

            DateTime inputYear;

            if (!DateTime.TryParseExact(year, "yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out inputYear))
            {
                throw new Exception(string.Format("Value is not a valid year, the format is yyyy (1981) and year needs to be above {0} and less then {1}",DateTime.MinValue.Year+1, DateTime.MaxValue.Year-1 ));
            }
            if (!gregorianCalender.HasValue)
            {
                throw new Exception("You need to restart and set calender type");
            }


            int intYear = inputYear.Year;

            double a, b, c, d, e,N,M, k,p,q;

            N = 6;
            M = 15;
            if (gregorianCalender.Value)
            {
                k = Math.Floor((double)(intYear/100));
                q = Math.Floor(k / 4);
                N = (4 + k - q) % 7;
                p = Math.Floor((13+8*k)/25);
                M = (15 - p + k - q) % 30;
            }

            a = intYear % 19;
            b = intYear % 4;
            c = intYear % 7;
            d = ((19*a)+M)%30;
            e = (2*b+4*c+6*d+N)%7;

            if (d == 29 && e == 6)
            {
                return new DateTime(intYear, 4, 19);
            }
            if (d == 28 && e == 6 && a== 10 )
            {
                return new DateTime(intYear, 4, 18);
            }

            var dateMarch = 22 + d + e;
            var dateApril = d + e - 9;
            

            if (dateMarch>= 22 && dateMarch<=31)
            {
                easterDay = new DateTime(intYear, 3, Convert.ToInt32(Math.Floor(dateMarch)));
            }else if (dateApril >= 1 && dateApril <= 25)
            {
                easterDay = new DateTime(intYear, 4, Convert.ToInt32(Math.Floor(dateApril)));
            }
            else
            {
                throw new Exception(string.Format("Calculation Error: Year:{0}   DateMarch:{1}  ,DateApril: {2} ",year, dateMarch, dateApril));
            }

            return easterDay;
        }
    }
}
