using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SwissMoteWebsite.Models;

namespace SwissMoteWebsite.Controllers
{

    [Authorize]


    public class TeamController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


       public ActionResult Online()
        {
            return View();
        }



        // GET: Team
        public ActionResult Index()
        {
            return View(db.Teams.ToList());
        }


        public ActionResult InviteMore(int id)
        {

            string userid = User.Identity.GetUserId();

            string username = User.Identity.GetUserName();

            string TeamCreatedByUserId = db.Teams.Where(a => a.TeamId == id)
                .Select(a => a.TeamCreatedByUserId).FirstOrDefault();

            string theuniqueid = db.Teams.Where(a => a.TeamId == id)
                .Select(a => a.TeamUniqueId).FirstOrDefault();

            string TeamName = db.Teams.Where(a => a.TeamId == id)
                .Select(a => a.TeamName).FirstOrDefault();

            if (userid == TeamCreatedByUserId)
            {


                Team team = new Team
                {
                    TeamId = id ,
                    TeamName = TeamName ,
                    TeamCreatedByUserId=TeamCreatedByUserId,
                    TeamInsights=0,
                    MemberInsights=0,
                    TeamMember="",
                    MemberHourlyRate=0,
                    MemberChatKey="",
                    TeamStatus=false ,
                    MemberStatus=false,
                    ClientName=username,
                    TeamUniqueId=theuniqueid
                };


                return View(team);
            }

            else
            {

                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult InviteMore(Team team)
        {


            if (ModelState.IsValid)

            {




                db.Teams.Add(team);
                db.SaveChanges();
                ViewBag.Message = team.TeamMember + " invited Successfully.";
               
                return View(team);


            }


            return View(team);

        }











        // GET: Team/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,TeamCreatedByUserId,TeamName,TeamMember,TeamInsights,MemberInsights,MemberHourlyRate,TeamStatus,MemberStatus,ClientName,MemberChatKey")] Team team)
        {
            if (ModelState.IsValid)
            {
                var theuniqueid = Guid.NewGuid().ToString();

                string userid = User.Identity.GetUserId();
                string username = User.Identity.GetUserName();

                team.TeamCreatedByUserId = userid;
                team.ClientName = username;
                team.TeamUniqueId = theuniqueid;

                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamCreatedByUserId,TeamName,TeamMember,TeamInsights,MemberInsights,MemberHourlyRate,TeamStatus,MemberStatus,ClientName,MemberChatKey")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Team/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
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
