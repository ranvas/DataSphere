using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Model.DataSphereTest
{
    public class Example
    {
        [Display(Name = "Id")]
        public string? Id { get; set; }
        [Display(Name = "Имя")]
        public string? Name { get; set; }
        [Display(Name = "Комментарий")]
        public string? Comment { get; set; }
        [Display(Name = "Значение")]
        public string? Value { get; set; }
    }
}
