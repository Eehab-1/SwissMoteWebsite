using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SwissMoteWebsite.Models;

namespace SwissMoteWebsite.Controllers
{
    public class ImageProofController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
