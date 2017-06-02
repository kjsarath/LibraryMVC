using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    [MetadataType(typeof(School_LibraryBookMasterMetaData))]
    public partial class School_LibraryBookMaster
    {
    }

    public class School_LibraryBookMasterMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookMastID { get; set; }
    }
}