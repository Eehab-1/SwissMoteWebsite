using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Online
{


    public static class SecurityHelper
    {
        public static Dictionary<string, DateTime> GetLoggedInUsers()
        {
            Dictionary<string, DateTime> loggedInUsers = new Dictionary<string, DateTime>();

            if (HttpContext.Current != null)
            {
                loggedInUsers = (Dictionary<string, DateTime>)HttpContext.Current.Application["loggedinusers"];
                if (loggedInUsers == null)
                {
                    loggedInUsers = new Dictionary<string, DateTime>();
                    HttpContext.Current.Application["loggedinusers"] = loggedInUsers;
                }
            }
            return loggedInUsers;

        }
    }



}