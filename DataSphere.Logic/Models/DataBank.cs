using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSphere.Logic.Models
{
    public class DataBank
    {
        [Display(Name = "DataID")]
        public string? DataId { get; set; }
        [Display(Name = "DataKeyword")]
        public string? DataKeyWord { get; set; }
        [Display(Name = "DataContext")]
        public string? DataContext { get; set; }
    }
}
