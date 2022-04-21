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
using PagedList;

namespace SwissMoteWebsite.Controllers
{
    public class InvoiceController : Controller
    {




        private ApplicationDbContext db = new ApplicationDbContext();

        MyFunctions myfunctions = new MyFunctions();




        public ActionResult Navigate()
        {


            return View();
        }



        // GET: Invoice
        public ActionResult Index(int? page)
        {
            var userid = User.Identity.GetUserId();

            var invoices = db.Invoices.Where(i => i.FromUserId == userid).ToList();

            return View(invoices.ToPagedList(page ?? 1, 2));
        }


        public ActionResult Received(int? page)
        {

            var userid = User.Identity.GetUserId();

            var invoices = db.Invoices.Where(i => i.ToUserId == userid).ToList();

            return View(invoices.ToPagedList(page ?? 1, 2));


        }





        public ActionResult Test()
        {
            return View();
        }



        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            string currentuserid = User.Identity.GetUserId();

            string invoiceuserid = db.Invoices.Where(a => a.Id == id).Select(a => a.ToUserId).FirstOrDefault();

            string invoiceworker = db.Invoices.Where(a => a.Id == id).Select(a => a.FromUserId).FirstOrDefault();

            if ((currentuserid == invoiceuserid) || (currentuserid == invoiceworker))

            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Invoice invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return HttpNotFound();
                }

                var invoicefrom = db.Invoices.Where(i => i.Id == id).Select(i => i.FromUserId).FirstOrDefault();

                string chatkey = db.Users.Where(u => u.Id == invoicefrom).Select(u => u.ChatKey).FirstOrDefault();

                ViewBag.ChatKey = chatkey;

