using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsStateMst
    {
        public byte StateID { get; set; }
        [Required(ErrorMessage="State Name Rerquired...")]
        public string StateName { get; set; }
    }
}