using System;
using System.Collections.Generic;
using System.Text;

namespace App13.Models
{
    class loads
    {
        public int id { get; set; }
        public string pickupfrom { get; set; }
        public string deliveryto { get; set; }
        public string pickupdate { get; set; }
        public string deliverydate { get; set; }
        public string weightpcs { get; set; }
        public Nullable<int> bid { get; set; }
        public Nullable<int> finalprice { get; set; }
        public Nullable<int> userid { get; set; }
        public string ordernumber { get; set; }
        public Nullable<int> statusid { get; set; }
        public Nullable<int> totalmiles { get; set; }
    }
}
