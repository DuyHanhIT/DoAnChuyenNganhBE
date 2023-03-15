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
    public class OrderAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/GetAllOrders/{keysearch}")]
        public HttpResponseMessage GetAllOrders(string keysearch)
        {
            try
            {
                var d = db.orders.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();
                var lstOrder = s.Where(x => x.orderid.ToString().Contains(keysearch) || x.phone.ToLower().Contains(keysearch.ToLower()) || x.firstName.ToLower().Contains(keysearch.ToLower()))
                    .Select(ss => new
                    {
                        ss.orderid,
                        ss.accountid,
                        ss.account.username,
                        ss.firstName,
                        ss.lastName,
                        ss.statusid,
                        ss.status.statusname,
                        ss.createdate,
                        ss.deliverydate,
                        ss.phone,
                        ss.email,
                        ss.note,
                        ss.total,
                        ss.payment,
                        ss.momo,
                        ss.address
                    }).OrderBy(i=>i.statusid);
                if (lstOrder.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả đơn hàng thành công", lstOrder));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có đơn hàng nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/Admin/GetOrderByOrderId/{orderid}")]
        public HttpResponseMessage GetOrderByOrderId(int orderid)
        {
            try
            {
                var d = db.orders.AsQueryable();

                var s = d.Where(x => x.orderid == orderid).ToList();
                var lstOrder = s.Select(ss => new
                {
                    ss.orderid,
                    ss.accountid,
                    ss.account.username,
                    ss.firstName,
                    ss.lastName,
                    ss.statusid,
                    ss.status.statusname,
                    ss.createdate,
                    ss.deliverydate,
                    ss.phone,
                    ss.email,
                    ss.note,
                    ss.total,
                    ss.payment,
                    ss.momo,
                    ss.address
                });

                if (lstOrder.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy đơn hàng by orderId thành công", lstOrder));
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có đơn hàng nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpPut]
        [ResponseType(typeof(order))]
        [Route("api/Admin/UpdateOrder/{orderid}")]
        public HttpResponseMessage UpdateOrder(int orderid, order ep)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (orderid != ep.orderid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy đơn hàng", null));
                }
                else
                {

                    if (ep.statusid == 4)
                    {
                        ep.deliverydate = DateTime.Now;
                        ep.payment = true;


                    }
                   



                    var g = db.orders.FirstOrDefault(i => i.orderid == orderid);

                    if (g.statusid == 4)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Đơn hàng đã giao thành công, không được điều chỉnh?", null));
                    }
                    else
                    if (g.statusid == 5)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Đơn hàng đã hủy, không được điều chỉnh?", null));
                    }
                    g.statusid = ep.statusid;
                    g.payment = ep.payment==null?g.payment:ep.payment;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    if (g.statusid == 4 && (g.momo == null || g.momo.Length < 1))
                    {

                        DateTime cre = (DateTime)g.createdate;
                        int month = cre.Month;
                        int year = cre.Year;
                        var getOD = db.orderdetails.Where(i => i.orderid == orderid).Sum(t => t.quantity);
                        int sellnumber = (int)getOD;
                        var m = db.monthlyrevenues.FirstOrDefault(i => i.month.Equals(month.ToString()) && i.year.Equals(year.ToString()));
                        var sellnumber1 = m.sellnumber == null ? m.sellnumber = 0 : m.sellnumber;
                        var turnover1 = m.turnover == null ? m.turnover = 0 : m.turnover;
                        m.sellnumber = sellnumber1 + sellnumber;
                        m.turnover = turnover1 + g.total;
                        db.Entry(m).State = EntityState.Modified;
                        db.SaveChanges();

                        var od = db.orderdetails.Where(i => i.orderid == orderid).ToList();
                        foreach (var od1 in od)
                        {
                            var md = db.monthlyrevenuedetails.FirstOrDefault(i => i.month.Equals(month.ToString()) && i.year.Equals(year.ToString()) && i.shoeid == od1.shoeid);
                            var sellnumber2 = md.sellnumber == null ? md.sellnumber = 0 : md.sellnumber;
                            var turnover2 = md.turnover == null ? md.turnover = 0 : md.turnover;
                            md.sellnumber = sellnumber2 + od1.quantity;
                            md.turnover = turnover2 + od1.total;
                            db.Entry(md).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                   
                    if ( g.payment == true && g.statusid == 5)
                    {
                        DateTime cre = (DateTime)g.createdate;
                        int month = cre.Month;
                        int year = cre.Year;
                        var getOD = db.orderdetails.Where(i => i.orderid == orderid).Sum(t => t.quantity);
                        int sellnumber = (int)getOD;
                        var m = db.monthlyrevenues.FirstOrDefault(i => i.month.Equals(month.ToString()) && i.year.Equals(year.ToString()));
                        var sellnumber1 = m.sellnumber == null ? m.sellnumber = 0 : m.sellnumber;
                        var turnover1 = m.turnover == null ? m.turnover = 0 : m.turnover;
                        m.sellnumber = sellnumber1 - sellnumber;
                        m.turnover = turnover1 - g.total;
                        db.Entry(m).State = EntityState.Modified;
                        db.SaveChanges();

                        var od = db.orderdetails.Where(i => i.orderid == orderid).ToList();
                        foreach (var od1 in od)
                        {
                            var md = db.monthlyrevenuedetails.FirstOrDefault(i => i.month.Equals(month.ToString()) && i.year.Equals(year.ToString()) && i.shoeid == od1.shoeid);
                            var sellnumber2 = md.sellnumber == null ? md.sellnumber = 0 : md.sellnumber;
                            var turnover2 = md.turnover == null ? md.turnover = 0 : md.turnover;
                            md.sellnumber = sellnumber2 - od1.quantity;
                            md.turnover = turnover2 - od1.total;
                            db.Entry(md).State = EntityState.Modified;
                            db.SaveChanges();
                        }


                    }
                    if (g.statusid == 5)
                    {
                        var od = db.orderdetails.Where(i => i.orderid == g.orderid).ToList();
                        if (od.Count() > 0)
                        {

                            foreach (var item in od)
                            {
                                var p = db.sizetables.Where(i => i.shoeid == item.shoeid).FirstOrDefault();
                                
                                orderdetail j = new orderdetail();
                                j = item;
                                var lsod = new List<orderdetail>();
                                lsod.Add(j);
                                var size38 = lsod.Where(i => i.size.ToString().Contains("38")).Select(i => i.quantity);
                                var size39 = lsod.Where(i => i.size.ToString().Contains("39")).Select(i => i.quantity);
                                var size40 = lsod.Where(i => i.size.ToString().Contains("40")).Select(i => i.quantity);
                                var size41 = lsod.Where(i => i.size.ToString().Contains("41")).Select(i => i.quantity);
                                var size42 = lsod.Where(i => i.size.ToString().Contains("42")).Select(i => i.quantity);
                                var size43 = lsod.Where(i => i.size.ToString().Contains("43")).Select(i => i.quantity);
                                var size44 = lsod.Where(i => i.size.ToString().Contains("44")).Select(i => i.quantity);
                                var size45 = lsod.Where(i => i.size.ToString().Contains("45")).Select(i => i.quantity);
                                var size46 = lsod.Where(i => i.size.ToString().Contains("46")).Select(i => i.quantity);
                                var size47 = lsod.Where(i => i.size.ToString().Contains("47")).Select(i => i.quantity);
                                var size48 = lsod.Where(i => i.size.ToString().Contains("48")).Select(i => i.quantity);
                                if (size38.Count() > 0)
                                {
                                    int si38 = (int)size38.FirstOrDefault();
                                    p.s38 = p.s38+si38;
                                }
                                if (size39.Count() > 0)
                                {
                                    int si39 = (int)size39.FirstOrDefault();
                                    p.s39 = p.s39+ si39;
                                }
                                if (size40.Count() > 0)
                                {
                                    int si40 = (int)size40.FirstOrDefault();
                                    p.s40 =p.s40+ si40;
                                }
                                if (size41.Count() > 0)
                                {
                                    int si41 = (int)size41.FirstOrDefault();
                                    p.s41 = p.s41+si41;

                                }
                                if (size42.Count() > 0)
                                {
                                    int si42 = (int)size42.FirstOrDefault();
                                    p.s42 =p.s42+ si42;

                                }
                                if (size43.Count() > 0)
                                {
                                    int si43 = (int)size43.FirstOrDefault();
                                    p.s43 =p.s43+ si43;

                                }
                                if (size44.Count() > 0)
                                {
                                    int si44 = (int)size44.FirstOrDefault();
                                    p.s44 =p.s44+ si44;

                                }
                                if (size45.Count() > 0)
                                {
                                    int si45 = (int)size45.FirstOrDefault();
                                    p.s45 =p.s45+ si45;
                                }
                                if (size46.Count() > 0)
                                {
                                    int si46 = (int)size46.FirstOrDefault();
                                    p.s46 =p.s46+ si46;
                                }
                                if (size47.Count() > 0)
                                {
                                    int si47 = (int)size47.FirstOrDefault();
                                    p.s47 =p.s47+ si47;
                                }
                                if (size48.Count() > 0)
                                {
                                    int si48 = (int)size48.FirstOrDefault();
                                    p.s48 = p.s48+si48;
                                }
                                db.Entry(p).State = EntityState.Modified;
                                db.SaveChanges();
                                var pu = db.shoes.FirstOrDefault(i => i.shoeid == item.shoeid);
                                pu.purchased = pu.purchased - item.quantity;
                                db.Entry(pu).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công đơn hàng có mã \"{ep.orderid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }

        [Authorize(Roles = "1,2")]
        [HttpPut]
        [ResponseType(typeof(order))]
        [Route("api/Admin/UpdateOrderTest/{orderid}")]
        public HttpResponseMessage UpdateOrderTest(int orderid, order ep)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (orderid != ep.orderid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy đơn hàng", null));
                }
                else
                {

                    if (ep.statusid == 4)
                    {
                        ep.deliverydate = DateTime.Now;
                    }



                    var g = db.orders.FirstOrDefault(i => i.orderid == orderid);

                    if (g.statusid == 4)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Đơn hàng đã giao thành công, không được điều chỉnh?", null));
                    }
                    else
                    if (g.statusid == 5)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Đơn hàng đã hủy, không được điều chỉnh?", null));
                    }
                    g.statusid = ep.statusid;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();

                    /*foreach (var visitobjs in ep.lsod)
                    {
                        orderdetail obj = new orderdetail();
                        obj.price = visitobjs.price;
                    }*/
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công đơn hàng có mã \"{ep.orderid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }
    }
}
