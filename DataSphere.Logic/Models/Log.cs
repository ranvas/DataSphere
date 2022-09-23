using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic.Models
{
    public class Log
    {
        [Display(Name = "Date")]
        public string? DateTime { get; set; }
        [Display(Name = "TGID")]
        public string? TgId { get; set; }
        [Display(Name = "Action")]
        public string? Action { get; set; }
        [Display(Name = "DataKeyword")]
        public string? DataKeyword { get; set; }
        [Display(Name = "result")]
        public string? Result { get; set; }
    }
}
