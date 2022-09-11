using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsClientMst
    {
        public short ClientID { get; set; }
        [Required(ErrorMessage="Company Name Required...")]
        public string CompName { get; set; }
        [Required(ErrorMessage = "Company's Address Required...")]
        public string CompAddress { get; set; }
        [Required(ErrorMessage = "Company's Owner Name Required...")]
        public string OwnerName { get; set; }
        public string CompanyLogo { get; set; }
        [Required(ErrorMessage = "Company's Contact No Required...")]
        [MaxLength(13, ErrorMessage = "Mobile No Must be of 13 Digits...")]
        [MinLength(11, ErrorMessage = "Mobile No Must be of 11 Digits...")]
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        public string GSTNo { get; set; }
        public string WebsiteName { get; set; }
        [Required(ErrorMessage = "Company's Email-Id Required")]
        public string EmailID { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public byte[] Passwd { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> ActiveLeaveDt { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
        public Nullable<byte> ApprovedEmpID { get; set; }
    }
}