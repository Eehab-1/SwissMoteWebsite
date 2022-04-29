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


        public ActionResult Show(int? page , string empid , int prounid)
        {


            var actionid = Url.RequestContext.RouteData.Values["id"];

            //int id = Convert.ToInt32(actionid);

            string emp_userid = actionid.ToString();

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



                string projectuniqueid = db.Projects.Where(p => p.Id == prounid)
                    .Select(p => p.UniqueId).FirstOrDefault();
                    

                var listOfSpecificImagesProofs = db.ImageProofs.ToList()
                    .Where(j => j.ProjectUniqueId == projectuniqueid)
                    .Where(s => s.UploadByUserId == inviteduserid);

                // Get Project Name and User Email and send them to view :

                var UserEmail = db.Projects.Where(d => d.Invited_UserId == inviteduserid)
                    .Select(u => u.Invited_Email_UserName).FirstOrDefault();

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



       // public ActionResult Show(int? page)
       // {


       //     var actionid = Url.RequestContext.RouteData.Values["id"];

       //     //int id = Convert.ToInt32(actionid);

       //     string emp_userid = actionid.ToString();

       //     //var inviteduserid = db.ProjectInvitations.Where(u => u.Id == id)
       //     //    .Select(d => d.Invited_UserId).FirstOrDefault();

       //     var inviteduserid = emp_userid;



       //     string creatoruserid = db.Teams.Where(a => a.TeamMemberUserId == emp_userid)
       //         .Select(u => u.TeamCreatedByUserId).FirstOrDefault();

       //     string currentuserid = User.Identity.GetUserId();

       //     if (currentuserid == creatoruserid)
       //     {


       //         //var projectuniqueid = db.Projects.Where(u => u.Id == id)
       //         //    .Select(p => p.UniqueId).FirstOrDefault();

       //         var projectuniqueid = TempData["projectuniqueid"];

       //         var listOfSpecificImagesProofs = db.ImageProofs.ToList()
       //             .Where(j => j.ProjectUniqueId == projectuniqueid)
       //             .Where(s => s.UploadByUserId == inviteduserid);

       //         // Get Project Name and User Email and send them to view :

       //         var UserEmail = db.Projects.Where(d => d.Invited_UserId == inviteduserid)
       //             .Select(u => u.Invited_Email_UserName).FirstOrDefault();

       //         ViewBag.EmailUserName = UserEmail;

       //         var ProjectName = db.Projects.Where(d => d.UniqueId == projectuniqueid)
       //            .Select(u => u.ProjectName).FirstOrDefault();

       //         ViewBag.ProjectName = ProjectName;


       //         var TodayDate = listOfSpecificImagesProofs.Select(s => s.UploadedDateWithoutTime).LastOrDefault();


       //         var AllSessionsTodayList = listOfSpecificImagesProofs
       //             .Where(d => d.UploadedDateWithoutTime == TodayDate);

       //         var RecordsWithMaxMouseClicksToday = AllSessionsTodayList
       // .GroupBy(x => x.DesktopUniqueSession,
       //          (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

       //         var TodayMouseClicks = RecordsWithMaxMouseClicksToday.Select(m => m.MouseClicks).Sum();
       //         ViewBag.TodayMouseClicks = TodayMouseClicks;


       //         var RecordsWithMaxKeyboardHitsToday = AllSessionsTodayList
       //.GroupBy(x => x.DesktopUniqueSession,
       //         (k, g) => g.Aggregate((a, x) => (x.KeyBoardHits > a.KeyBoardHits) ? x : a));

       //         var TodayKeyboardHits = RecordsWithMaxKeyboardHitsToday.Select(k => k.KeyBoardHits).Sum();

       //         ViewBag.TodayKeyPresses = TodayKeyboardHits;



       //         var RecordsWithMaxMouseClicksTotal = listOfSpecificImagesProofs
       //.GroupBy(x => x.DesktopUniqueSession,
       //         (k, g) => g.Aggregate((a, x) => (x.MouseClicks > a.MouseClicks) ? x : a));

       //         var TotalMouseClicks = RecordsWithMaxMouseClicksTotal.Select(m => m.MouseClicks).Sum();

       //         ViewBag.TotalMouseClicks = TotalMouseClicks;




       //         var RecordsWithMaxKeyboardHitsTotal = listOfSpecificImagesProofs
       //         .GroupBy(x => x.DesktopUniqueSession,
       //       (k, g) => g.Aggregate((a, x) => (x.KeyBoardHits > a.KeyBoardHits) ? x : a));

       //         var TotalKeyboardHits = RecordsWithMaxKeyboardHitsTotal.Select(k => k.KeyBoardHits).Sum();




       //         ViewBag.TotalKeyPresses = TotalKeyboardHits;




       //         var latestlist = listOfSpecificImagesProofs.OrderByDescending(u => u.UploadedDate);


       //         return View(latestlist.ToList()
       //                 .ToPagedList(page ?? 1, 4));
       //     }

       //     else
       //     {


       //         return Content("Non Authorized Access");

       //     }
       // }

















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
