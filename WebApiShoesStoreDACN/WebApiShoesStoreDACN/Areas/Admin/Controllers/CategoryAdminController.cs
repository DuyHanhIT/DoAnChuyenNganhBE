using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Areas.Admin.Controllers
{
    public class CategoryAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(category))]
        [Route("api/Admin/getAllCategorys/{keysearch}")]
        public HttpResponseMessage getAllCategorys(string keysearch)
        {

            try
            {
                var f = db.categories.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var v = f.ToList();
                var b = v.Where(x => x.categoryname.ToLower().Contains(keysearch.ToLower())).ToList();
                var lstcategory = b.Select(bb => new
                {
                    bb.categoryid,
                    bb.categoryname,
                });
                if (lstcategory.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy danh mục thành công", lstcategory));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có danh mục nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(category))]
        [Route("api/Admin/GetCategoryByCategoryId/{categoryid}")]
        public HttpResponseMessage GetCategoryByCategoryId(int? categoryid)
        {
            try
            {
                var s = db.categories.AsQueryable();
                if (categoryid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 404, "Bạn chưa truyền mã danh mục", null));
                }
                else
                {
                    var lstcate = s.Where(i => i.categoryid == categoryid).Select(ss => new
                    {
                        ss.categoryid,
                        ss.categoryname
                    });
                    if (lstcate.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy danh mục theo mã danh mục thành công", lstcate));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có danh mục nào", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));
            }

        }
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(category))]
        [Route("api/Admin/UpdateCategory/{categoryid}")]
        public HttpResponseMessage UpdateCategory(int categoryid, category ca)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (categoryid != ca.categoryid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy danh mục", null));
                }
                else
                {
                    if (ca.categoryname == null || ca.categoryname.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền tên danh mục", null));
                    }
                    var u = db.categories.Where(i => i.categoryname.ToLower().Equals(ca.categoryname.ToLower())).ToList();
                    var o = db.categories.Where(i => i.categoryid == ca.categoryid).ToList();

                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.categoryid != m.categoryid)
                        {
                            u.Add(ca);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, $"Tên danh mục \"{ca.categoryname}\" đã tồn tại!", null));

                        }
                    }


                    var g = db.categories.FirstOrDefault(i => i.categoryid == categoryid);
                    g.categoryname = ca.categoryname;

                    db.Entry(g).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công danh mục có mã \"{ca.categoryid}\"", null));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý!", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));
            }
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(category))]
        [Route("api/Admin/AddCategory")]
        public HttpResponseMessage AddCategory(category ca)
        {
            try
            {
                if (ca.categoryname == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền tên danh mục", null));
                }
                var checkCate = db.categories.Where(a => a.categoryname.Equals(ca.categoryname));
                if (checkCate.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên danh mục đã tồn tại", null));
                }
                else
                {
                    try
                    {
                        category cate = new category();
                        cate.categoryname = ca.categoryname;

                        db.categories.Add(cate);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm thành công \"{ca.categoryname}\"!", null));
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình đăng ký", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Error server", null));
            }
        }
    }
}
