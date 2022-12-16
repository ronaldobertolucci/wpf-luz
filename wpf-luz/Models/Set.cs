using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_luz.Models
{
    public class Set
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Card_Count { get; set; }
        public string Set_Type { get; set; }
        public string Search_Uri { get; set; }
        public Set() { }
    }
}
