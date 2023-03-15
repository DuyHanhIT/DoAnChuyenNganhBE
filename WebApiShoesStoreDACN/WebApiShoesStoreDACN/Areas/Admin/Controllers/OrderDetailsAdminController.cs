using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Areas.Admin.Controllers
{
    public class OrderDetailsAdminController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(orderdetail))]
        [Route("api/Admin/getOrderDetails/{orderid}")]
        public HttpResponseMessage getOrderDetails(int orderid)
        {

            try
            {
                var b = db.orderdetails.AsQueryable();
                var lstOrderDetails = b.Where(s => s.orderid == orderid).ToList();
                var lstOrder = db.orders.Where(w => w.orderid == orderid);
                var lstShoes = db.shoes.ToList();

                var fillLstOrrderDetails = (from a in lstOrderDetails
                                            join c in lstOrder on a.orderid equals c.orderid
                                            join t in lstShoes on a.shoeid equals t.shoeid
                                            select new
                                            {
                                                a.orderid,
                                                a.shoeid,
                                                a.quantity,
                                                a.size,
                                                a.price,
                                                a.total,
                                                c.status.statusname,
                                                c.statusid,
                                                t.shoename,
                                                t.image1

                                            });
                if (fillLstOrrderDetails.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy chi tiết đơn hàng thành công", fillLstOrrderDetails));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có chi tiết đơn hàng nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
    }
}
