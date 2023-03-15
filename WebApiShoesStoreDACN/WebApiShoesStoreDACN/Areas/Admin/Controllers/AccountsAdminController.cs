using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Controllers;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.ControllerAdmin
{
    public class AccountsAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();



        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAllAccount/{keysearch}")]
        public HttpResponseMessage GetAllAccounts(string keysearch)
        {
            var d = db.accounts.AsQueryable();
            if (keysearch.Equals("="))
            {
                var s = d.ToList();
                var lstacc = s.Select(ss => new
                {
                    ss.accountid,
                    ss.username,
                    ss.password,
                    ss.roleid,
                    ss.active,
                    ss.createdate,
                    ss.role.rolename
                });
                try
                {
                    if (lstacc.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get all account success", lstacc));
                        //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest);

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all account faild", null));
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all account faild", null));

                }
            }
            else
            {
                var ds = d.Where(x => x.username.ToLower().Contains(keysearch.ToLower())).ToList();
                var lstacc = ds.Select(ss => new
                {
                    ss.accountid,
                    ss.username,
                    ss.password,
                    ss.roleid,
                    ss.active,
                    ss.createdate,
                    ss.role.rolename
                });
                try
                {
                    if (lstacc.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Có {lstacc.Count()} kết quả nào phù hợp với từ khóa \"{ keysearch }\"", lstacc));
                        //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest);

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, $"Không có kết quả nào phù hợp với từ khóa {keysearch}", null));
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all search faild", null));

                }
            }

        }
        /*[Authorize]*/
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAccountByAccountId/{accountid}")]
        public HttpResponseMessage GetAccountByAccountId(int? accountid)
        {
            var s = db.accounts.AsQueryable();
            if (accountid == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 404, "Bạn chưa truyền accountid", null));

            }
            else
            {
                var lstacc = s.Where(i => i.accountid == accountid).Select(ss => new
                {
                    ss.accountid,
                    ss.username,
                    ss.password,
                    ss.roleid,
                    ss.active,
                    ss.createdate,
                    ss.role.rolename
                });
                try
                {
                    if (lstacc.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get account by accountid success", lstacc));
                        //return Request.CreateResponse(HttpStatusCode.OK, lstacc);


                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.BadRequest);

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get account by accountid account faild", null));
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all account faild", null));

                }
            }

        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/AddAccount")]
        public HttpResponseMessage PostRegister(RegiterModel register)
        {
            try
            {
                if (register.roleid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng chọn phân quyền", null));
                }
                string username = register.username;
                /*string pass = register.password;//123123123
                string repass = register.Repassword;*/
                int roleId = (int)register.roleid;
                string pass = "0";

                // var f_password = GetMD5(acc.password);
                var isLogin = db.accounts.Where(a => a.username.Equals(username));
                var checkEmail = new UserController().IsValidEmail(username);

                if (checkEmail == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng nhập đúng địnhn dạng email", null));

                }
                else

                if (isLogin.Count() > 0)
                {
                    /*var s = isLogin.Select(a => new { a.accountid, a.roleid, a.username, a.active, a.createdate });*/
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên người dùng đã tồn tại", null));
                }
                if (roleId > 3)
                {
                    /*var s = isLogin.Select(a => new { a.accountid, a.roleid, a.username, a.active, a.createdate });*/
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Sai phân quyền", null));
                }


                /*else if (!pass.Equals(repass))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Mật khẩu không trùng khớp", null));
                }*/
                else
                {
                    try
                    {
                        var f_pass = new AccountController().GetMD5(pass);
                        account acc = new account();
                        acc.username = username;
                        acc.password = f_pass;
                        acc.roleid = roleId;
                        acc.createdate = DateTime.Now;
                        acc.active = false;
                        db.accounts.Add(acc);
                        db.SaveChanges();
                        var lstacc = new List<account>();
                        lstacc.Add(acc);
                        var lstacc1 = lstacc.Select(x => new { x.accountid, x.username });
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Đăng ký thành công", lstacc1));
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình đăng ký", null));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đăng ký thất bại", null));
            }
        }
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/UpdateAccount/{accountId}")]
        public HttpResponseMessage PutAccount(int accountId, account acc)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (accountId != acc.accountid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy tài khoản", null));
                }
                else
                {

                    var g = db.accounts.FirstOrDefault(i => i.accountid == accountId);
                    int roleId = acc.roleid;
                    bool active = acc.active;
                    g.roleid = roleId;
                    g.active = active;
                    DateTime createDate = (DateTime)g.createdate;

                    if (roleId == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin!", null));
                    }
                    if (roleId <= 0 || roleId > 3)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Sai phân quyền", null));
                    }
                    else
                    {
                        db.Entry(g).State = EntityState.Modified;
                        try
                        {

                            var lstAcc = new List<account>();
                            lstAcc.Add(g);
                            var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });


                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Cập nhật thành công", lstUser1));
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
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/DeleteAccount/{accountId}")]
        public HttpResponseMessage DeleteAccount(int accountId, account acc)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (accountId != acc.accountid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy tài khoản", null));
                }
                else
                {

                    var g = db.accounts.FirstOrDefault(i => i.accountid == accountId);

                    g.active = !g.active;

                    try
                    {
                        db.Entry(g).State = EntityState.Modified;
                        var lstAcc = new List<account>();
                        lstAcc.Add(g);
                        var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });


                        db.SaveChanges();
                        if (g.active == false)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Vô hiệu hóa tài khoản {g.username} thành công", lstUser1));
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Mở khóa tài khoản {g.username} thành công", lstUser1));
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
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
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/ResetPassword/{accountId}")]
        public HttpResponseMessage ResetPassword(int accountId, account acc)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (accountId != acc.accountid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy tài khoản", null));
                }
                else
                {

                    var g = db.accounts.FirstOrDefault(i => i.accountid == accountId);

                    var newMD5 = new AccountController().GetMD5("0");
                    g.password = newMD5;


                    try
                    {
                        db.Entry(g).State = EntityState.Modified;
                        var lstAcc = new List<account>();
                        lstAcc.Add(g);
                        var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });


                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Đặt lại mật khẩu cho tài khoản {g.username} thành công", lstUser1));


                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
            }
        }
        // Lấy những account chưa đang ký cho employee
        //[Authorize]
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/GetAllAccountNotExistsINEmployee")]
        public HttpResponseMessage GetAllAccountNotExistsINEmployee()
        {
            var d = db.accounts.AsQueryable();

            var t = d.Where(i => i.roleid < 3 && i.active == true).ToList();

            var g = from f in t
                    where !db.employees.Any(es => (es.accountid == f.accountid) && (f.roleid < 3))
                    select new
                    {
                        f.accountid,
                        f.username
                    };



            /* var lstacc = s.Select(ss => new
                 {
                     ss.accountid,
                     ss.username,
                     ss.password,
                     ss.roleid,
                     ss.active,
                     ss.createdate,
                     ss.role.rolename
                 });*/
            try
            {
                if (g.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Get all account success", g));
                    //return Request.CreateResponse(HttpStatusCode.OK, lstacc);
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all account faild", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Get all account faild 1", null));

            }
        }
        [HttpPost]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Admin/Login")]
        public HttpResponseMessage Login(account acc)
        {
            try
            {
                var f_password = acc.password; /*GetMD5(acc.password);*/
                var isLogin = db.accounts.Where(a => a.username.Equals(acc.username) && a.password.Equals(f_password));
                account isLogin1 = db.accounts.FirstOrDefault(a => a.username.Equals(acc.username) && a.password.Equals(f_password));
                if (acc.username.Equals("") || acc.password.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Chưa điền username or passwor", null));
                }
                if (isLogin.Count() > 0 && isLogin1.accountid != 0)
                {

                    if ((bool)isLogin1.active == true)
                    {
                        var s = isLogin.Select(a => new { a.accountid, a.roleid, a.username, a.active, a.createdate, a.password }).ToList();
                        var q = db.employees.ToList();
                        var q1 = (from s1 in s
                                  join q2 in q on s1.accountid equals q2.accountid
                                  select new { s1.accountid, s1.roleid, s1.username, s1.active, s1.createdate, s1.password, q2.avatar }).ToList();
                        if (q1.Count() > 0)
                        {
                            CheckSale();
                            return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Đăng nhập thành công", q1));
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đăng nhập thất bại", null));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tài khoản của bạn đang bị khóa", null));

                    }




                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên đăng nhập hoặc mật khẩu không đúng", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đăng nhập thất bại", null));
            }
        }

        public void CheckSale()
        {
            try
            {
                var d = db.sales.Where(i => i.endday < DateTime.Now).ToList();
                if (d.Count > 0)
                {
                    var sd = db.saleDetails.ToList();
                    foreach (var item in d)
                    {
                        var sd1 = sd.Where(i => i.saleid == item.saleid).ToList();
                        if (sd1.Count() > 0)
                        {
                            var sd2 = sd1.Where(i => i.active == true).ToList();
                            if (sd2.Count() > 0)
                            {
                                foreach (var item1 in sd1)
                                {
                                    item1.active = false;
                                    db.Entry(item1).State = EntityState.Modified;
                                }
                                db.SaveChanges();
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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
