using SwissMoteWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SwissMoteWebsite.Controllers
{


    public class UploadController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();



        //public string UserId()
        //{
        //    string prouniqueidtrim = "34ade66c-f257-4062-b3e7-9260b7ad5dbb";

        //    string apikeytrim = "12f6f205-adca-49bd-affe-a4537d2c3e0c";

        //    string senttouserid = db.ProjectInvitations.Where(p => p.UniqueId == prouniqueidtrim)
        //        .Where(c => c.Invited_UserId == apikeytrim).Select(s => s.CreatedByUserId).FirstOrDefault();


        //    return senttouserid;
        //}

        public async Task<string> Image(string apikey, string projectuniqueid, string keypresscount, string mouseclickscount, string desktopsession, string desktoptimer)
        {

            // continue by inserting keypresscount , mouseclickscount To the table .


            string apikeytrim = apikey.Trim();

            string prouniqueidtrim = projectuniqueid.Trim();

            //  string userid = User.Identity.GetUserId();

            bool IsValidApiKey = db.Users.Any(u => u.Id == apikeytrim);

            //bool IsValidProuniqueId = db.InvitationInboxes.Any(p => p.ProjectInvitationUniqueId == prouniqueidtrim);

            bool IsValidProuniqueId = db.Projects.Any(p => p.UniqueId == prouniqueidtrim);


            if (IsValidApiKey && IsValidProuniqueId)
            {
                //string senttouserid = db.ProjectInvitations.Where(p => p.UniqueId == prouniqueidtrim)
                //.Where(c => c.Invited_UserId == apikeytrim).Select(s => s.CreatedByUserId).FirstOrDefault();

                string senttouserid = db.Projects.Where(p => p.UniqueId == projectuniqueid)
                    .Select(p => p.CreatedByUserId).FirstOrDefault();


                ImageProof imageproof = new ImageProof();

                // find sendto userid 






                var ctx = HttpContext.Current;
                var root = ctx.Server.MapPath("~/UploadedFiles");
                var provider =
                    new MultipartFormDataStreamProvider(root);

                List<string> ImagesList = new List<string>();
                try
                {
                    await Request.Content
                        .ReadAsMultipartAsync(provider);

                    foreach (var file in provider.FileData)
                    {
                        var name = file.Headers
                            .ContentDisposition
                            .FileName;

                        // remove double quotes from string.
                        name = name.Trim('"');

                        var localFileName = file.LocalFileName;
                        var filePath = Path.Combine(root, name);

                        ImagesList.Add(name);

                        File.Move(localFileName, filePath);
                    }
                }
                catch (Exception e)
                {
                    return $"Error: {e.Message}";
                }

                //   return "File uploaded!";

                // check desktoptimer correct :

                bool desktoptiming = TimeSpan.TryParse(desktoptimer, out var mytime);

                if (desktoptiming)
                {


                    int keypresscountInt = Convert.ToInt32(keypresscount);
                    int mouseclickscountInt = Convert.ToInt32(mouseclickscount);


                    string uploadeddatewithouttime = DateTime.UtcNow.ToString("MM-dd-yyyy");
                    imageproof.ImageFileName = ImagesList[0];
                    imageproof.ProjectUniqueId = prouniqueidtrim;
                    imageproof.UploadByUserId = apikeytrim;
                    imageproof.UploadedToUserId = senttouserid;
                    imageproof.UploadedDate = DateTime.UtcNow;
                    imageproof.UploadedDateWithoutTime = uploadeddatewithouttime;
                    imageproof.KeyBoardHits = keypresscountInt;
                    imageproof.MouseClicks = mouseclickscountInt;
                    imageproof.DesktopUniqueSession = desktopsession;
                    imageproof.DesktopTimer = desktoptimer;
                    db.ImageProofs.Add(imageproof);
                    db.SaveChanges();


                    return ImagesList[0];
                }

                else
                {
                    return "Not Valid Time format , .e.g. valid format 00:00:00";
                }

            }


            else
            {
                return "Not Valid API Key or project Id";
            }
        }





    }




}