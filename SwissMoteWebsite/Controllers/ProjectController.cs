﻿using System;
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
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Project
        public ActionResult Index()
        {

            string userid = User.Identity.GetUserId();


            var projects = db.Projects.Include(p => p.Team);
            return View(projects.Where(a=>a.CreatedByUserId==userid).ToList());
        }


        public ActionResult Insights()
        {

            string userid = User.Identity.GetUserId();


            var projects = db.Projects.Include(p => p.Team);
            return View(projects.Where(a => a.CreatedByUserId == userid).ToList());
        }



        public ActionResult D1()
        {

            var projects = db.Projects.Include(p => p.Team);
            return View(projects.ToList());
        }



        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create

    


        public ActionResult Create()
        {
            string userid = User.Identity.GetUserId();

            var UniqueTeamName = db.Teams.GroupBy(x => x.TeamName).Select(x => x.FirstOrDefault());

            ViewBag.TeamId = new SelectList(UniqueTeamName.Where(a=>a.TeamCreatedByUserId== userid), "TeamId", "TeamName");
            return View();
        }


        public ActionResult Created()
        {
            string userid = User.Identity.GetUserId();

            ViewBag.TeamId = new SelectList(db.Teams.Where(a => a.TeamCreatedByUserId == userid), "TeamId", "TeamName");
            return View();
        }





        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectName,Note,CreatedByUserName,CreatedByUserId,CreationDate,Invited_Email_UserName,Invited_UserId,InvitationSentDate,InvitationAcceptedDate,invitedrole,invitationstatus,UniqueId,IsOn,TeamId")] Project project)
        {
            string userid = User.Identity.GetUserId();

            string username = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                var theuniqueid = Guid.NewGuid().ToString();

                project.CreatedByUserId = userid;
                project.CreatedByUserName = username;
                project.CreationDate = DateTime.Now;
                project.IsOn = true;
                project.UniqueId = theuniqueid;

                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams.Where(a => a.TeamCreatedByUserId == userid), "TeamId", "TeamName", project.TeamId);
            return View(project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            string userid = User.Identity.GetUserId();

            ViewBag.TeamId = new SelectList(db.Teams.Where(a => a.TeamCreatedByUserId == userid), "TeamId", "TeamName");

           // ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamCreatedByUserId", project.TeamId);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectName,Note,CreatedByUserName,CreatedByUserId,CreationDate,Invited_Email_UserName,Invited_UserId,InvitationSentDate,InvitationAcceptedDate,invitedrole,invitationstatus,UniqueId,IsOn,TeamId")] Project project)
        {
            if (ModelState.IsValid)
            {



                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "TeamCreatedByUserId", project.TeamId);
            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
