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
    public class BrandController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(brand))]
        [Route("api/getAllBrands")]
        public HttpResponseMessage getAllBrands()
        {
            
            try
            {
                var b = db.brands.ToList();
                var lstbrand = b.Select(bb => new
                {
                    bb.brandid,
                    bb.brandname,
                    bb.logo,
                    bb.information
                });
                if (lstbrand.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả hãng giày thành công", lstbrand));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có hãng giày nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
    }
}
