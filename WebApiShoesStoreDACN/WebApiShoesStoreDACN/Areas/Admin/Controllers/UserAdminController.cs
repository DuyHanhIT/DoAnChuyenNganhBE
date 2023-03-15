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
    public class UserAdminController : ApiController
    {

        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(user))]
        [Route("api/Admin/GetAllUser/{keysearch}")]
        public HttpResponseMessage GetAllUser(string keysearch)
        {
            try
            {
                var d = db.users.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();
                var lstUser = s.Where(x => x.firstName.ToLower().Contains(keysearch.ToLower()))
                    .Select(ss => new
                    {
                        ss.userid,
                        ss.firstName,
                        ss.lastName,
                        ss.accountid,
                        ss.phone,
                        ss.address,
                        ss.avatar,
                        ss.email,
                        ss.account.username
                    });
                if (lstUser.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả user thành công", lstUser));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có user nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(user))]
        [Route("api/Admin/GetUserByUserId/{userid}")]
        public HttpResponseMessage GetUserByUserId(int userid)
        {
            try
            {
                var d = db.users.AsQueryable();

                var s = d.Where(x => x.userid == userid).ToList();
                var lstUser = s.Select(ss => new
                {
                    ss.userid,
                    ss.firstName,
                    ss.lastName,
                    ss.accountid,
                    ss.phone,
                    ss.address,
                    ss.avatar,
                    ss.email,
                    ss.account.username
                });

                if (lstUser.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy user by UserId thành công", lstUser));
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có user nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
    }
}
