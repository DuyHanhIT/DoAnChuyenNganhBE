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
    public class StatusAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(status))]
        [Route("api/Admin/getAllStatus/{keysearch}")]
        public HttpResponseMessage getAllStatus(string keysearch)
        {

            try
            {
                var f = db.status.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var v = f.ToList();
                var b = v.Where(x => x.statusname.ToLower().Contains(keysearch.ToLower())).ToList();
                var lstStatus = b.Select(bb => new
                {
                    bb.statusid,
                    bb.statusname,
                });
                if (lstStatus.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả status thành công", lstStatus));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có status nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
    }
}
