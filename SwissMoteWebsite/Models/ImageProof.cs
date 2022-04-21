using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Models
{
    public class ImageProof
    {


        public int Id { get; set; }

        [Display(Name = "Image Name")]
        public string ImageFileName { get; set; }

        public string UploadByUserId { get; set; }

        public string ProjectUniqueId { get; set; }

        public string UploadedToUserId { get; set; }

        [Display(Name = "Screenshot Date")]
        public DateTime? UploadedDate { get; set; }

        public string UploadedDateWithoutTime { get; set; }

        public int KeyBoardHits { get; set; }


        public int MouseClicks { get; set; }


        public string DesktopUniqueSession { get; set; }

        public string DesktopTimer { get; set; }











    }
}