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
    public class BrandAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(brand))]
        [Route("api/Admin/getAllBrands/{keysearch}")]
        public HttpResponseMessage getAllBrands(string keysearch)
        {

            try
            {
                var f = db.brands.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var v = f.ToList();
                var b = v.Where(x => x.brandname.ToLower().Contains(keysearch.ToLower())).ToList();
                var lstbrand = b.Select(bb => new
                {
                    bb.brandid,
                    bb.brandname,
                    bb.logo,
                    bb.information
                });
                if (lstbrand.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy hãng giày thành công", lstbrand));
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

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(brand))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetBrandByBrandId/{brandid}")]
        public HttpResponseMessage GetBrandByBrandId(int? brandid)
        {

            try
            {
                var s = db.brands.AsQueryable();
                if (brandid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 404, "Bạn chưa truyền mã hãng!", null));

                }
                var lstbrand = s.Where(i => i.brandid == brandid).Select(ss => new
                {
                    ss.brandid,
                    ss.brandname,
                    ss.information,
                    ss.logo
                });
                if (lstbrand.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy hãng có mã: \"{brandid}\" thành công", lstbrand));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có hãng nào!", null));
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(brand))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/AddBrand")]
        public HttpResponseMessage AddBrand(brand b)
        {
            try
            {
                if (b.brandname == null || b.brandname.Equals("") || b.information == null || b.information.Equals("") || b.logo == null || b.logo.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));
                }
                var checkBrand = db.brands.Where(a => a.brandname.Equals(b.brandname));
                if (checkBrand.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Tên hãng \"{b.brandname}\" đã tồn tại", null));
                }
                else
                {
                    brand ba = new brand();
                    ba.brandname = b.brandname;
                    ba.information = b.information;
                    ba.logo = b.logo;
                    db.brands.Add(ba);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm hãng \"{b.brandname}\" thành công", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));
            }
        }
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(brand))]
        [Route("api/Admin/UpdateBrand/{brandid}")]
        public HttpResponseMessage UpdateBrand(int brandid, brand ba)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (brandid != ba.brandid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy hãng", null));
                }
                else
                {
                    if (ba.brandname == null || ba.brandname.Equals("") || ba.information == null || ba.information.Equals("") || ba.logo == null || ba.logo.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    var u = db.brands.Where(i => i.brandname.ToLower().Equals(ba.brandname.ToLower())).ToList();
                    var o = db.brands.Where(i => i.brandid == ba.brandid).ToList();

                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.brandid != m.brandid)
                        {
                            u.Add(ba);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, $"Tên hãng \"{ba.brandname}\" đã tồn tại!", null));

                        }
                    }


                    var g = db.brands.FirstOrDefault(i => i.brandid == brandid);
                    g.brandname = ba.brandname;
                    g.information = ba.information;
                    g.logo = ba.logo;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công hãng có mã \"{ba.brandid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }

    }
}

