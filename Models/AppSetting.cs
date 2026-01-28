using System.ComponentModel.DataAnnotations;

namespace MyJournal.Models
{
    public class AppSetting
    {
        public int Id { get; set; }

        [Required] public string Key { get; set; } = "";
        [Required] public string Value { get; set; } = "";
    }
}
