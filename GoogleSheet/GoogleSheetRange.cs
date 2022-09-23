﻿using GoogleSheet.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet
{
    public class GoogleSheetRange: IGoogleSheetRange
    {
        public string? List { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public string? StartColumn { get; set; }
        public string? EndColumn { get; set; }
        public override string ToString()
        {
            return $"{List}!{StartColumn}{StartRow}:{EndColumn}{EndRow}";
        }
    }
}
