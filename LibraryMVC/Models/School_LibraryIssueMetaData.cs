﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryMVC.Models
{
    [MetadataType(typeof(School_LibraryIssueMetaData))]
    public partial class School_LibraryIssue
    {

    }
    public class School_LibraryIssueMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity )]
        public int ID { get; set; }
    }
}