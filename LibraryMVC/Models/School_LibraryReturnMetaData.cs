using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    [MetadataType(typeof(School_LibraryReturnMetaData))]
    public partial class School_LibraryReturn
    {
    }

    public class School_LibraryReturnMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity )]
        public int ID { get; set; }
    }
}