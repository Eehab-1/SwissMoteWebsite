using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using SwissMoteWebsite.Models;

namespace SwissMoteWebsite.Controllers
{
    public class ImageProofController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        MyFunctions myfunctions = new MyFunctions();

        public ActionResult Progress(int? page)
        {
            string actionid = Url.RequestContext.RouteData.Values["id"].ToString();

            string currentuserid = User.Identity.GetUserId();

            string projectcreatoruserid = db.Projects.Where(a => a.UniqueId == actionid)
                .Select(a => a.CreatedByUserId).FirstOrDefault();


            //TempData["projectuniqueid"] = actionid;

            if (currentuserid == projectcreatoruserid)
            {


                return View(db.Projects.ToList()
                    .Where(m => m.UniqueId == actionid).ToPagedList(page ?? 1, 10));

            }

            else
            {
                return Content("Not Authorized Access");
            }


        }


        public ActionResult ProgressByMember()
        {





            //string actionid = Url.RequestContext.RouteData.Values["id"].ToString();

            //string TeamUniqueId = actionid;

            var actionid = Url.RequestContext.RouteData.Values["id"];


            int projectid = Convert.ToInt32(actionid);

            // from projectid => teamid => teamuniqueid => list all assigned team members .

            int teamid = db.Projects.Where(p => p.Id == projectid)
                .Select(p => p.TeamId).FirstOrDefault();

            string teamuniqueid = db.Teams.Where(t => t.TeamId == teamid)
                .Select(t => t.TeamUniqueId).FirstOrDefault();



            string userid = User.Identity.GetUserId();

            return View(db.Teams.Where(a => a.TeamCreatedByUserId == userid).Where(t=>t.TeamUniqueId==teamuniqueid)
                .ToList());
        }


