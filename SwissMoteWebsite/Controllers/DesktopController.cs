using Microsoft.AspNet.Identity;
using SwissMoteWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SwissMoteWebsite.Controllers
{


    public class DesktopController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult ApiKeyCheck(string apikey)
        {

            var theapikey = db.Users.Any(u => u.Id == apikey);

            var username = db.Users.Where(a => a.Id == apikey).Select(a => a.UserName).FirstOrDefault();

            if (theapikey)
            {
                return Content("yes" + " username:" + username);
            }

            else
            {
                return Content("no");
            }


        }



        public ActionResult GetProjects(string apikey)
        {

            // Teammemberid => teamname(s) => all assigned projects for this team(s) .

            string userid = apikey;

            string emp_username = db.Users.Where(u => u.Id == apikey).Select(u => u.UserName).FirstOrDefault();

            var allteams = db.Teams.Where(t => t.TeamMemberUserId == userid)
                .Select(t => t.TeamName).ToList();

            var ProjectNames = (from p in db.Projects
                                join t in allteams
                                on p.Team.TeamName equals t
                                select new
                                {
                                    Name = p.ProjectName,
                                    UniqueId = p.UniqueId,
                                    Username = emp_username
                                });


            //var projects = db.InvitationInboxes.Where(a => a.SentToUserId == apikey
            //&& a.IsOn == true && a.invitationInboxstatus == (InvitationInboxStatus)1)
            //    .Select(p => new
            //    {
            //        Name = p.ProjectName,
            //        UniqueId = p.ProjectInvitationUniqueId,
            //        Username = p.SentTo
            //    });



            return Json(ProjectNames, JsonRequestBehavior.AllowGet);


        }



        public string Test()
        {
            var t1 = DateTime.UtcNow;

            Thread.Sleep(1000);

            var t2 = DateTime.UtcNow;

            double result = (t2 - t1).TotalSeconds;

            // return result;

            return result.ToString();

        }


        public double ToSeconds(string s)
        {
            var time = TimeSpan.Parse(s);

            return time.TotalSeconds;

        }

        public ActionResult Report()
        {

            //  List<int> TotalProjectSeconds = new List<int>();

            var userid = User.Identity.GetUserId();

            var actionid = Url.RequestContext.RouteData.Values["id"];

            int id = Convert.ToInt32(actionid);


            ViewBag.Id = id;

            //string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

            string projectsharedid = db.Projects.Where(i => i.Id == id)
               .Select(i => i.UniqueId).FirstOrDefault();


            //string projectname = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectName).FirstOrDefault();

            string projectname = db.Projects.Where(i => i.Id == id)
                .Select(i => i.ProjectName).FirstOrDefault();

            ViewBag.ProjectName = projectname;

            var ThisProjectThisUserList = db.ImageProofs
                .Where(i => i.ProjectUniqueId == projectsharedid)
                .Where(i => i.UploadByUserId == userid);

            if (ThisProjectThisUserList.Count() > 0)
            {



                var lista = ThisProjectThisUserList.ToList()
      .GroupBy(x => x.DesktopUniqueSession,
               (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

                var listb = lista.Select(a => new
                {
                    thedate = a.UploadedDateWithoutTime,
                    desktoptimer = ToSeconds(a.DesktopTimer)

                });


                var Datax = listb.GroupBy(r => r.thedate)
    .Select(
    g => new
    {
        date = g.Key,
        desktoptimerx = g.Sum(s => s.desktoptimer),
    });


                var totalseconds = Datax.Select(d => d.desktoptimerx).Sum();

                var totalminutes = ToMinutes(totalseconds);

                var totalhours = (totalminutes / 60);

                ViewBag.TotalHours = Math.Round(totalhours, 2);

                var max = Datax.Select(d => d.desktoptimerx).Max();

                var maxinminutes = ToMinutes(max);

                ViewBag.MaxValue = maxinminutes;

            }

            return View();



        }



        public DateTime ToHoursMinutes(double totalseconds)
        {

            var minutes = Math.Ceiling(totalseconds / 60);

            TimeSpan time = TimeSpan.FromMinutes(minutes);

            string str = time.ToString(@"hh\:mm");

            var hmm = Convert.ToDateTime(str);
            return hmm;

        }



        public JsonResult TimerJson()
        {

            var userid = User.Identity.GetUserId();

            var actionid = Url.RequestContext.RouteData.Values["id"];

            int id = Convert.ToInt32(actionid);

            //string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

            string projectsharedid = db.Projects.Where(i => i.Id == id)
               .Select(i => i.UniqueId).FirstOrDefault();

            var ThisProjectThisUserList = db.ImageProofs
                .Where(i => i.ProjectUniqueId == projectsharedid)
                .Where(i => i.UploadByUserId == userid);



            var lista = ThisProjectThisUserList.ToList()
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

            var listb = lista.Select(a => new
            {
                thedate = a.UploadedDateWithoutTime,
                desktoptimer = ToSeconds(a.DesktopTimer)

            });



            var Datax = listb.GroupBy(r => r.thedate)
.Select(
g => new
{
    date = g.Key,
    desktoptimerx = ToMinutes(g.Sum(s => s.desktoptimer)),
});




            //            var Datax = Datab.GroupBy(r => r.desktoptimerx)
            //.Select(
            //g => new
            //{
            //    date = g.Key,
            //    desktoptimerx = ToHours(g.Sum(s => s.desktoptimerx)),
            //});





            var dataobject = Datax;
            var chartData = new object[dataobject.Count() + 1];
            chartData[0] = new object[]{
       "date",
       "Timing"
     };
            int w = 0;
            foreach (var i in dataobject)
            {
                w++;
                chartData[w] = new object[] { i.date, i.desktoptimerx };
            }


            return Json(chartData, JsonRequestBehavior.AllowGet);







        }



        public string ChangeDateFormat(string str)

        {


            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(str);

            var date = dt.ToString("yyyy-MM-dd");

            return date;
        }

        public JsonResult ThisProjectDates()
        {

            var userid = User.Identity.GetUserId();

            var actionid = Url.RequestContext.RouteData.Values["id"];

            int id = Convert.ToInt32(actionid);

            //string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

            string projectsharedid = db.Projects.Where(i => i.Id == id)
                .Select(i => i.UniqueId).FirstOrDefault();


            var ThisProjectThisUserList = db.ImageProofs
                .Where(i => i.ProjectUniqueId == projectsharedid)
                .Where(i => i.UploadByUserId == userid);



            var lista = ThisProjectThisUserList.ToList()
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

            var listb = lista.Select(a => new
            {
                thedate = a.UploadedDateWithoutTime,
                desktoptimer = ToSeconds(a.DesktopTimer)

            });



            var Datax = listb.GroupBy(r => r.thedate)
.Select(
g => new
{
    date = ChangeDateFormat(g.Key),
    desktoptimerx = ToMinutes(g.Sum(s => s.desktoptimer)),
});







            var dataobject = Datax;
            var chartData = new object[dataobject.Count() + 1];
            chartData[0] = new object[]{
       "date",

     };
            int w = 0;
            foreach (var i in dataobject)
            {
                w++;
                chartData[w] = new object[] { i.date };
            }


            return Json(chartData, JsonRequestBehavior.AllowGet);







        }

        public double ToMinutes(double totalseconds)
        {

            var minutes = Math.Ceiling((totalseconds / 60));


            return minutes;


        }


        public ActionResult TotalHours()
        {

            string s1 = "2021-05-16";
            DateTime dfrom = Convert.ToDateTime(s1);

            string s2 = "2021-05-18";
            DateTime dto = Convert.ToDateTime(s2);



            var userid = User.Identity.GetUserId();

            var actionid = Url.RequestContext.RouteData.Values["id"];

            int id = Convert.ToInt32(actionid);

            //string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

            string projectsharedid = db.Projects.Where(i => i.Id == id)
               .Select(i => i.UniqueId).FirstOrDefault();

            var ThisProjectThisUserList = db.ImageProofs
                .Where(i => i.ProjectUniqueId == projectsharedid)
                .Where(i => i.UploadByUserId == userid);



            var lista = ThisProjectThisUserList.ToList()
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

            var listb = lista.Select(a => new
            {
                thedate = a.UploadedDateWithoutTime,
                desktoptimer = ToSeconds(a.DesktopTimer)

            });



            var Datab = listb.GroupBy(r => r.thedate)
.Select(
g => new
{
    date = ChangeDateFormat(g.Key),
    desktoptimerx = ToMinutes(g.Sum(s => s.desktoptimer)),
});

            var Datax = listb.GroupBy(r => r.thedate)
.Select(
g => new
{
    date = ToDateType(g.Key),
    desktoptimerx = ToMinutes(g.Sum(s => s.desktoptimer)),
});

            var TotalMinutes = Datax.Where(d => d.date >= dfrom).Where(d => d.date <= dto)
                .Select(d => d.desktoptimerx).Sum();

            var TotalHours = (TotalMinutes / 60);

            var hoursatrange = Math.Round(TotalHours, 2);

            return Content(hoursatrange.ToString());

        }


        public DateTime ToDateType(string date)
        {
            DateTime Date = Convert.ToDateTime(date);

            return Date;


        }


    }




}