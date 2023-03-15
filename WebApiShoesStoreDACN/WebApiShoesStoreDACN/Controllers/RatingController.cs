using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Controllers
{
    public class RatingController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpPost]
        [ResponseType(typeof(rating))]
        [Route("api/AddRating")]
        public HttpResponseMessage AddRating(rating ra)
        {
            try
            {
                if (ra.shoeid == null || ra.accountid == null || ra.rate == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));
                }
                if (ra.rate <= 0 || ra.rate > 5)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Giá trị rate không hợp lệ", null));
                }
                var getRate = db.ratings.Where(i => i.accountid == ra.accountid && i.shoeid == ra.shoeid).FirstOrDefault();
                if (getRate != null)
                {
                    getRate.rate = ra.rate;
                    db.Entry(getRate).State = EntityState.Modified;
                    db.SaveChanges();
                    var avgRate = db.ratings.Where(i => i.shoeid == ra.shoeid)
                            .GroupBy(g => g.shoeid, i => i.rate)
                            .Select(g => new
                            {
                                Rating = g.Average()
                            });
                    var gShoes = db.shoes.Where(i => i.shoeid == ra.shoeid).FirstOrDefault();
                    var k = avgRate.Select(i => i.Rating).FirstOrDefault();
                    decimal l = 0;
                    if (k == 0.5M ||k==1.5M||k==2.5M||k==3.5M||k==4.5M)
                    {
                        l = (decimal)k;
                        gShoes.rate = l;
                    }
                    else 
                    {
                        l = (decimal)Math.Round((double)k);
                        gShoes.rate = l;
                    }
                        
                    db.Entry(gShoes).State = EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Cập nhật đánh giá thành công!!", null));

                }
                else
                {
                    var lo = db.orders.ToList();
                    var lod = db.orderdetails.ToList();

                    var lstOAndOd = (from ss in lo
                                     join a in lod on ss.orderid equals a.orderid
                                     select new
                                     {
                                         ss.orderid,
                                         ss.accountid,
                                         ss.statusid,
                                         a.shoeid
                                     }).ToList();
                    if (lstOAndOd.Count() > 0)
                    {
                        var lOAndOd1 = lstOAndOd.Where(i => i.accountid == ra.accountid && i.shoeid == ra.shoeid).ToList();
                        if (lOAndOd1.Count() > 0)
                        {
                            var checkstatus = lOAndOd1.Where(i => i.statusid == 4).ToList();
                            if (checkstatus.Count() > 0)
                            {
                                rating r = new rating();
                                r.accountid = ra.accountid;
                                r.shoeid = ra.shoeid;
                                r.rate = ra.rate;
                                db.ratings.Add(r);
                                db.SaveChanges();
                                var avgRate = db.ratings.Where(i => i.shoeid == ra.shoeid)
                                    .GroupBy(g => g.shoeid, i => i.rate)
                                    .Select(g => new
                                    {
                                        Rating = g.Average()
                                    });
                                var gShoes = db.shoes.Where(i => i.shoeid == ra.shoeid).FirstOrDefault();
                                var k = avgRate.Select(i => i.Rating).FirstOrDefault();
                                decimal l = 0;
                                if (k == 0.5M || k == 1.5M || k == 2.5M || k == 3.5M || k == 4.5M)
                                {
                                    l = (decimal)k;
                                    gShoes.rate = l;
                                }
                                else
                                {
                                    l = (decimal)Math.Round((double)k);
                                    gShoes.rate = l;
                                }
                                db.Entry(gShoes).State = EntityState.Modified;
                                db.SaveChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Đánh giá thành công!", null));

                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Đơn hàng bạn chưa giao thành công, bạn không thể đánh giá.", null));
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Bạn chưa mua sản phẩm này", null));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Chưa có đơn hàng nào trong hệ thống", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }
    }
}
