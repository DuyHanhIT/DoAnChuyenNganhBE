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
    public class RolesAdminController : ApiController
    {
        // GET: api/RolesAdmin
        DBShoesStore db = new DBShoesStore();
       // [Authorize]
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(role))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAllRoles")]
        public HttpResponseMessage GetAllRoles( )
        {
            
            try
            {
                var d = db.roles.AsQueryable();
                var s = d.ToList();
                var lsRoles = s.Select(ss => new
                {
                    ss.roleid,
                    ss.rolename
                
                });
                if (lsRoles.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get all roles success", lsRoles));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Get all roles faild", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Server error", null));

            }
            

        }
    }
}
