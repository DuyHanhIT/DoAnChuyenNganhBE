using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;
namespace WebApiShoesStoreDACN.Controllers
{
    public class OrderController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(order))]
        [Route("api/getOrderByAccountId/{accountid}")]
        public HttpResponseMessage getOrderByAccountId(int accountid)
        {

            try
            {
                var b = db.orders.AsQueryable();
                var lstOrder = b.Where(s => s.accountid == accountid).ToList();
                var fillLstOrrder = lstOrder.Select(bb => new
                {
                    bb.orderid,
                    bb.accountid,
                    bb.statusid,
                    bb.status.statusname,
                    bb.createdate,
                    bb.deliverydate,
                    bb.firstName,
                    bb.lastName,
                    bb.phone,
                    bb.email,
                    bb.note,
                    bb.total,
                    bb.payment,
                    bb.momo,
                    bb.address
                });
                if (fillLstOrrder.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy đơn hàng thành công", fillLstOrrder));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có đơn hàng nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(order))]
        [Route("api/AddOrder")]
        public HttpResponseMessage AddOrder(OrderAndOrderdetails lstO)
        {

            try
            {
                var o = lstO.order;
                if (o.accountid == null || o.firstName == null || o.firstName.Equals("") || o.phone == null || o.phone.Equals("") || o.email == null ||
                    o.email.Equals("") || o.lastName == null || o.lastName.Equals("") || o.address == null || o.address.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));
                }
                foreach (var item in lstO.lstOrderDetails)
                {
                    if (item.shoeid == null || item.quantity == null || item.size == null || item.size.Equals("") || item.price == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    if (item.quantity <= 0 || item.price <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Giá, Số lượng phải >=0", null));
                    }
                    var shid = item.shoeid;
                    var p = db.sizetables.Where(i => i.shoeid == shid);
                    var s38 = p.Select(i => i.s38).FirstOrDefault(); var s39 = p.Select(i => i.s39).FirstOrDefault(); var s40 = p.Select(i => i.s40).FirstOrDefault();
                    var s41 = p.Select(i => i.s41).FirstOrDefault(); var s42 = p.Select(i => i.s42).FirstOrDefault(); var s43 = p.Select(i => i.s43).FirstOrDefault();
                    var s44 = p.Select(i => i.s44).FirstOrDefault(); var s45 = p.Select(i => i.s45).FirstOrDefault(); var s46 = p.Select(i => i.s46).FirstOrDefault();
                    var s47 = p.Select(i => i.s47).FirstOrDefault(); var s48 = p.Select(i => i.s48).FirstOrDefault();
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
                        if (si38 > s38)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size39.Count() > 0)
                    {
                        int si39 = (int)size39.FirstOrDefault();
                        if (si39 > s39)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size40.Count() > 0)
                    {
                        int si40 = (int)size40.FirstOrDefault();
                        if (si40 > s40)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size41.Count() > 0)
                    {
                        int si41 = (int)size41.FirstOrDefault();
                        if (si41 > s41)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size42.Count() > 0)
                    {
                        int si42 = (int)size42.FirstOrDefault();
                        if (si42 > s42)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size43.Count() > 0)
                    {
                        int si43 = (int)size43.FirstOrDefault();
                        if (si43 > s43)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size44.Count() > 0)
                    {
                        int si44 = (int)size44.FirstOrDefault();
                        if (si44 > s44)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size45.Count() > 0)
                    {
                        int si45 = (int)size45.FirstOrDefault();
                        if (si45 > s45)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size46.Count() > 0)
                    {
                        int si46 = (int)size46.FirstOrDefault();
                        if (si46 > s46)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size47.Count() > 0)
                    {
                        int si47 = (int)size47.FirstOrDefault();
                        if (si47 > s47)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                    if (size48.Count() > 0)
                    {
                        int si48 = (int)size48.FirstOrDefault();
                        if (si48 > s48)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Size {item.size} của mã giày {item.shoeid} không đủ số lượng", null));
                        }
                    }
                }

                order or = new order();
                or.accountid = o.accountid;
                or.statusid = 1;
                or.createdate = DateTime.Now;
                or.deliverydate = DateTime.Now.AddDays(3);
                or.firstName = o.firstName;
                or.lastName = o.lastName;
                or.phone = o.phone;
                or.email = o.email;
                or.note = o.note;
                or.total = 0;
                or.payment = false;
                or.momo = o.momo;
                or.address = o.address;
                db.orders.Add(or);
                db.SaveChanges();
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                string month1 = month.ToString();
                string year1 = year.ToString();
                var checkMonthlyRevenue = db.monthlyrevenues.Where(i => i.month.Equals(month1) && i.year.Equals(year1)).ToList();
                if (checkMonthlyRevenue.Count() < 1)
                {
                    monthlyrevenue m = new monthlyrevenue();
                    m.month = month1;
                    m.year = year1;
                    db.monthlyrevenues.Add(m);
                    db.SaveChanges();
                }


                foreach (var item in lstO.lstOrderDetails)
                {

                    orderdetail obj = new orderdetail();
                    obj.orderid = or.orderid;
                    obj.shoeid = item.shoeid;
                    obj.quantity = item.quantity;
                    obj.size = item.size;
                    obj.price = item.price;
                    obj.total = (item.price * item.quantity);
                    db.orderdetails.Add(obj);
                    db.SaveChanges();

                    int month2 = DateTime.Now.Month;
                    int year2 = DateTime.Now.Year;
                    string month3 = month.ToString();
                    string year3 = year.ToString();
                    var checkMonthlyRevenueDetails = db.monthlyrevenuedetails.Where(i => i.month.Equals(month3) && i.year.Equals(year3) && i.shoeid == obj.shoeid).ToList();
                    if (checkMonthlyRevenueDetails.Count() < 1)
                    {
                        monthlyrevenuedetail m1 = new monthlyrevenuedetail();
                        m1.shoeid = (int)obj.shoeid;
                        m1.month = month3;
                        m1.year = year3;
                        db.monthlyrevenuedetails.Add(m1);
                        db.SaveChanges();
                    }
                    Thread.Sleep(500);

                }
                var lod = new List<order>();
                lod.Add(or);
                var lord = lod.Select(i => new
                {
                    i.orderid,
                    i.accountid,
                    i.statusid,
                    i.createdate,
                    i.deliverydate,
                    i.firstName,
                    i.lastName,
                    i.phone,
                    i.email,
                    i.note,
                    i.total,
                    i.payment,
                    i.momo,
                    i.address
                }).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm đơn hàng có mã \"{or.orderid}\" thành công", lord));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }
        [Authorize]
        [HttpPut]
        [ResponseType(typeof(order))]
        [Route("api/UpdateMomoAndPaymentOfOrder/{orderid}")]
        public HttpResponseMessage UpdateMomoAndPaymentOfOrder(int orderid, order ep)
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
                    if (ep.momo == null || ep.momo.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Bạn chưa truyền token momo", null));
                    }
                    var g = db.orders.FirstOrDefault(i => i.orderid == orderid);
                    g.momo = ep.momo;
                    g.payment = true;

                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();


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


                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công đơn hàng có mã \"{ep.orderid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }
        [Authorize]
        [HttpPut]
        [ResponseType(typeof(order))]
        [Route("api/CancelOrder/{orderid}")]
        public HttpResponseMessage CancelOrder(int? orderid)
        {
            try
            {
                if (orderid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Bạn chưa truyền mã đơn hàng", null));
                }
                var g = db.orders.FirstOrDefault(i => i.orderid == orderid);
                if (g.statusid == 3)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Đơn hàng đang giao, bạn liên hệ cửa hàng để hủy đơn ", null));
                }
                if (g.statusid == 4)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Đơn hàng đã giao hoàn tất, không thể hủy, mọi vấn đề vui lòng liên hệ cửa hàng ", null));
                }
                if (g.statusid == 5)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Đơn hàng đã được hủy ", null));
                }
                else
                {
                    g.statusid = 5;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    if (g.payment == true)
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
                                    p.s38 = p.s38 + si38;
                                }
                                if (size39.Count() > 0)
                                {
                                    int si39 = (int)size39.FirstOrDefault();
                                    p.s39 = p.s39 + si39;
                                }
                                if (size40.Count() > 0)
                                {
                                    int si40 = (int)size40.FirstOrDefault();
                                    p.s40 = p.s40 + si40;
                                }
                                if (size41.Count() > 0)
                                {
                                    int si41 = (int)size41.FirstOrDefault();
                                    p.s41 = p.s41 + si41;

                                }
                                if (size42.Count() > 0)
                                {
                                    int si42 = (int)size42.FirstOrDefault();
                                    p.s42 = p.s42 + si42;

                                }
                                if (size43.Count() > 0)
                                {
                                    int si43 = (int)size43.FirstOrDefault();
                                    p.s43 = p.s43 + si43;

                                }
                                if (size44.Count() > 0)
                                {
                                    int si44 = (int)size44.FirstOrDefault();
                                    p.s44 = p.s44 + si44;

                                }
                                if (size45.Count() > 0)
                                {
                                    int si45 = (int)size45.FirstOrDefault();
                                    p.s45 = p.s45 + si45;
                                }
                                if (size46.Count() > 0)
                                {
                                    int si46 = (int)size46.FirstOrDefault();
                                    p.s46 = p.s46 + si46;
                                }
                                if (size47.Count() > 0)
                                {
                                    int si47 = (int)size47.FirstOrDefault();
                                    p.s47 = p.s47 + si47;
                                }
                                if (size48.Count() > 0)
                                {
                                    int si48 = (int)size48.FirstOrDefault();
                                    p.s48 = p.s48 + si48;
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
                    var b = db.orders.AsQueryable();
                    var lstOrder = b.Where(s => s.accountid == g.accountid).ToList();

                    var fillLstOrrder = lstOrder.Select(bb => new
                    {
                        bb.orderid,
                        bb.accountid,
                        bb.statusid,
                        bb.status.statusname,
                        bb.createdate,
                        bb.deliverydate,
                        bb.firstName,
                        bb.lastName,
                        bb.phone,
                        bb.email,
                        bb.note,
                        bb.total,
                        bb.payment,
                        bb.momo,
                        bb.address
                    });
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Hủy thành công đơn hàng có mã \"{g.orderid}\"", fillLstOrrder));
                }




            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }

        [Authorize]
        [HttpDelete]
        [ResponseType(typeof(order))]
        [Route("api/DeleteOrder/{orderid}")]
        public HttpResponseMessage DeleteOrder(int? orderid)
        {
            try
            {
                if (orderid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Bạn chưa truyền mã đơn hàng", null));
                }
                var g = db.orders.FirstOrDefault(i => i.orderid == orderid);
                if (g == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy đơn hàng", null));
                }
                else
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
                                p.s38 = p.s38 + si38;
                            }
                            if (size39.Count() > 0)
                            {
                                int si39 = (int)size39.FirstOrDefault();
                                p.s39 = p.s39 + si39;
                            }
                            if (size40.Count() > 0)
                            {
                                int si40 = (int)size40.FirstOrDefault();
                                p.s40 = p.s40 + si40;
                            }
                            if (size41.Count() > 0)
                            {
                                int si41 = (int)size41.FirstOrDefault();
                                p.s41 = p.s41 + si41;

                            }
                            if (size42.Count() > 0)
                            {
                                int si42 = (int)size42.FirstOrDefault();
                                p.s42 = p.s42 + si42;

                            }
                            if (size43.Count() > 0)
                            {
                                int si43 = (int)size43.FirstOrDefault();
                                p.s43 = p.s43 + si43;

                            }
                            if (size44.Count() > 0)
                            {
                                int si44 = (int)size44.FirstOrDefault();
                                p.s44 = p.s44 + si44;

                            }
                            if (size45.Count() > 0)
                            {
                                int si45 = (int)size45.FirstOrDefault();
                                p.s45 = p.s45 + si45;
                            }
                            if (size46.Count() > 0)
                            {
                                int si46 = (int)size46.FirstOrDefault();
                                p.s46 = p.s46 + si46;
                            }
                            if (size47.Count() > 0)
                            {
                                int si47 = (int)size47.FirstOrDefault();
                                p.s47 = p.s47 + si47;
                            }
                            if (size48.Count() > 0)
                            {
                                int si48 = (int)size48.FirstOrDefault();
                                p.s48 = p.s48 + si48;
                            }
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                            var pu = db.shoes.FirstOrDefault(i => i.shoeid == item.shoeid);
                            pu.purchased = pu.purchased - item.quantity;
                            db.Entry(pu).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    var t = db.orderdetails.Where(i => i.orderid == orderid).ToList();
                    db.orderdetails.RemoveRange(t);

                    db.SaveChanges();
                    db.orders.Remove(g);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Xóa thành công đơn hàng có mã \"{orderid}\"", null));
                }




            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }

    }///getOrder/{accountid}
}
