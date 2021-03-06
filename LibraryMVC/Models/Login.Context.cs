﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OrisonSystemSCHOOLEntities : DbContext
    {
        public OrisonSystemSCHOOLEntities()
            : base("name=OrisonSystemSCHOOLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PortalUser> PortalUsers { get; set; }
        public virtual DbSet<School_LibraryBookMaster> School_LibraryBookMaster { get; set; }
        public virtual DbSet<School_LibraryBookDetails> School_LibraryBookDetails { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Library_Members> Library_Members { get; set; }
        public virtual DbSet<School_LibraryCategoryMaster> School_LibraryCategoryMaster { get; set; }
        public virtual DbSet<School_LibraryIssue> School_LibraryIssue { get; set; }
        public virtual DbSet<School_LibraryReturn> School_LibraryReturn { get; set; }
    
        public virtual ObjectResult<LibraryBookMasterListSP_Result> LibraryBookMasterListSP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookMasterListSP_Result>("LibraryBookMasterListSP");
        }
    
        public virtual ObjectResult<LibraryMembersListSP_Result> LibraryMembersListSP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryMembersListSP_Result>("LibraryMembersListSP");
        }
    
        public virtual ObjectResult<LibraryMemberDetailsSP_Result> LibraryMemberDetailsSP(Nullable<int> memberID, Nullable<int> accountID)
        {
            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(int));
    
            var accountIDParameter = accountID.HasValue ?
                new ObjectParameter("accountID", accountID) :
                new ObjectParameter("accountID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryMemberDetailsSP_Result>("LibraryMemberDetailsSP", memberIDParameter, accountIDParameter);
        }
    
        public virtual ObjectResult<LibrarySubMasterDetailsSP_Result> LibrarySubMasterDetailsSP(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibrarySubMasterDetailsSP_Result>("LibrarySubMasterDetailsSP", iDParameter);
        }
    
        public virtual ObjectResult<LibrarySubMasterListSP_Result> LibrarySubMasterListSP(string sHead)
        {
            var sHeadParameter = sHead != null ?
                new ObjectParameter("SHead", sHead) :
                new ObjectParameter("SHead", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibrarySubMasterListSP_Result>("LibrarySubMasterListSP", sHeadParameter);
        }
    
        public virtual ObjectResult<LibraryBookIssueListSP_Result> LibraryBookIssueListSP(string academicYear, string shift)
        {
            var academicYearParameter = academicYear != null ?
                new ObjectParameter("AcademicYear", academicYear) :
                new ObjectParameter("AcademicYear", typeof(string));
    
            var shiftParameter = shift != null ?
                new ObjectParameter("Shift", shift) :
                new ObjectParameter("Shift", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookIssueListSP_Result>("LibraryBookIssueListSP", academicYearParameter, shiftParameter);
        }
    
        public virtual ObjectResult<LibraryGetIssueDetails_Result> LibraryGetIssueDetails(string academicYear, string shift, Nullable<int> issueID)
        {
            var academicYearParameter = academicYear != null ?
                new ObjectParameter("AcademicYear", academicYear) :
                new ObjectParameter("AcademicYear", typeof(string));
    
            var shiftParameter = shift != null ?
                new ObjectParameter("Shift", shift) :
                new ObjectParameter("Shift", typeof(string));
    
            var issueIDParameter = issueID.HasValue ?
                new ObjectParameter("IssueID", issueID) :
                new ObjectParameter("IssueID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryGetIssueDetails_Result>("LibraryGetIssueDetails", academicYearParameter, shiftParameter, issueIDParameter);
        }
    
        public virtual ObjectResult<LibraryBookMasterDetailsSP_Result> LibraryBookMasterDetailsSP(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookMasterDetailsSP_Result>("LibraryBookMasterDetailsSP", iDParameter);
        }
    
        public virtual ObjectResult<LibraryBookCopyDetailsSP_Result> LibraryBookCopyDetailsSP(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookCopyDetailsSP_Result>("LibraryBookCopyDetailsSP", iDParameter);
        }
    
        public virtual ObjectResult<LibraryBookReturnDetailsSP_Result> LibraryBookReturnDetailsSP(Nullable<int> returnID, string academicYear)
        {
            var returnIDParameter = returnID.HasValue ?
                new ObjectParameter("ReturnID", returnID) :
                new ObjectParameter("ReturnID", typeof(int));
    
            var academicYearParameter = academicYear != null ?
                new ObjectParameter("AcademicYear", academicYear) :
                new ObjectParameter("AcademicYear", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookReturnDetailsSP_Result>("LibraryBookReturnDetailsSP", returnIDParameter, academicYearParameter);
        }
    
        public virtual ObjectResult<LibraryBookReturnListSP_Result> LibraryBookReturnListSP(string academicYear, string shift, Nullable<System.DateTime> date)
        {
            var academicYearParameter = academicYear != null ?
                new ObjectParameter("AcademicYear", academicYear) :
                new ObjectParameter("AcademicYear", typeof(string));
    
            var shiftParameter = shift != null ?
                new ObjectParameter("Shift", shift) :
                new ObjectParameter("Shift", typeof(string));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LibraryBookReturnListSP_Result>("LibraryBookReturnListSP", academicYearParameter, shiftParameter, dateParameter);
        }
    }
}
