//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjSurveyPro
{
    using System;
    using System.Collections.Generic;
    
    public partial class SurveyAn
    {
        public short SurveyID { get; set; }
        public byte CategoryID { get; set; }
        public short QuestionID { get; set; }
        public short UserID { get; set; }
        public byte Answer { get; set; }
    
        public virtual CategoryMst CategoryMst { get; set; }
        public virtual SurveyQue SurveyQue { get; set; }
        public virtual SurveyMst SurveyMst { get; set; }
        public virtual UserMst UserMst { get; set; }
    }
}
