using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsQueryMst
    {
        public int QueryID { get; set; }
        public string QueryDisc { get; set; }
        [Required(ErrorMessage="User Email-Id Required...")]
        public string EmailID { get; set; }
        public Nullable<bool> ReplyStatus { get; set; }
    }
}