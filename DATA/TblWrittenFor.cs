//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblWrittenFor
    {
        public int Summary_Num { get; set; }
        public int Treatment_Id { get; set; }
        public string @new { get; set; }
    
        public virtual TblSummary TblSummary { get; set; }
        public virtual TblTreatment TblTreatment { get; set; }
    }
}