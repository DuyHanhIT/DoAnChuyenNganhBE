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
namespace WebApiShoesStoreDACN.Controllers
{
    public class UserController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

       public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [ResponseType(typeof(user))]
        [Route("api/insertUser")]
        public HttpResponseMessage PostUser(user u)
        {
            try
            {
                //u.userid
                int accountid = u.accountid;
                string firstName = u.firstName;//123123123
                string lastName = u.lastName;
                string phone = u.phone;
                string email = u.email;
                string address = u.address;
                string avatar = u.avatar; 
                // var f_password = GetMD5(acc.password);
                var existUser = db.users.Where(a => a.accountid== accountid);
                var existAccont = db.accounts.Where(b => b.accountid == accountid);
                if (existAccont.Count()<= 0 )
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Không có tài khản có mã: {u.accountid}!", null));
                }else
                if (accountid == 0 || firstName ==null||firstName.Equals("")||lastName==null|| lastName.Equals("") ||
                    phone == null || phone.Equals("") || email == null || email.Equals("") || address == null || address.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin!", null));
                }
                else
                if (existUser.Count() > 0 )
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Thông tin người dùng đã tồn tại!", null));
                }
                else if (IsValidEmail(email) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng nhập đúng địnhn dạng email", null));
                }
                else
                {
                    try
                    {
                        if (avatar == null || avatar.Equals(""))
                        {
                            avatar = "https://sv3.anh365.com/images/2022/10/25/image7d52779535c30442.png";
                        }
                        user us = new user();
                        us.accountid = accountid;
                        us.firstName = firstName;
                        us.phone = phone;
                        us.email = email;
                        us.lastName = lastName;
                        us.address = address;
                        us.avatar = avatar;
                        
                        var lstUser = new List<user>();
                        lstUser.Add(us);
                        var lstUser1 = lstUser.Select(x => new { x.userid, x.accountid, x.firstName, x.lastName, x.phone, x.email, x.avatar,x.address });
                        db.users.Add(us);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Thêm thông tin người dùng thành công", lstUser1));                
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
        // PUT: api/ACCOUNTs/5
        //[ResponseType(typeof(void))]
        [Authorize]
        [HttpPut]
        [ResponseType(typeof(user))]
        [Route("api/updateUserByUserId/{userid}")]
        public HttpResponseMessage PutUserByUserId(int userid, user u)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (userid != u.userid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy user", null));
                }
                else
                {
                    string firstName = u.firstName;
                    string lastName = u.lastName;
                    string phone = u.phone;
                    string email = u.email;
                    string address = u.address;
                    string avatar = u.avatar;
                    if (firstName == null || firstName.Equals("") || lastName == null || lastName.Equals("") ||
                        phone == null || phone.Equals("") || email == null || email.Equals("") || address == null || address.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin!", null));
                    }
                    else 
                    {
                        if (avatar == null || avatar.Equals(""))
                        {
                            avatar = "https://sv3.anh365.com/images/2022/10/25/image7d52779535c30442.png";
                            u.avatar = avatar;
                        }
                        db.Entry(u).State = EntityState.Modified;
                        try
                        {

                            var lstUser = new List<user>();
                            lstUser.Add(u);
                            var lstUser1 = lstUser.Select(x => new { x.userid, x.accountid, x.firstName, x.lastName, x.phone, x.email, x.avatar, x.address });
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Cập nhật thành công", lstUser1));
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
                        }
                    }
                    
                }
            }
            catch (Exception ex){
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
            }
        }

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(user))]
        [Route("api/getUserByAccountId/{accountid}")]
        public HttpResponseMessage getUserByAccountId(int accountid)
        {

            try
            {
                var s = db.users.AsQueryable();
                //if(!string.IsNullOrEmpty(shoesid))
                var b = s.Where(a => a.accountid == accountid).ToList();
                var lstUserById = b.Select(ss => new
                {
                    ss.userid,
                    ss.accountid,
                    ss.firstName,
                    ss.lastName,
                    ss.phone,
                    ss.email,
                    ss.address,
                    ss.avatar,
                });
                if (lstUserById.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy user có mã = {accountid} thành công", lstUserById));
                }// $"Get shoes by id = {shoesid} success"
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, $"Không có user có mã = {accountid}", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        /*
         if (ModelState.IsValid)
            {
                var E_Account = db.ACCOUNTs.First(m => m.IdAccount == id);

                if (E_Account != null)
                {
                    var OldPassword = collection["OldPassword"];
                    var NewPassword = collection["NewPassword"];
                    var ReNewPassword = collection["ReNewPassword"];
                    var f_Oldpassword = GetMD5(OldPassword);
                    E_Account.IdAccount = id;
                    if (OldPassword == null || OldPassword == "")
                    {
                        ViewData["NhapMKC"] = "Phải nhập mật khẩu cũ ";
                    }
                    else
                    if (!f_Oldpassword.Equals(E_Account.Password))
                    {
                        ViewData["NhapMKCSai"] = "Mật khẩu cũ không đúng";
                    }
                    else
                    if (NewPassword == null || NewPassword == "")
                    {
                        ViewData["NhapMKM"] = "Phải nhập mật khẩu mới ";
                    }
                    else
                    if (ReNewPassword == null || ReNewPassword == "")
                    {
                        ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận ";
                    }
                    else
                    {
                        if (!NewPassword.Equals(ReNewPassword))
                        {
                            ViewData["MatKhauGiongNhau"] = "Mật khẩu mới và mật khẩu xác nhận phải giống nhau";
                        }
                        else
                        {

                            E_Account.Password = NewPassword;
                            E_Account.Password = GetMD5(E_Account.Password);
                            UpdateModel(E_Account);
                            db.ACCOUNTs.AddOrUpdate(E_Account);
                            db.SaveChanges();
                            Logout();
                            return RedirectToAction("DangNhap");
                        }
                    }
                }
                else
                {
                    ViewData["TenNguoiDungDaTonTai"] = "Có lỗi xảy ra khi thực hiện thao tác";
                    return View("DoiMatKhau");
                }
            }
            return this.DoiMatKhau(id);*/
        /*[Authorize]
        [HttpGet]
        [ResponseType(typeof(orderdetail))]
        [Route("api/getOrderDetails/{orderid}")]
        public HttpResponseMessage getOrderDetails(int orderid)
        {
            try
            {
                var b = db.orderdetails.AsQueryable();
                var lstOrderDetails = b.Where(s => s.orderid == orderid).ToList();
                var lstOrder = db.orders.Where(w => w.orderid == orderid);
                var lstShoes = db.shoes.ToList();
                var fillLstOrrderDetails1 = lstOrderDetails.Select(bb => new
                {
                    bb.orderid,
                    bb.shoeid,
                    bb.quantity,
                    bb.size,
                    bb.price,
                    bb.total

                });
                var fillLstOrrderDetails = (from a in lstOrderDetails
                                            join c in lstOrder on a.orderid equals c.orderid
                                            join t in lstShoes on a.shoeid equals t.shoeid
                                            select new
                                            {
                                                a.orderid,
                                                a.shoeid,
                                                a.quantity,
                                                a.size,
                                                a.price,
                                                a.total,
                                                c.status.statusname,
                                                c.statusid,
                                                t.shoename,
                                                t.image1

                                            });
                if (fillLstOrrderDetails.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy chi tiết đơn hàng thành công", fillLstOrrderDetails));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có chi tiết đơn hàng nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }*/
    }
}
