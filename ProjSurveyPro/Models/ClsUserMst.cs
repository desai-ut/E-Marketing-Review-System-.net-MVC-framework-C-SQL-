using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsUserMst
    {
        public short UserID { get; set; }
        [Required(ErrorMessage="User Name Required...")]
        public string UserName { get; set; }
        public bool Gender { get; set; }
        [Required(ErrorMessage = "User Address Required...")]
        public string UserAddress { get; set; }
        [Required(ErrorMessage = "Select your State...")]
        public Nullable<byte> StateID { get; set; }
        [Required(ErrorMessage = "Select Your City...")]
        public Nullable<byte> CityID { get; set; }
        [Required(ErrorMessage = "User Mobile No. Required...")]
        [MaxLength(10,ErrorMessage="Mobile No Must be of 10 Digits...")]
        [MinLength(10, ErrorMessage = "Mobile No Must be of 10 Digits...")]
        public string MobNo { get; set; }
        [Required(ErrorMessage = "User Email-ID Required...")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "User DOB Required...")]
        public Nullable<System.DateTime> DtBirth { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public byte[] Passwd { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> ActiveLeaveDate { get; set; }
        public string UserImage { get; set; }
        public Nullable<System.DateTime> LastLoginDt { get; set; }
        public Nullable<short> RedeemRewardPoint { get; set; }
        public Nullable<byte> ApprovedEmpID { get; set; }
    }
}