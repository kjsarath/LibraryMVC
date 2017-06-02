using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    [MetadataType(typeof(School_LibraryCategoryMasterMetaData))]
    public partial class School_LibraryCategoryMaster
    {

    }
    public class School_LibraryCategoryMasterMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity )]
        public int ID { get; set; }
        [DataType(DataType.Date )]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:dd/mm/yyyy}")]
        public Nullable<System.DateTime> ToDate { get; set; }
    }
}