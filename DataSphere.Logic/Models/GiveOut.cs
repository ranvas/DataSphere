using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic.Models
{
    public class GiveOut
    {
        [Display(Name = "TGUser")]
        public string? TgUser { get; set; }
        [Display(Name = "Text")]
        public string? Text { get; set; }
        [Display(Name = "State")]
        public string? State { get; set; }
        [Display(Name = "Date")]
        public string? DateTime { get; set; }
        [Display(Name = "CharName")]
        public string? CharName { get; set; }
        [Display(Name = "Error")]
        public string? Error { get; set; }
    }
}
