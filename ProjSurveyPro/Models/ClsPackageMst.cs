using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsPackageMst
    {
        public byte PackageID { get; set; }
        [Required(ErrorMessage="Name of Package Required...")]
        public string PackageTitle { get; set; }
        [Required(ErrorMessage = "Package Description Required...")]
        public string Discription { get; set; }
        [Required(ErrorMessage = "Price of Package Required...")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Select Duration of Package...")]
        public byte Duration { get; set; }
        [Required(ErrorMessage = "Enter Max Number of Product can be Entered by the Client...")]
        public byte NoOfProduct { get; set; }
        public Nullable<byte> NoOfSurveyQues { get; set; }
        public Nullable<byte> NoOfReviewQues { get; set; }
        public Nullable<short> NoOfResponse { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<byte> EmpID { get; set; }
    }
}