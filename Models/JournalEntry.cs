using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Content { get; set; } = string.Empty;

        public string PrimaryMood { get; set; } = string.Empty;
        public string? SecondaryMood1 { get; set; }
      

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        

    }
}
