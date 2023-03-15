using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiShoesStoreDACN.Models
{
    public class ApiResult
    {
        public bool isSuccess { get; set; }
        public int status { get; set; }
        public string Message { get; set; }
        public object data { get; set; }

        public ApiResult(bool isSuccess, int status, string Message, object data)
        {
            this.isSuccess = isSuccess;
            this.status = status;
            this.Message = Message;
            this.data = data;
        }
    }
}