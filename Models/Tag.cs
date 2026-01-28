using System.ComponentModel.DataAnnotations;

namespace MyJournal.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = "";

        public bool IsPrebuilt { get; set; } = true;

        public List<EntryTag> EntryTags { get; set; } = new();
    }
}
