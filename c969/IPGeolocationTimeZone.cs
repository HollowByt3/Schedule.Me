using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c969
{
    internal class IPGeolocationTimeZone
    {
        public string timezone { get; set; }
        public int Timezone_offset { get; set; }
        public int Timezone_offset_with_dst { get; set; }
        public string Date { get; set; }
        public DateTime Date_time { get; set; }
        public string Date_time_txt { get; set; }
        public string Date_time_wti { get; set; }
        public DateTime Date_time_ymd { get; set; }
        public double Date_time_unix { get; set; }
        public string Time_24 { get; set; }
        public string Time_12 { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Year_abbr { get; set; }
        public bool Is_dst { get; set; }
        public int Dst_savings { get; set; }
        public bool Dst_exists { get; set; }
        
    }
}
