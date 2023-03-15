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
    public class SaleDetailsAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1,2")]

        [HttpGet]
        [ResponseType(typeof(saleDetail))]
        [Route("api/Admin/GetSaleDetailsBySaleId/{saleid}/{keysearch}")]
        public HttpResponseMessage GetSaleDetailsBySaleId(int saleid, string keysearch)
        {
            try
            {
                var d = db.saleDetails.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }


                var s = d.Where(x => x.saleid == saleid).ToList();
                var sl = db.sales.Where(i => i.saleid == saleid).ToList();
                var sh = db.shoes.ToList();
                var lstSale = from ss in s
                              join a in sh on ss.shoeid equals a.shoeid
                              join b in sl on ss.saleid equals b.saleid
                              select new
                              {
                                  ss.saleid,
                                  ss.shoeid,
                                  a.shoename,
                                  a.image1,
                                  a.price,
                                  ss.saleprice,
                                  b.percent,
                                  ss.updateby,
                                  ss.active



                              };
                var lstSaleDetails1 = lstSale.Where(i => i.shoename.ToLower().Contains(keysearch.ToLower())).ToList();
                if (lstSale.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy sản phẩm khuyến mãi by saleid thành công", lstSaleDetails1));

                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm khuyến mãi nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Lấy sản phẩm khuyến mãi by saleid thất bại", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpDelete]
        [ResponseType(typeof(saleDetail))]
        [Route("api/Admin/DeleteSaleDetailsByShoesIdAndSaleid/{saleid}/{shoeid}")]
        public HttpResponseMessage DeleteSaleDetailsByShoesIdAndSaleid(int saleid, int shoeid)
        {
            try
            {

                var g = db.saleDetails.FirstOrDefault(i => i.saleid == saleid && i.shoeid == shoeid);
                if (g == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy sản phẩm ", null));
                }
                else
                {


                    db.saleDetails.Remove(g);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Xóa thành công sản phẩm khuyến mãi", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Lấy sản phẩm khuyến mãi by saleid thất bại", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAllShoesNotExistsInSaledetails/{keysearch}")]
        public HttpResponseMessage GetAllShoesNotExistsInSaledetails(string keysearch)
        {
            var d = db.shoes.AsQueryable();
            if (keysearch.Equals("="))
            {
                keysearch = "";
            }
            var p = d.ToList();
            var t = p.Where(i => i.active == true && i.shoename.ToLower().Contains(keysearch.ToLower())).ToList();

            var g = from f in t
                    where !db.saleDetails.Any(es => (es.shoeid == f.shoeid && es.active == true))
                    select new
                    {
                        f.shoeid,
                        f.shoename,
                        f.image1,
                        f.price,
                        f.brand.brandname,
                        f.category.categoryname,
                        f.stock,
                        f.purchased
                    };



            try
            {
                if (g.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả giày chưa khuyến mãi thành công", g));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có giày nào chưa khuyến mãi", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        [ResponseType(typeof(saleDetail))]
        [Route("api/Admin/AddShoesToSaleDetails")]
        public HttpResponseMessage AddOrder(List<saleDetail> lstS)
        {

            try
            {
                var j = db.sales.AsQueryable();
                var k = db.shoes.AsQueryable(); 
                foreach (var item in lstS)
                {
                    var jj = j.FirstOrDefault(i => i.saleid == item.saleid);
                    var lstshoes = k.ToList();
                    var ll = new List<saleDetail>();
                    ll.Add(item);
                    var lll = ll.ToList();
                    ///
                    var getShoesidAndPrice = (from p in lll
                                              join q in lstshoes on p.shoeid equals q.shoeid
                                              select new
                                              {
                                                  p.shoeid,
                                                  q.price,
                                              }).ToList();
                    var checkSaleDetail = db.saleDetails.Where(i => i.shoeid == item.shoeid && i.saleid==item.saleid).ToList();
                    
                    if (getShoesidAndPrice.Count() > 0)
                    {
                        if (checkSaleDetail.Count() < 1)
                        {
                            saleDetail obj = new saleDetail();
                            obj.saleid = item.saleid;
                            obj.shoeid = item.shoeid;
                            obj.updateby = item.updateby;
                            obj.active = true;
                            var llll = getShoesidAndPrice[0];
                            obj.saleprice = llll.price - ((jj.percent * llll.price) / 100);
                            db.saleDetails.Add(obj);
                            db.SaveChanges();

                        }
                        else
                        {
                           var c= checkSaleDetail.FirstOrDefault();
                            var llll = getShoesidAndPrice[0];
                            c.saleprice = llll.price - ((jj.percent * llll.price) / 100);
                            c.active = true;
                            c.updateby = item.updateby;
                            db.Entry(c).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                       
                    }
                    
                   

                }
               
                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm sản phẩm cho chương trình khuyến mãi thành công", null));

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
