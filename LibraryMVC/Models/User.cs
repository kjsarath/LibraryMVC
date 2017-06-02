using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.Models
{
    public class User
    {
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name ="User name")]
        [StringLength(50,MinimumLength =1)]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password )]
        [StringLength(100,MinimumLength =1)]
        public string Password { get; set; }

        [Display(AutoGenerateField =false)]
        public string Catregory { get; set; }

        public bool isValid(String un, String pwrd)
        {

            return false;
        }
    }
}