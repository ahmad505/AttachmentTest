using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchiceTest.Models.Entities;

namespace ArchiceTest.Models
{
    public class Context :DbContext
    {
         public Context(DbContextOptions<Context> options):base(options)
        {

        }
        
        public DbSet<Attachment> attachments { get; set; }
    }
}
