using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finder.Models
{
    public class SearchModel
    {
        public string Extension { get; set; }

        public int Year { get; set; }

        public int[] Months { get; set; }
    }
}
