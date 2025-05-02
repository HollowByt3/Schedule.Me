using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c969
{
    internal class appointment
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string contact { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public DateTime Start { get; set; }
        public DateTime end { get; set; }
        public int Customer_ID { get; set; }
        public static BindingList<appointment> appointments { get;private set; }
    }
}
