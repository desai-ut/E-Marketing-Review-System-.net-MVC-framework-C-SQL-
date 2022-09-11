using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsFeedbackMst
    {
        public int FeedbackID { get; set; }
        public Nullable<short> SurveyID { get; set; }
        public string FeedbackDisc { get; set; }
        [Required(ErrorMessage="Rate the product..")]
        public byte Ratings { get; set; }
        [Required(ErrorMessage="User Email Required...")]
        public string UserEmail { get; set; }
        public string UserContactNo { get; set; }
    }
}