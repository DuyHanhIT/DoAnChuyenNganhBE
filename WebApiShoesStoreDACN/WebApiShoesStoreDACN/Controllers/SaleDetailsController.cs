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
    public class SaleDetailsController : ApiController
    {
        DBShoesStore db = new DBShoesStore();
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(saleDetail))]
        // GET: api/ACCOUNTs
        [Route("api/getSaleDetailsBySaleId/{saleid}")]
        public HttpResponseMessage getSaleDetailsBySaleId(int saleid)
        {
            try
            {
                var countSales = db.sales.Where(i => i.saleid == saleid).ToList();
            if (countSales.Count() > 0)
            {
                   // var countSalesActive = db.sales.Where(i => i.saleid == saleid &&i.startday <= DateTime.Now && i.endday >= DateTime.Now).Count();
                    var countSalesActive = countSales.Where(i =>i.startday <= DateTime.Now && i.endday >= DateTime.Now).Count();
                    
                    if (countSalesActive > 0) {
                        var sd = db.saleDetails.Where(x => x.saleid == saleid && x.active == true).ToList();

                        var s = db.shoes.ToList();
                        var lstSaleDetails = (from a in sd
                                              join c in s on a.shoeid equals c.shoeid
                                              select new { a.saleid, a.shoeid, a.saleprice, a.updateby, c.shoename, c.price, c.image1 });
                        if (lstSaleDetails.Count() > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả sản phẩm đang khuyến mãi thành công", lstSaleDetails));
                            //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                        }
                        else
                        {
                            //return Request.CreateResponse(HttpStatusCode.BadRequest);

                            return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm nào trong chương trình khuyến mãi này", null));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Chương trình khuyến mãi này không khả dụng!", null));
                    }
                    /*var lstSale = s.Select(ss => new
                    {
                        ss.saleid,
                        ss.salename,
                        ss.createby,
                        ss.updateby,
                        ss.createdate,
                        ss.startday,
                        ss.endday,
                        ss.content,
                        ss.percent
                    });*/



                }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tồn tại chương tình khuyến mãi này!", null));
            }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }

        }
    }
}
