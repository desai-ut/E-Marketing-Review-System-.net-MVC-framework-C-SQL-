using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsCategoryMst
    {
        public byte CategoryID { get; set; }
        [Required(ErrorMessage="Category Name Required...")]
        public string CategoryDisc { get; set; }
    }
}