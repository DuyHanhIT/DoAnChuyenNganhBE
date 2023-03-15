using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiShoesStoreDACN.Models
{
    public class RegiterModel
    {
        public int accountid { get; set; }

        public int? roleid { get; set; }

        
        public string username { get; set; }

        public string Oldpassword { get; set; }

        public string password { get; set; }
        public string Repassword { get; set; }

        public DateTime? createdate { get; set; }

        public bool active { get; set; }
    }
}