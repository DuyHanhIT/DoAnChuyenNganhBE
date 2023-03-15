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
    public class StatisticalController : ApiController
    {
        DBShoesStore db = new DBShoesStore();
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/getStatisticalOrder/{day}/{month}/{year}/{type}")]
        public HttpResponseMessage getAllCategorys(int day, int month, int year, int type)
        {
            try
            {
                var q0 = db.orders.ToList();
                var qq = new List<order>();
                if (type == 1)
                {
                    qq = (List<order>)q0.Where(i => i.createdate.Day == day && i.createdate.Month == month && i.createdate.Year == year).ToList();
                }
                else if (type == 2)
                {
                    qq = (List<order>)q0.Where(i => i.createdate.Month == month && i.createdate.Year == year).ToList();
                }
                else if (type == 3)
                {
                    qq = (List<order>)q0.Where(i => i.createdate.Year == year).ToList();
                }
                if (qq.Count() > 0)
                {
                    var q = qq.GroupBy(i => i.statusid)
                        .Select(x => new
                        {
                            statusid = x.Key,
                            countOrder = x.Count()
                        })
                        .OrderBy(x => x.statusid).ToList();
                    var q1 = db.status.ToList();
                    var q2 = (from n in q
                              join m in q1 on n.statusid equals m.statusid
                              select new { n.statusid, m.statusname, n.countOrder }
                              ).ToList();
                    if (q2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy danh mục thành công", q2));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có danh mục nào", null));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có TK nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/getStatisticalMonth/{month}/{year}/{type}")]
        public HttpResponseMessage getStatisticalMonth(int month, int year, int type)
        {
            try
            {
                var month1 = month > 9 ? month.ToString() : "0" + month.ToString();
                string year1 = year.ToString();

                var q = db.monthlyrevenues.ToList();
                if (type == 1)
                {
                    var o = db.orders.Where(i => i.statusid != 5 && i.payment == true && i.createdate.Month.Equals(month) && i.createdate.Year.Equals(year)).GroupBy(i => i.createdate.Day)
                           .Select(i => new { day = i.Key, month = i.Select(e => e.createdate.Month).FirstOrDefault(), turnover = i.Sum(t => t.total) }).ToList();
                    // var o1 = o.GroupBy(i => i.day).Select(i=>new {day= i.Key});
                    if (o.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK Day thành công", o));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê day nào", null));
                    }
                }
                else if (type == 2)
                {
                    var q1 = q.Where(i => i.year.Equals(year1));
                    var q2 = q1.Select(i => new { i.month, i.turnover }).ToList();
                    if (q2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK Month thành công", q2));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê month nào", null));
                    }
                }
                else
                {

                    var q1 = q.GroupBy(i => i.year)
                        .Select(x => new
                        {

                            year = x.Key,
                            turnover = x.Sum(t => t.turnover)
                        }).OrderBy(t => t.year);

                    if (q1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK Year thành công", q1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "null", null));
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/getStatisticalCurrentTime/{type}")]
        public HttpResponseMessage getStatisticalCurrentTime(int type)
        {

            try
            {

                var q = db.monthlyrevenues.ToList();

                if (type == 1)
                {
                    var q1 = q.Where(i => i.year.Equals(DateTime.Now.Year.ToString())).GroupBy(i => i.year)
                        .Select(x => new
                        {
                            year = x.Key,
                            turnover = x.Sum(t => t.turnover)
                        }).OrderBy(t => t.year);

                    if (q1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK current Year thành công", q1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê current year nào", null));
                    }
                }
                else if (type == 2)
                {
                    var q1 = q.Where(i => i.year.Equals(DateTime.Now.Year.ToString()) && i.month.Equals(DateTime.Now.Month.ToString()))
                        .Select(x => new
                        {
                            x.month,
                            x.year,
                            x.turnover
                        });

                    if (q1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK current Month thành công", q1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê month nào", null));
                    }

                }
                else if (type == 3)
                {
                    var o = db.orders.Where(i => i.statusid != 5 && i.payment == true && i.createdate.Day == DateTime.Now.Day && i.createdate.Month == DateTime.Now.Month && i.createdate.Year == DateTime.Now.Year).GroupBy(i => i.createdate.Day)
                        .Select(i => new { day = i.Key, month = i.Select(e => e.createdate.Month).FirstOrDefault(), year = i.Select(e => e.createdate.Year).FirstOrDefault(), turnover = i.Sum(t => t.total) }).ToList();
                    if (o.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK current day thành công", o));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê current day nào", null));
                    }

                }
                else
                {
                    var o = db.orders.Where(i => i.statusid != 5 && i.payment == true && i.createdate.Year == DateTime.Now.Year && (((i.createdate.Month - 1) / 3) + 1) == (((DateTime.Now.Month - 1) / 3) + 1)).GroupBy(i => ((i.createdate.Month - 1) / 3) + 1)
                        .Select(i => new { quarter = i.Key, year = i.Select(e => e.createdate.Year).FirstOrDefault(), turnover = i.Sum(t => t.total) }).ToList();
                    if (o.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK current day thành công", o));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê current day nào", null));
                    }
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/getStatisticalBrand/{day}/{month}/{year}/{time}")]
        public HttpResponseMessage getStatisticalBrand(int day, int month, int year, int time)
        {
            var month1 = month > 9 ? month.ToString() : "0" + month.ToString();
            try
            {
                var b = db.brands.ToList();
                var s = db.shoes.ToList();
                var q = db.monthlyrevenuedetails.ToList();


                if (time == 3)
                {
                    var q1 = q.Where(i => i.year.Equals(year.ToString())).ToList();
                    var query = (from ss in q1
                                 join sd in s on ss.shoeid equals sd.shoeid
                                 select new
                                 {
                                     ss,
                                     sd
                                 }).ToList();
                    var query1 = from s1 in b
                                 join ss in query on s1.brandid equals ss.sd.brandid into sd
                                 from subsaledetail in sd.DefaultIfEmpty()
                                 select new
                                 {
                                     s1.brandname,
                                     s1.brandid,

                                     sellnumber = subsaledetail?.ss.sellnumber ?? 0,
                                     turnover = subsaledetail?.ss.turnover ?? 0
                                 };
                    var query2 = query1.GroupBy(i => i.brandid)
                         .Select(i => new { brandname = i.Select(x => x.brandname).FirstOrDefault(), sellnumber = i.Sum(t => t.sellnumber), turnover = i.Sum(t => t.turnover) }).ToList();

                    if (query2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK brand in thành công", query2));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in year nào", null));
                    }
                }
                else
                if (time == 2)
                {
                    var q1 = q.Where(i => i.year.Equals(year.ToString()) && i.month.Equals(month1.ToString())).ToList();
                    var query = (from ss in q1
                                 join sd in s on ss.shoeid equals sd.shoeid
                                 select new
                                 {
                                     ss,
                                     sd
                                 }).ToList();
                    var query1 = from s1 in b
                                 join ss in query on s1.brandid equals ss.sd.brandid into sd
                                 from subsaledetail in sd.DefaultIfEmpty()
                                 select new
                                 {
                                     s1.brandname,
                                     s1.brandid,
                                     sellnumber = subsaledetail?.ss.sellnumber ?? 0,
                                     turnover = subsaledetail?.ss.turnover ?? 0
                                 };
                    var query2 = query1.GroupBy(i => i.brandid)
                         .Select(i => new { brandname = i.Select(x => x.brandname).FirstOrDefault(), sellnumber = i.Sum(t => t.sellnumber), turnover = i.Sum(t => t.turnover) }).ToList();

                    if (query2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK brand month thành công", query2));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in month nào", null));
                    }
                }
                else
                {
                    var o = db.orders.ToList();
                    var od = db.orderdetails.ToList();
                    var o1 = o.Where(i => i.statusid != 5 && i.payment == true && i.createdate.Year == year && i.createdate.Month == month && i.createdate.Day == day).ToList();
                    var query = (from ss in o1
                                 join sd in od on ss.orderid equals sd.orderid
                                 join sh in s on sd.shoeid equals sh.shoeid
                                 join b1 in b on sh.brandid equals b1.brandid/*into sd
                                 from subsaledetail in sd.DefaultIfEmpty()*/
                                 select new
                                 {

                                     sd.quantity,
                                     b1.brandid,
                                     b1.brandname,
                                     sd.total,
                                 }).ToList();
                    var query1 = query.GroupBy(i => i.brandid)
                        .Select(i => new { brandname = i.Select(x => x.brandname).FirstOrDefault(), sellnumber = i.Sum(t => t.quantity), turnover = i.Sum(t => t.total) }).ToList();
                    if (query1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK brand day thành công", query1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in day nào", null));
                    }

                }







            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [Route("api/Admin/getStatisticalShoes/{day}/{month}/{year}/{time}/{brand}")]
        public HttpResponseMessage getStatisticalShoes(int day, int month, int year, int time, int brand)
        {
            var month1 = month > 9 ? month.ToString() : "0" + month.ToString();
            try
            {
                var b = db.brands.ToList();
                var s = db.shoes.ToList();
                var q = db.monthlyrevenuedetails.ToList();
                var b1 = new List<brand>();
                var s1 = new List<shoes>();
                if (brand == 0)
                {
                    s1 = s;
                    b1 = b;
                }
                else
                {
                    var b2 = b.FirstOrDefault(i => i.brandid == brand);
                    var s2 = s.Where(i => i.brandid == b2.brandid).ToList();
                    s1 = s2;
                    b1.Add(b2);
                }
                if (time == 3)
                {

                    var q1 = q.Where(i => i.year.Equals(year.ToString())).ToList();
                    var query = (from ss in s1
                                 join sd in q1 on ss.shoeid equals sd.shoeid into sd
                                 from subsaledetail in sd.DefaultIfEmpty()

                                 select new
                                 {
                                     ss.shoeid,
                                     ss.shoename,
                                     sellnumber = subsaledetail?.sellnumber ?? 0,
                                     turnover = subsaledetail?.turnover ?? 0


                                 }).ToList();
                    var query1 = query.GroupBy(i => i.shoeid).Select(x => new { shoename = x.Select(i => i.shoename).FirstOrDefault(), sellnumber = x.Sum(t => t.sellnumber), turnover = x.Sum(t => t.turnover) }).ToList();
                    if (query1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK shoes in brand of year thành công", query1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in year nào", null));
                    }
                }
                else
                if (time == 2)
                {
                    var q1 = q.Where(i => i.year.Equals(year.ToString()) && i.month.Equals(month1)).ToList();
                    var query = (from ss in s1
                                 join sd in q1 on ss.shoeid equals sd.shoeid into sd
                                 from subsaledetail in sd.DefaultIfEmpty()

                                 select new
                                 {
                                     ss.shoeid,
                                     ss.shoename,
                                     sellnumber = subsaledetail?.sellnumber ?? 0,
                                     turnover = subsaledetail?.turnover ?? 0


                                 }).ToList();
                    var query1 = query.GroupBy(i => i.shoeid).Select(x => new { shoename = x.Select(i => i.shoename).FirstOrDefault(), sellnumber = x.Sum(t => t.sellnumber), turnover = x.Sum(t => t.turnover) }).ToList();
                    if (query1.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK shoes in brand of year thành công", query1));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in year nào", null));
                    }
                }
                else
                {
                    var o = db.orders.ToList();
                    var od = db.orderdetails.ToList();
                    var o1 = o.Where(i => i.statusid != 5 && i.payment == true && i.createdate.Year == year && i.createdate.Month == month && i.createdate.Day == day).ToList();
                    var query = (from ss in o1
                                 join sd in od on ss.orderid equals sd.orderid
                                 select new { sd }).ToList();
                    var query1 = (from sh in s1
                                  join sd in query on sh.shoeid equals sd.sd.shoeid into sd
                                  from subsaledetail in sd.DefaultIfEmpty()
                                  select new
                                  {
                                      sh.shoeid,
                                      sh.shoename,
                                      quantity = subsaledetail?.sd.quantity ?? 0,
                                      total = subsaledetail?.sd.total ?? 0

                                  }).ToList();

                    var query2 = query1.GroupBy(i => i.shoeid)
                        .Select(i => new { shoename = i.Select(x => x.shoename).FirstOrDefault(), sellnumber = i.Sum(t => t.quantity), turnover = i.Sum(t => t.total) }).ToList();
                    if (query2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy TK brand day thành công", query2));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có thống kê brand in day nào", null));
                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));

            }
        }
    }
}
