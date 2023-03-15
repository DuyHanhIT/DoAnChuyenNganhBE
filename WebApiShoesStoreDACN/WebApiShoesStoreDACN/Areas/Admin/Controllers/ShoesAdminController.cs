using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Areas.Admin.Controllers
{
    public class ShoesAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        //[Authorize]
        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAllShoes/{keysearch}")]
        public HttpResponseMessage GetAllShoes(string keysearch)
        {
            try
            {
                var d = db.shoes.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();
                var lstShoes = s.Where(x => x.shoename.ToLower().Contains(keysearch.ToLower())).Select(ss => new
                {
                    ss.shoeid,
                    ss.shoename,
                    ss.price,
                    ss.description,
                    ss.stock,
                    ss.purchased,
                    ss.shoenew,
                    ss.createdate,
                    ss.createby,
                    ss.updateby,
                    ss.dateupdate,
                    ss.active,
                    ss.brandid,
                    ss.categoryid,
                    ss.image1,
                    ss.image2,
                    ss.image3,
                    ss.image4,
                    ss.brand.brandname,
                    ss.category.categoryname,
                    ss.rate
                });
                if (lstShoes.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get all shoes success", lstShoes));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có sản phẩm nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Get all shoes faild", null));

            }
        }
        //[Authorize]
        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetShoesByShoesId/{shoesid}")]
        public HttpResponseMessage GetShoesByShoesId(int shoesid)
        {
            try
            {
                var d = db.shoes.AsQueryable();
                var account = db.accounts.ToList();

                var s = d.Where(x => x.shoeid == shoesid).ToList();
                var lstShoes = from ss in s
                               join a in account on ss.createby equals a.accountid
                               join aa in account on ss.updateby equals aa.accountid
                               select new
                               {
                                   ss.shoeid,
                                   ss.shoename,
                                   ss.price,
                                   ss.description,
                                   ss.stock,
                                   ss.purchased,
                                   ss.shoenew,
                                   ss.createdate,
                                   ss.createby,
                                   ss.updateby,
                                   ss.dateupdate,
                                   ss.active,
                                   ss.brandid,
                                   ss.categoryid,
                                   ss.image1,
                                   ss.image2,
                                   ss.image3,
                                   ss.image4,
                                   ss.brand.brandname,
                                   ss.category.categoryname,
                                   createByA = a.username,
                                   updateByA = aa.username,
                                   ss.rate

                               };
                
                if (lstShoes.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get shoes by shoesId success", lstShoes));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có sản phẩm nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Get shoes by shoesId faild", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/AddShoes")]
        public HttpResponseMessage AddShoes(shoes sh)
        {
            try
            {
                if (sh.brandid == null || sh.categoryid == null || sh.shoename == null || sh.shoename.Equals("")
                    || sh.price == null || sh.description == null || sh.description.Equals("") || sh.shoename == null
                    || sh.shoenew == null || sh.image1 == null || sh.image1.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng nhập đầy đủ thông tin", null));
                }
                var checkCreateBy = db.accounts.Where(x => x.accountid == sh.createby);
                var checkUpdateBy = db.accounts.Where(x => x.accountid == sh.updateby);
                if (checkCreateBy.Count() < 1 || checkUpdateBy.Count() < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không tồn tại tài khoản này", null));
                }
                var lstS = new List<shoes>();
                lstS.Add(sh);
                var roleidCreate = (from a in lstS
                                    join c in checkCreateBy on a.createby equals c.accountid
                                    select new { c.roleid }).First();
                var roleidUpdate = (from a in lstS
                                    join c in checkUpdateBy on a.updateby equals c.accountid
                                    select new { c.roleid }).First();
                if (roleidCreate.roleid == 3 || roleidUpdate.roleid == 3)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tài khoản phải có quyền admin hoặc staff", null));
                }
                var exitShoes = db.shoes.Where(x => x.shoename.Equals(sh.shoename)).ToList();
                if (exitShoes.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên sản phẩm đã tồn tại", null));
                }
                if (sh.price < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Giá bán phải lớn hơn 0$", null));
                }
                else
                {
                    try
                    {
                        shoes sho = new shoes();
                        sho.brandid = sh.brandid;
                        sho.categoryid = sh.categoryid;
                        sho.shoename = sh.shoename;
                        sho.price = sh.price;
                        sho.description = sh.description;
                        sho.stock = 0;
                        sho.purchased = 0;
                        sho.shoenew = sh.shoenew;
                        sho.createby = sh.createby;
                        sho.updateby = sh.updateby;
                        sho.createdate = DateTime.Now;
                        sho.dateupdate = DateTime.Now;
                        sho.active = true;
                        sho.image1 = sh.image1;
                        sho.image2 = sh.image2 == null ? null : sh.image2.Equals("") ? null : sh.image2;
                        sho.image3 = sh.image3 == null ? null : sh.image3.Equals("") ? null : sh.image3;
                        sho.image4 = sh.image4 == null ? null : sh.image4.Equals("") ? null : sh.image4;
                        
                        db.shoes.Add(sho);
                        db.SaveChanges();
                        var shoesId = sho.shoeid;
                       
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm thành công sản phẩm có mã {shoesId}", null));

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));
            }
        }
        [Authorize(Roles = "1,2")]
        [HttpPut]
        [ResponseType(typeof(shoes))]
        [Route("api/Admin/UpdateShoes/{shoesid}")]
        public HttpResponseMessage PutShoes(int shoesid, shoes sh)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (shoesid != sh.shoeid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy sản phẩm", null));
                }
                else
                {

                    if (sh.brandid == null || sh.categoryid == null || sh.shoename == null || sh.shoename.Equals("") ||
                        sh.price == null || sh.description == null || sh.description.Equals("") || sh.shoenew == null || sh.updateby == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    var u = db.shoes.Where(i => i.shoename.ToLower().Equals(sh.shoename.ToLower())).ToList();
                    var o = db.shoes.Where(i => i.shoeid == sh.shoeid).ToList();
                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.shoeid != m.shoeid)
                        {
                            u.Add(sh);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Tên giày không được trùng", null));
                        }
                    }

                    if (sh.price <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Giá phải lớn hơn 0$", null));

                    }
                    if (sh.image1 == null || sh.image1.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng chọn ảnh số 1", null));
                    }
                    else
                    {

                        try
                        {
                            var g = db.shoes.FirstOrDefault(i => i.shoeid == shoesid);
                            g.brandid = sh.brandid;
                            g.categoryid = sh.categoryid;
                            g.price = sh.price;
                            g.shoename = sh.shoename;
                            g.description = sh.description;
                            g.active = sh.active;
                            g.shoenew = sh.shoenew;
                            g.image1 = sh.image1;
                            g.image2 = sh.image2;
                            g.image3 = sh.image3;
                            g.image4 = sh.image4;
                            g.dateupdate = DateTime.Now;
                            g.updateby = sh.updateby;

                            db.Entry(g).State = EntityState.Modified;
                            db.SaveChanges();
                            var getsaledetails = db.saleDetails.FirstOrDefault(i => i.shoeid == g.shoeid && i.active == true);
                            if(getsaledetails != null)
                            {
                                var getSale = db.sales.FirstOrDefault(i => i.saleid == getsaledetails.saleid);
                                getsaledetails.saleprice = g.price - ((getSale.percent * g.price) / 100);
                                db.Entry(getsaledetails).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công sp có mã {g.shoeid}", null));
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


        [Authorize(Roles = "1,2")]
        [HttpPut]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/InactiveShoes/{shoesid}")]
        public HttpResponseMessage InactiveShoes(int shoesid, shoes sh)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (shoesid != sh.shoeid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy sản phẩm", null));
                }
                else
                {

                    if (sh.updateby == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    else
                    {

                        try
                        {
                            var g = db.shoes.FirstOrDefault(i => i.shoeid == shoesid);
                            if (g.active == false)
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Sản phẩm đã ngừng kinh doanh", null));
                            }
                            g.active = false;

                            g.dateupdate = DateTime.Now;
                            g.updateby = sh.updateby;
                            db.Entry(g).State = EntityState.Modified;
                            /*var lstAcc = new List<account>();
                            lstAcc.Add(g);
                            var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });*/


                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Ngừng kinh doanh thành công sp có mã {g.shoeid}", null));
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
            }
        }
    }
}
