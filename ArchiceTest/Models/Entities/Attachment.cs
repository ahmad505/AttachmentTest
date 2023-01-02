using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiceTest.Models.Entities
{
    public class Attachment
    {
        [Key]

        public int Id { get; set; }

        public string AttachPath { get; set; }
    }
}
