using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiShoesStoreDACN.Models
{
    public class OrderAndOrderdetails
    {
        public order order { get; set; }
        public List<orderdetail> lstOrderDetails { get; set; }
    }
}