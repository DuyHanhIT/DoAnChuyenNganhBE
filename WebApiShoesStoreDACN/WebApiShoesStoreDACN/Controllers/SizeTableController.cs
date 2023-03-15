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
    public class SizeTableController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(sizetable))]
        [Route("api/getSizeByShoesId/{shoesid}")]
        public HttpResponseMessage getSizeByShoesId(int shoesid)
        {
           
            try
            {
                var st = db.sizetables.Where(x => x.shoeid == shoesid).ToList();
                var lstst = st.Select(bb => new
                {
                    bb.stid,
                    bb.shoeid,
                    bb.s38,
                    bb.s39,
                    bb.s40,
                    bb.s41,
                    bb.s42,
                    bb.s43,
                    bb.s44,
                    bb.s45,
                    bb.s46,
                    bb.s47,
                    bb.s48

                });
                if (lstst.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy size có mã sản phẩm ={shoesid} thành công", lstst));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, $"Không có size nào có mã sản phẩm = {shoesid}", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
    }
}
