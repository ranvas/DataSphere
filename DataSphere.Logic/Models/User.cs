using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic.Models
{
    public class User
    {
        [Display(Name = "TGID")]
        public string? TgId { get; set; }
        [Display(Name = "TGUser")]
        public string? TgUser { get; set; }
        [Display(Name = "CharName")]
        public string? CharName { get; set; }
    }
}
