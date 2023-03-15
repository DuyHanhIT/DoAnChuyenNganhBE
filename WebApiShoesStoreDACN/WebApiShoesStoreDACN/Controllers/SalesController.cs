using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Controllers
{
    public class SalesController : ApiController
    {
        DBShoesStore db = new DBShoesStore();
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(sale))]
        // GET: api/ACCOUNTs
        [Route("api/getAllSales")]
        public HttpResponseMessage getAllSales()
        {
            try
            {
                var s = db.sales.Where(x=>x.startday <= DateTime.Now && x.endday>=DateTime.Now).ToList();//ktab.KTABTOM <= DateTime.Now
                if (s.Count() > 0)
                {
                    var lstSale = s.Select(ss => new
                    {
                        ss.saleid,
                        ss.salename,
                        ss.createby,
                        ss.updateby,
                        ss.createdate,
                        ss.startday,
                        ss.endday,
                        ss.content,
                        ss.percent,
                        ss.imgsale,
                    });

                    if (lstSale.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả chương trình khuyễn mãi thành công", lstSale));
                        //return Request.CreateResponse(HttpStatusCode.OK, lstacc);
                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest);

                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Đã xảy ra lỗi trong quá trình xử lý", null));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có chương trình khuyến mãi nào", null));
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
