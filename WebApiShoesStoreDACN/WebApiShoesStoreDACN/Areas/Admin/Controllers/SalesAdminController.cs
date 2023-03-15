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
    public class SalesAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(sale))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/getAllSales/{keysearch}")]
        public HttpResponseMessage getAllSales(string keysearch)
        {
            try
            {
                var d = db.sales.AsQueryable(); ;//ktab.KTABTOM <= DateTime.Now
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();

                var lstSale = s.Where(i => i.salename.ToLower().Contains(keysearch.ToLower())).Select(ss => new
                {
                    ss.saleid,
                    ss.salename,
                    ss.createby,
                    ss.updateby,
                    ss.createdate,
                    ss.startday,
                    ss.endday,
                    ss.content,
                    ss.percent,
                    ss.imgsale,
                });

                if (lstSale.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả chương trình khuyễn mãi thành công", lstSale));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có chương trình khuyễn mãi nào", null));
                }



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(sale))]
        [Route("api/Admin/AddSale")]
        public HttpResponseMessage PostSale(sale s)
        {
            try
            {
                if (s.salename == null || s.salename.Equals("") || s.startday == null || s.startday.Equals("") ||
                    s.endday == null || s.endday.Equals("") || s.createby == null || s.updateby == null ||
                    s.content == null || s.content.Equals("") || s.percent == null|| s.imgsale==null||s.imgsale.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin!", null));
                }
                string salename = s.salename;
                int? createby = s.createby;
                int? updateby = s.updateby;
                DateTime startday = (DateTime)s.startday;
                DateTime endday = (DateTime)s.endday;
                string content = s.content;
                DateTime createDate = DateTime.Now;
                decimal percent = (decimal)s.percent;
                string imgSale = s.imgsale;

                var existSale = db.sales.Where(a => a.salename.ToLower().Equals(salename.ToLower()));
                var exitCreateby = db.accounts.Where(f => f.accountid == createby).ToList();
                var exitUpdateby = db.accounts.Where(v => v.accountid == updateby).ToList();
                var lstS = new List<sale>();
                lstS.Add(s);

                TimeSpan diffDate;
                try
                {
                    diffDate = endday.Subtract(startday);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong qua trình diff date", null));

                }
                if (existSale.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Chương trình khuyến mãi đã tồn tại!", null));
                }
                else if (exitCreateby.Count() < 1 || exitUpdateby.Count() < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không tồn tại tài khoản này trong hệ thống", null));
                }
                var roleidCreate = (from a in lstS
                                    join c in exitCreateby on a.createby equals c.accountid
                                    select new { c.roleid }).First();
                var roleidUpdate = (from a in lstS
                                    join c in exitUpdateby on a.updateby equals c.accountid
                                    select new { c.roleid }).First();
                if (roleidCreate.roleid == 3 || roleidUpdate.roleid == 3)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tài khoản phải có quyền admin hoặc staff", null));
                }
                else if (startday < (DateTime.Now).AddDays(-1))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Ngày bắt đầu khuyến mãi phải lớn hơn ngày hiện tại", null));
                }
                else if (diffDate.Days < 3)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Chương trình khuyến mãi phải bắt đầu từ 3 ngày trở lên", null));
                }
                else if (percent < 0 || percent > 99.99M)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Phần trăm khuyễn mãi phải trong khoảng 0% - 99.99%", null));
                }
                else
                {
                    try
                    {
                        sale sa = new sale();
                        sa.salename = salename;
                        sa.createby = createby;
                        sa.updateby = updateby;
                        sa.startday = startday;
                        sa.endday = endday;
                        sa.content = content;
                        sa.createdate = createDate;
                        sa.percent = percent;
                        sa.imgsale = imgSale;
                        var lstSales = new List<sale>();
                        lstSales.Add(sa);
                        var lstSales1 = lstSales.Select(x => new
                        {
                            x.saleid,
                            x.salename,
                            x.createby,
                            x.updateby,
                            x.createdate,
                            x.startday,
                            x.endday,
                            x.content,
                            x.percent,
                            x.imgsale
                        });
                        db.sales.Add(sa);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Thêm chương trình khuyến mãi thành công", lstSales1));

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý 1", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
            }
        }
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/UpdateSale/{saleid}")]
        public HttpResponseMessage UpdateSale(int saleid, sale sh)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (saleid != sh.saleid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy chương trình KM", null));
                }
                else
                {

                    if (sh.salename == null || sh.salename.Equals("") || sh.startday == null || sh.startday.Equals("") || sh.endday == null || sh.endday.Equals("") ||
                        sh.percent == null || sh.content == null || sh.content.Equals("") || sh.imgsale == null || sh.imgsale.Equals("") || sh.updateby == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    var u = db.sales.Where(i => i.salename.ToLower().Equals(sh.salename.ToLower())).ToList();
                    var o = db.sales.Where(i => i.saleid == sh.saleid).ToList();
                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.saleid != m.saleid)
                        {
                            u.Add(sh);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Tên chương trình không được trùng", null));
                        }
                    }
                    DateTime startday = (DateTime)sh.startday;
                    DateTime endday = (DateTime)sh.endday;
                    TimeSpan diffDate;
                    try
                    {
                        diffDate = endday.Subtract(startday);
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong qua trình diff date", null));

                    } 
                    if (diffDate.Days < 3)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Chương trình khuyến mãi phải bắt đầu từ 3 ngày trở lên", null));
                    }
                    else if (sh.percent < 0 || sh.percent > 99.99M)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Phần trăm khuyễn mãi phải trong khoảng 0% - 99.99%", null));
                    }
                    else
                    {

                        try
                        {
                            var g = db.sales.FirstOrDefault(i => i.saleid == saleid);
                            if (startday < g.createdate)
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Ngày bắt đầu khuyễn mãi phải lớn hơn ngày tạo chương trình khuyến mãi", null));
                            }
                            DateTime e = (DateTime)sh.endday;
                            var e1 = e.AddDays(1).AddSeconds(-1);
                            g.salename = sh.salename;
                            g.updateby = sh.updateby;
                            g.startday = sh.startday;
                            g.endday = e1;
                            g.content = sh.content;
                            g.percent = sh.percent;
                            g.imgsale = sh.imgsale;
                            db.Entry(g).State = EntityState.Modified;
                            db.SaveChanges();

                            var ctkm = db.saleDetails.Where(i => i.saleid == g.saleid).ToList();
                            var lstshoes = db.shoes.ToList();

                            ///
                           var getShoesidAndPrice=  (from p in ctkm join q in lstshoes on p.shoeid equals q.shoeid
                            select new
                            {
                               p.shoeid,
                               q.price,
                            }).ToList();
                            if (getShoesidAndPrice.Count() > 0)
                            {
                                foreach(var l in getShoesidAndPrice)
                                {
                                    var sdt = ctkm.FirstOrDefault(i => i.saleid == g.saleid && i.shoeid == l.shoeid);
                                    sdt.saleprice = l.price - ((g.percent * l.price) / 100);
                                    sdt.updateby = g.updateby;
                                    db.Entry(sdt).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                            }
                            ///

                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công CTKm có mã {g.saleid}", null));
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));
                        }
                    }

                }
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, e.ToString(), null));
            }
        }
        [Authorize(Roles = "1")]

        [HttpGet]
        [ResponseType(typeof(sale))]
        [Route("api/Admin/GetSaleBySaleId/{saleid}")]
        public HttpResponseMessage GetSaleBySaleId(int saleid)
        {
            try
            {
                var d = db.sales.AsQueryable();
                var account = db.accounts.ToList();

                var s = d.Where(x => x.saleid == saleid).ToList();
                var lstSale = from ss in s
                              join a in account on ss.createby equals a.accountid
                              join aa in account on ss.updateby equals aa.accountid
                              select new
                              {
                                  ss.saleid,
                                  ss.salename,
                                  ss.createby,
                                  ss.updateby,
                                  ss.createdate,
                                  ss.startday,
                                  ss.endday,
                                  ss.content,
                                  ss.percent,
                                  ss.imgsale,
                                  createByA = a.username,
                                  updateByA = aa.username

                              };

                if (lstSale.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy chương trình khuyến mãi by saleid thành công", lstSale));

                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có chương trình khuyễn mãi nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Lấy chương trình khuyến mãi by saleid thất bại", null));

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