        public ActionResult Show(int? page , string empid , int proid)
        {


         //   var actionid = Url.RequestContext.RouteData.Values["id"];

            //int id = Convert.ToInt32(actionid);

            string emp_userid = empid;

            //var inviteduserid = db.ProjectInvitations.Where(u => u.Id == id)
            //    .Select(d => d.Invited_UserId).FirstOrDefault();

            // var inviteduserid = emp_userid;

           var inviteduserid = empid;


            string creatoruserid = db.Teams.Where(a => a.TeamMemberUserId ==  emp_userid)
                .Select(u => u.TeamCreatedByUserId).FirstOrDefault();

            string currentuserid = User.Identity.GetUserId();

            if (currentuserid == creatoruserid)
            {


                //var projectuniqueid = db.Projects.Where(u => u.Id == id)
                //    .Select(p => p.UniqueId).FirstOrDefault();



                string projectuniqueid = db.Projects.Where(p => p.Id == proid)
                    .Select(p => p.UniqueId).FirstOrDefault();
                    

                var listOfSpecificImagesProofs = db.ImageProofs.ToList()
                    .Where(j => j.ProjectUniqueId == projectuniqueid)
                    .Where(s => s.UploadByUserId == inviteduserid);

                // Get Project Name and User Email and send them to view :

                var UserEmail = db.Teams.Where(d => d.TeamMemberUserId == inviteduserid)
                    .Select(u => u.TeamMember).FirstOrDefault();

                ViewBag.EmailUserName = UserEmail;

                var ProjectName = db.Projects.Where(d => d.UniqueId == projectuniqueid)
                   .Select(u => u.ProjectName).FirstOrDefault();

                ViewBag.ProjectName = ProjectName;


                var TodayDate = listOfSpecificImagesProofs.Select(s => s.UploadedDateWithoutTime).LastOrDefault();


                var AllSessionsTodayList = listOfSpecificImagesProofs
                    .Where(d => d.UploadedDateWithoutTime == TodayDate);

                var RecordsWithMaxMouseClicksToday = AllSessionsTodayList
        .GroupBy(x => x.DesktopUniqueSession,
                 (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

                var TodayMouseClicks = RecordsWithMaxMouseClicksToday.Select(m => m.MouseClicks).Sum();
                ViewBag.TodayMouseClicks = TodayMouseClicks;


                var RecordsWithMaxKeyboardHitsToday = AllSessionsTodayList
       .GroupBy(x => x.DesktopUniqueSession,
                (k, g) => g.Aggregate((a, x) => (x.KeyBoardHits > a.KeyBoardHits) ? x : a));

                var TodayKeyboardHits = RecordsWithMaxKeyboardHitsToday.Select(k => k.KeyBoardHits).Sum();

                ViewBag.TodayKeyPresses = TodayKeyboardHits;



                var RecordsWithMaxMouseClicksTotal = listOfSpecificImagesProofs
       .GroupBy(x => x.DesktopUniqueSession,
                (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

                var TotalMouseClicks = RecordsWithMaxMouseClicksTotal.Select(m => m.MouseClicks).Sum();

                ViewBag.TotalMouseClicks = TotalMouseClicks;




                var RecordsWithMaxKeyboardHitsTotal = listOfSpecificImagesProofs
                .GroupBy(x => x.DesktopUniqueSession,
              (k, g) => g.Aggregate((a, x) => (x.KeyBoardHits > a.KeyBoardHits) ? x : a));

                var TotalKeyboardHits = RecordsWithMaxKeyboardHitsTotal.Select(k => k.KeyBoardHits).Sum();




                ViewBag.TotalKeyPresses = TotalKeyboardHits;




                var latestlist = listOfSpecificImagesProofs.OrderByDescending(u => u.UploadedDate);


                return View(latestlist.ToList()
                        .ToPagedList(page ?? 1, 4));
            }

            else
            {


                return Content("Non Authorized Access");

            }
        }






        public ActionResult ProgressChart(string empid, int proid)
        {

            //var actionid = Url.RequestContext.RouteData.Values["id"];

            //int id = Convert.ToInt32(actionid);

            int id = proid;

            string creatoruserid = db.Projects.Where(a => a.Id == id)
            .Select(u => u.CreatedByUserId).FirstOrDefault();



            string currentuserid = User.Identity.GetUserId();

            if (creatoruserid == currentuserid)
            {


                ViewBag.Id = id;



                //var inviteduserid = db.ProjectInvitations.Where(u => u.Id == id)
                //    .Select(d => d.Invited_UserId).FirstOrDefault();

                var inviteduserid = empid;


                var projectuniqueid = db.Projects.Where(u => u.Id == id)
                    .Select(p => p.UniqueId).FirstOrDefault();

                #region Get Invitationinboxes id for TimerJsonEmployerSide + related data

                //int InvitationInboxesId = db.InvitationInboxes.Where(i => i.ProjectInvitationUniqueId == projectuniqueid)
                //    .Where(i => i.SentToUserId == inviteduserid).Select(i => i.Id).FirstOrDefault();

                //ViewBag.InvitationInboxesId = InvitationInboxesId;

                var ThisProjectThisUserList = db.ImageProofs
              .Where(i => i.ProjectUniqueId == projectuniqueid)
              .Where(i => i.UploadByUserId == inviteduserid);



                var lista = ThisProjectThisUserList.ToList()
      .GroupBy(x => x.DesktopUniqueSession,
               (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

                var listb = lista.Select(a => new
                {
                    thedate = a.UploadedDateWithoutTime,
                    desktoptimer = myfunctions.ToSeconds(a.DesktopTimer)

                });



                var Dataxwork = listb.GroupBy(r => r.thedate)
    .Select(
    g => new
    {
        date = g.Key,
        desktoptimerx = myfunctions.ToMinutes(g.Sum(s => s.desktoptimer)),
    });

                var desktoptimerx_count = Dataxwork.Select(a => a.desktoptimerx).Count();
                if (desktoptimerx_count == 0)
                {
                    return View();
                }

                var maxinwork = Dataxwork.Select(x => x.desktoptimerx).Max();

                ViewBag.MaxInWork = maxinwork;


                #endregion




                var projectname = db.Projects.Where(p => p.UniqueId == projectuniqueid)
                    .Select(p => p.ProjectName).FirstOrDefault();

                ViewBag.ProjectName = projectname;

                var useremailfreelancer = db.Users.Where(u => u.Id == inviteduserid).Select(u => u.Email).FirstOrDefault();

                ViewBag.UserEmail = useremailfreelancer;

                var listOfSpecificImagesProofs = db.ImageProofs.ToList()
                    .Where(j => j.ProjectUniqueId == projectuniqueid)
                    .Where(s => s.UploadByUserId == inviteduserid);

                var RecordsWithMaxMouseClicksTotal = listOfSpecificImagesProofs
      .GroupBy(x => x.DesktopUniqueSession,
               (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

                var Data = RecordsWithMaxMouseClicksTotal.Select(s => new
                {
                    TheDate = s.UploadedDateWithoutTime,
                    TheValue = s.MouseClicks

                });


                var Datax = Data.GroupBy(r => r.TheDate)
    .Select(
        g => new
        {
            date = g.Key,
            mouseclicks = g.Sum(s => s.TheValue),
        });

                if (Datax.Count() > 0)
                {


                    var min_mouseclicks = Datax.Select(d => d.mouseclicks).Min();
                    var max_mouseclicks = Datax.Select(d => d.mouseclicks).Max();

                    ViewBag.MinMouseClicks = min_mouseclicks;
                    ViewBag.MaxMouseClicks = max_mouseclicks;


                    var Dataxkeys = Data.GroupBy(r => r.TheDate)
        .Select(
        g => new
        {
            date = g.Key,
            keypresses = g.Sum(s => s.TheValue),
        });


                    var max_keypresses = Dataxkeys.Select(d => d.keypresses).Max();


                    ViewBag.MaxKeyPresses = max_keypresses;

                    return View();
                }

                else
                {
                    return Content("No Reports Yet !");
                }
            }




            else
            {
                return Content("Non Authorized Access !");
            }
        }



        public JsonResult MouseClicksData(string empid, int proid)
        {
            //var actionid = Url.RequestContext.RouteData.Values["id"];

            //int id = Convert.ToInt32(actionid);

            var inviteduserid = empid;

            var projectuniqueid = db.Projects.Where(u => u.Id == proid)
                .Select(p => p.UniqueId).FirstOrDefault();

            var listOfSpecificImagesProofs = db.ImageProofs.ToList()
                .Where(j => j.ProjectUniqueId == projectuniqueid)
                .Where(s => s.UploadByUserId == inviteduserid);

            var RecordsWithMaxMouseClicksTotal = listOfSpecificImagesProofs
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

            var Data = RecordsWithMaxMouseClicksTotal.Select(s => new
            {
                TheDate = s.UploadedDateWithoutTime,
                TheValue = s.MouseClicks

            });


            var Datax = Data.GroupBy(r => r.TheDate)
.Select(
    g => new
    {
        date = g.Key,
        mouseclicks = g.Sum(s => s.TheValue),
    });


            var dataobject = Datax;
            var chartData = new object[dataobject.Count() + 1];
            chartData[0] = new object[]{
       "date",
       "mouseclick"
     };
            int w = 0;
            foreach (var i in dataobject)
            {
                w++;
                chartData[w] = new object[] { i.date, i.mouseclicks };
            }


            return Json(chartData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult TimerJsonEmployerSide(string empid, int proid)
        {

            var employerid = User.Identity.GetUserId();


            //var actionid = Url.RequestContext.RouteData.Values["id"];

            //int id = Convert.ToInt32(actionid); //id from InvitationInboxes Table.


            //string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

            string projectsharedid = db.Projects.Where(i => i.Id == proid)
               .Select(i => i.UniqueId).FirstOrDefault();

            //string FreeLancerUserId = db.InvitationInboxes.Where(i => i.Id == id)
            //    .Select(i => i.SentToUserId).FirstOrDefault();

            string FreeLancerUserId = empid;

            var ThisProjectThisUserList = db.ImageProofs
                .Where(i => i.ProjectUniqueId == projectsharedid)
                .Where(i => i.UploadByUserId == FreeLancerUserId);



            var lista = ThisProjectThisUserList.ToList()
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

            var listb = lista.Select(a => new
            {
                thedate = a.UploadedDateWithoutTime,
                desktoptimer = myfunctions.ToSeconds(a.DesktopTimer)

            });



            var Datax = listb.GroupBy(r => r.thedate)
.Select(
g => new
{
    date = g.Key,
    desktoptimerx = myfunctions.ToMinutes(g.Sum(s => s.desktoptimer)),
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


        public JsonResult KeyPressesData(string empid, int proid)
        {

            var actionid = Url.RequestContext.RouteData.Values["id"];

            int id = Convert.ToInt32(actionid);

            var inviteduserid = empid;

            var projectuniqueid = db.Projects.Where(u => u.Id == proid)
                .Select(p => p.UniqueId).FirstOrDefault();


            var projectname = db.Projects.Where(p => p.UniqueId == projectuniqueid)
                .Select(p => p.ProjectName).FirstOrDefault();

            ViewBag.ProjectName = projectname;

            var useremailfreelancer = db.Users.Where(u => u.Id == inviteduserid).Select(u => u.Email).FirstOrDefault();

            ViewBag.UserEmail = useremailfreelancer;

            var listOfSpecificImagesProofs = db.ImageProofs.ToList()
                .Where(j => j.ProjectUniqueId == projectuniqueid)
                .Where(s => s.UploadByUserId == inviteduserid);

            var RecordsWithMaxKeyPressesTotal = listOfSpecificImagesProofs
  .GroupBy(x => x.DesktopUniqueSession,
           (k, g) => g.Aggregate((a, x) => (x.KeyBoardHits > a.KeyBoardHits) ? x : a));

            var Data = RecordsWithMaxKeyPressesTotal.Select(s => new
            {
                TheDate = s.UploadedDateWithoutTime,
                TheValue = s.KeyBoardHits

            });


            var Datax = Data.GroupBy(r => r.TheDate)
.Select(
    g => new
    {
        date = g.Key,
        keypresses = g.Sum(s => s.TheValue),
    });


            var dataobject = Datax;
            var chartData = new object[dataobject.Count() + 1];
            chartData[0] = new object[]{
       "date",
       "keypresses"
     };
            int w = 0;
            foreach (var i in dataobject)
            {
                w++;
                chartData[w] = new object[] { i.date, i.keypresses };
            }


            return Json(chartData, JsonRequestBehavior.AllowGet);


        }






        // GET: ImageProof
        public ActionResult Index()
        {
            return View(db.ImageProofs.ToList());
        }

        // GET: ImageProof/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageProof imageProof = db.ImageProofs.Find(id);
            if (imageProof == null)
            {
                return HttpNotFound();
            }
            return View(imageProof);
        }

        // GET: ImageProof/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageProof/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageFileName,UploadByUserId,ProjectUniqueId,UploadedToUserId,UploadedDate,UploadedDateWithoutTime,KeyBoardHits,MouseClicks,DesktopUniqueSession,DesktopTimer")] ImageProof imageProof)
        {
            if (ModelState.IsValid)
            {
                db.ImageProofs.Add(imageProof);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imageProof);
        }

        // GET: ImageProof/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageProof imageProof = db.ImageProofs.Find(id);
            if (imageProof == null)
            {
                return HttpNotFound();
            }
            return View(imageProof);
        }

        // POST: ImageProof/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageFileName,UploadByUserId,ProjectUniqueId,UploadedToUserId,UploadedDate,UploadedDateWithoutTime,KeyBoardHits,MouseClicks,DesktopUniqueSession,DesktopTimer")] ImageProof imageProof)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imageProof).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imageProof);
        }

        // GET: ImageProof/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageProof imageProof = db.ImageProofs.Find(id);
            if (imageProof == null)
            {
                return HttpNotFound();
            }
            return View(imageProof);
        }

        // POST: ImageProof/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImageProof imageProof = db.ImageProofs.Find(id);
            db.ImageProofs.Remove(imageProof);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
