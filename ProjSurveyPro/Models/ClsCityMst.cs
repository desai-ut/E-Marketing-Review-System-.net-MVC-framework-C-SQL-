using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsCityMst
    {
        public byte StateID { get; set; }
        public byte CityID { get; set; }
        [Required(ErrorMessage="City Name Required...")]
        public string CityName { get; set; }
    }
}