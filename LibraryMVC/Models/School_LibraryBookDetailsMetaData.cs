using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    [MetadataType(typeof(School_LibraryBookDetailsMetaData))]
    public partial class School_LibraryBookDetails
    {
    }

    public class School_LibraryBookDetailsMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
    }
}