using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryMVC.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LibraryMVC.DAL
{
    public class LibraryContext:DbContext
    {
        public LibraryContext() : base("LibraryContext")
        {

        }
        public DbSet<User> Users { get; set; }

    }
}