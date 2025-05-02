using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c969
{
    internal class user
    {
       //public int userId { get; set; }
       public string userName { get; set; }
       public  BindingList<appointment> Getappointments { get; set; }
        
        public DateTime Start { get; set; }
        public DateTime end { get; set; }
        public string description { get; set; }

    }
}
