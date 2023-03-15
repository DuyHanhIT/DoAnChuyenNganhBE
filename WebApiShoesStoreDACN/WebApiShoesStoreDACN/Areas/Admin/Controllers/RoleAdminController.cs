using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Areas.Admin.Controllers
{
    public class RoleAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(role))]
        [Route("api/Admin/getAllRole/{keysearch}")]
        public HttpResponseMessage getAllRole(string keysearch)
        {

            try
            {
                var f = db.roles.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var v = f.ToList();
                var b = v.Where(x => x.rolename.ToLower().Contains(keysearch.ToLower())).ToList();
                var lstRole = b.Select(bb => new
                {
                    bb.roleid,
                    bb.rolename
                });
                if (lstRole.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy danh sách phân quyền thành công", lstRole));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có phân quyền nào nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Lỗi máy chủ", null));

            }
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(role))]
        [Route("api/Admin/GetRoleByRoleId/{roleid}")]
        public HttpResponseMessage GetBrandByBrandId(int? roleid)
        {

            try
            {
                var s = db.roles.AsQueryable();
                if (roleid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 404, "Bạn chưa truyền mã phân quyền!", null));
                }
                var lstRole = s.Where(i => i.roleid == roleid).Select(ss => new
                {
                    ss.roleid,
                    ss.rolename
                });
                if (lstRole.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy phân quyền có mã: \"{roleid}\" thành công", lstRole));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có phân quyền nào!", null));
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(role))]
        [Route("api/Admin/AddRole")]
        public HttpResponseMessage AddRole(role r)
        {
            try
            {
                if (r.rolename == null || r.rolename.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền tên phân quyền", null));
                }
                var checkRole = db.roles.Where(a => a.rolename.Equals(r.rolename));
                if (checkRole.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Tên phân quyền \"{r.rolename}\" đã tồn tại", null));
                }
                else
                {
                    role ro = new role();
                    ro.rolename = r.rolename;
                    db.roles.Add(ro);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm phân quyền \"{r.rolename}\" thành công", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));
            }
        }
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(role))]
        [Route("api/Admin/UpdateRole/{roleid}")]
        public HttpResponseMessage UpdateRole(int roleid, role r)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (roleid != r.roleid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy phân quyền", null));
                }
                else
                {
                    if (r.roleid < 3)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Bạn không được sử phân quyền Admin và Staff", null));
                    }
                    if (r.rolename == null || r.rolename.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền tên phân quyền", null));
                    }
                    var u = db.roles.Where(i => i.rolename.ToLower().Equals(r.rolename.ToLower())).ToList();
                    var o = db.roles.Where(i => i.roleid == r.roleid).ToList();

                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.roleid != m.roleid)
                        {
                            u.Add(r);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, $"Tên phân quyền \"{r.rolename}\" đã tồn tại!", null));
                        }
                    }


                    var g = db.roles.FirstOrDefault(i => i.roleid == roleid);
                    g.rolename = r.rolename;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công phân quyền có mã \"{r.roleid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
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
