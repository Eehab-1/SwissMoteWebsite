using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Models
{
    public class MyFunctions
    {


        public DateTime ToDateType(string date)
        {
            DateTime Date = Convert.ToDateTime(date);

            return Date;


        }

        public double ToMinutes(double totalseconds)
        {

            var minutes = Math.Ceiling((totalseconds / 60));


            return minutes;


        }

        public string ChangeDateFormat(string str)

        {


            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(str);

            var date = dt.ToString("yyyy-MM-dd");

            return date;
        }

        public DateTime ToHoursMinutes(double totalseconds)
        {

            var minutes = Math.Ceiling(totalseconds / 60);

            TimeSpan time = TimeSpan.FromMinutes(minutes);

            string str = time.ToString(@"hh\:mm");

            var hmm = Convert.ToDateTime(str);
            return hmm;

        }


        public string ToHoursMinutesFromMinutes(double totalminutes)
        {



            TimeSpan time = TimeSpan.FromMinutes(totalminutes);

            string str = time.ToString(@"hh\:mm");

            //var hmm = Convert.ToDateTime(str);
            //return hmm;

            return str;
        }





        public double ToSeconds(string s)
        {
            var time = TimeSpan.Parse(s);

            return time.TotalSeconds;

        }












    }
}