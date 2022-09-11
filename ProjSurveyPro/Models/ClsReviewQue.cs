using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsReviewQue
    {
        public short SurveyID { get; set; }
        public byte CategoryID { get; set; }
        public short QuestionID { get; set; }
        [Required(ErrorMessage="Question For Review Required...")]
        public string Question { get; set; }
    }
}