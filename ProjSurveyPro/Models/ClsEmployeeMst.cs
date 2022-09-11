using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsEmployeeMst
    {
        public byte EmpID { get; set; }
        [Required(ErrorMessage = "Employee's Name Required...")]
        public string EmpName { get; set; }
        public bool Gender { get; set; }
        [Required(ErrorMessage = "Employee's Address Required...")]
        public string EmpAddress { get; set; }
        [Required(ErrorMessage = "Employee's Mobile No. Required...")]
        [MaxLength(10, ErrorMessage = "Mobile No.Must be of 10 DIgits...")]
        [MinLength(10, ErrorMessage = "Mobile No.Must be of 10 DIgits...")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Employee's Email-Id Required...")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Employee's Date of Birth Required...")]
        public Nullable<System.DateTime> DtBirth { get; set; }
        public System.DateTime JoiningDate { get; set; }
        [Required(ErrorMessage = "Employee's Designation Required...")]
        public string Designation { get; set; }
        public byte?[] Passwd { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> ActiveLeaveDt { get; set; }
        public string EmpImage { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
    }
}