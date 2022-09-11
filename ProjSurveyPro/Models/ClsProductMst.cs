using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjSurveyPro.Models
{
    public class ClsProductMst
    {
        public short ClientID { get; set; }
        public short ProductID { get; set; }
        [Required(ErrorMessage="Product Name Required...")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Company Name Required...")]
        public string ProductCompany { get; set; }
        [Required(ErrorMessage = "Model Name Required...")]
        public string ModelName { get; set; }
        [Required(ErrorMessage = "Short Description Required...")]
        public string ShortDiscription { get; set; }
        [Required(ErrorMessage = "Brief Description Required...")]
        public string BriefDiscription { get; set; }
        public string Facility { get; set; }
        public Nullable<decimal> Price { get; set; }
        [Required(ErrorMessage = "Atleast one Product Image Required...")]
        public string ProductImage1 { get; set; }
        public string ProductImage2 { get; set; }
        public string ProductImage3 { get; set; }
        public string ProductImage4 { get; set; }
        public string Video { get; set; }
        public Nullable<decimal> TotalSurveyAmt { get; set; }
        public Nullable<decimal> SurveyPerAmount { get; set; }
        public Nullable<short> NoOfSurvey { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<short> SurveyRewardPoint { get; set; }
        public Nullable<bool> IsPaid { get; set; }
    }
}