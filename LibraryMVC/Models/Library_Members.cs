//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Library_Members
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ContactNum { get; set; }
        public string ContactNumAlt { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string Nationality { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Area { get; set; }
        public string Emirates { get; set; }
        public string Status { get; set; }
        public byte[] Image { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> AccountID { get; set; }
    }
}