                return View(invoice);

            }

            else
            {
                return Content("Non Authorized content");
            }
        }

        public ActionResult PaymentInfo(string chatkey)
        {
            string currentuserid = User.Identity.GetUserId();

            var thechatkey = db.Users.Where(d => d.Id == currentuserid).Select(d => d.ChatKey).FirstOrDefault();

            if (chatkey == thechatkey)
            {
                var userrow = db.Users.Where(d => d.ChatKey == chatkey).FirstOrDefault();

                return View(userrow);
            }

            else
            {
                return View();
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult PaymentInfo(ApplicationUser userrow)
        {
            var theuserrow = db.Users.Where(u => u.ChatKey == userrow.ChatKey).FirstOrDefault();

            theuserrow.FullPaymentName = userrow.FullPaymentName;
            theuserrow.Country = userrow.Country;
            theuserrow.City = userrow.City;
            theuserrow.Address = userrow.Address;
            theuserrow.MobileNumber = userrow.MobileNumber;
            theuserrow.BankName = userrow.BankName;
            theuserrow.IBAN = userrow.IBAN;
            theuserrow.PayPalEmail = userrow.PayPalEmail;

            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult PaymentInfoShow(string chatkey)
        {
            var invoiceIssuer = db.Users.Where(u => u.ChatKey == chatkey).Select(d => d.Id).FirstOrDefault();

            var currentuserid = User.Identity.GetUserId();

            var invoicesentto = db.Invoices.Where(v => v.FromUserId == invoiceIssuer)
                .Where(a => a.ToUserId == currentuserid).Select(a => a.ToUserId).FirstOrDefault();



            if ((currentuserid == invoiceIssuer) || (currentuserid == invoicesentto))
            {
                var paymentinfo = db.Users.Where(u => u.Id == invoiceIssuer).FirstOrDefault();

                return View(paymentinfo);
            }

            else
            {
                return View();
            }

        }

        // GET: Invoice/Create
        //public ActionResult Create()
        //{

        //    var actionid = Url.RequestContext.RouteData.Values["id"];

        //    int id = Convert.ToInt32(actionid);


        //    string currentuserid = User.Identity.GetUserId();

        //    string workeruserid = db.InvitationInboxes.Where(p => p.Id == id)
        //        .Select(f => f.SentToUserId).FirstOrDefault();

        //    if (currentuserid == workeruserid)
        //    {


        //        var projectname = db.InvitationInboxes.Where(p => p.Id == id)
        //            .Select(p => p.ProjectName).FirstOrDefault();

        //        var projectuniqueid = db.InvitationInboxes.Where(p => p.Id == id)
        //            .Select(p => p.ProjectInvitationUniqueId).FirstOrDefault();

        //        ViewBag.ProjectName = projectname;


        //        Invoice invoice = new Invoice
        //        {
        //            ProjectName = projectname,
        //            ProjectSharedUniqueId = projectuniqueid

        //        };


        //        return View(invoice);

        //    }

        //    else
        //    {
        //        return View();
        //    }

        //}

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ProjectName,InvoiceFromUser,FromUserId,InvoiceToUser,ToUserId,TotalHours,HourlyRate,FromDate,ToDate,CreationDate,IsPaid,ProjectSharedUniqueId")] Invoice invoice)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userid = User.Identity.GetUserId();

        //        var username = User.Identity.GetUserName();

        //        string projectshareduniqueid = invoice.ProjectSharedUniqueId;

        //        var touserid = db.ProjectInvitations.Where(p => p.UniqueId == projectshareduniqueid)
        //            .Select(p => p.CreatedByUserId).FirstOrDefault();

        //        int theid = db.InvitationInboxes.Where(i => i.ProjectInvitationUniqueId == projectshareduniqueid)
        //            .Where(i => i.SentToUserId == userid).Select(i => i.Id).FirstOrDefault();

        //        var invoicetouser = db.Users.Where(u => u.Id == touserid).Select(u => u.UserName).FirstOrDefault();

        //        invoice.InvoiceFromUser = username;
        //        invoice.FromUserId = userid;
        //        invoice.ToUserId = touserid;
        //        invoice.InvoiceToUser = invoicetouser;
        //        invoice.CreationDate = DateTime.UtcNow;
        //        invoice.TotalHours = TotalHours(invoice.FromDate, invoice.ToDate, theid);
        //        invoice.TotalPayment = invoice.HourlyRate * invoice.TotalHours;

        //        db.Invoices.Add(invoice);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(invoice);
        //}





//        public double TotalHours(string fromdate, string todate, int theid)
//        {

//            string s1 = fromdate;
//            DateTime dfrom = Convert.ToDateTime(s1);

//            string s2 = todate;
//            DateTime dto = Convert.ToDateTime(s2);



//            var userid = User.Identity.GetUserId();



//            int id = theid;

//            string projectsharedid = db.InvitationInboxes.Where(i => i.Id == id)
//                .Select(i => i.ProjectInvitationUniqueId).FirstOrDefault();

//            var ThisProjectThisUserList = db.ImageProofs
//                .Where(i => i.ProjectUniqueId == projectsharedid)
//                .Where(i => i.UploadByUserId == userid);



//            var lista = ThisProjectThisUserList.ToList()
//  .GroupBy(x => x.DesktopUniqueSession,
//           (k, g) => g.Aggregate((a, x) => (x.DesktopTimer != a.DesktopTimer) ? x : a));

//            var listb = lista.Select(a => new
//            {
//                thedate = a.UploadedDateWithoutTime,
//                desktoptimer = myfunctions.ToSeconds(a.DesktopTimer)

//            });



//            var Datab = listb.GroupBy(r => r.thedate)
//.Select(
//g => new
//{
//    date = myfunctions.ChangeDateFormat(g.Key),
//    desktoptimerx = myfunctions.ToMinutes(g.Sum(s => s.desktoptimer)),
//});

//            var Datax = listb.GroupBy(r => r.thedate)
//.Select(
//g => new
//{
//    date = myfunctions.ToDateType(g.Key),
//    desktoptimerx = myfunctions.ToMinutes(g.Sum(s => s.desktoptimer)),
//});

//            var TotalMinutes = Datax.Where(d => d.date >= dfrom).Where(d => d.date <= dto)
//                .Select(d => d.desktoptimerx).Sum();

//            var TotalHours = (TotalMinutes / 60);

//            var hoursatrange = Math.Round(TotalHours, 2);

//            return hoursatrange;

//        }














        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            string currentuserid = User.Identity.GetUserId();

            string invoiceuserid = db.Invoices.Where(a => a.Id == id).Select(a => a.ToUserId).FirstOrDefault();

            if (currentuserid == invoiceuserid)
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Invoice invoice = db.Invoices.Find(id);
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                return View(invoice);

            }

            else
            {
                return Content("Non Authorized Access");
            }
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectName,InvoiceFromUser,FromUserId,InvoiceToUser,ToUserId,TotalHours,HourlyRate,FromDate,ToDate,CreationDate,IsPaid,ProjectSharedUniqueId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Received");
            }
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
