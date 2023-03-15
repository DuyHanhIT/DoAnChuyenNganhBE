using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models; 
namespace WebApiShoesStoreDACN.Controllers
{
    public class AccountController : ApiController
    {
        DBShoesStore db = new DBShoesStore();


        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        //[Authorize]
        [HttpPost]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Login")]
        public HttpResponseMessage PostLogin(account acc)
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
                        var s = isLogin.Select(a => new { a.accountid, a.roleid, a.username, a.active, a.createdate,a.password });
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Đăng nhập thành công", s));
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

        [HttpPost]
        [ResponseType(typeof(account))]
        // GET: api/ACCOUNTs
        [Route("api/Register")]
        public HttpResponseMessage PostRegister(RegiterModel register)
        {
            try
            {
                String username = register.username;
                String pass = register.password;//123123123
                String repass = register.Repassword;

                // var f_password = GetMD5(acc.password);
                var isLogin = db.accounts.Where(a => a.username.Equals(username));
                if (username.Length < 3)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên đăng nhập lớn hơn 3 ký tự", null));
                }
                if (isLogin.Count() > 0)
                {
                    /*var s = isLogin.Select(a => new { a.accountid, a.roleid, a.username, a.active, a.createdate });*/
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tên người dùng đã tồn tại", null));
                } else if (pass.Equals("d41d8cd98f00b204e9800998ecf8427e"))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Mật khẩu không được để trống", null));
                }
                else if (!pass.Equals(repass))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Mật khẩu không trùng khớp", null));
                }
                else
                {
                    try
                    {
                        /*var f_pass = GetMD5(pass);*/
                        account acc = new account();
                        acc.username = username;
                        acc.password = pass;
                        acc.roleid = 3;
                        acc.createdate = DateTime.Now;
                        acc.active = true;
                        db.accounts.Add(acc);
                        db.SaveChanges();
                        var lstacc = new List<account>();
                        lstacc.Add(acc);
                        var lstacc1 = lstacc.Select(x => new { x.accountid });
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
        [Authorize]
        [HttpPut]
        [ResponseType(typeof(account))]
        [Route("api/ChangePassword")]
        public HttpResponseMessage ChangePassword(RegiterModel acc)
        {
            try
            {
                if (acc.Oldpassword == null || acc.Oldpassword.Equals("d41d8cd98f00b204e9800998ecf8427e") || acc.password == null || acc.password.Equals("d41d8cd98f00b204e9800998ecf8427e") || acc.Repassword == null || acc.Repassword.Equals("d41d8cd98f00b204e9800998ecf8427e"))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin!", null));
                }
                if (!acc.password.Equals(acc.Repassword))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Mật khẩu không trùng khớp", null));
                }
                var g = db.accounts.FirstOrDefault(i => i.accountid == acc.accountid);
                if (!g.password.Equals(acc.Oldpassword))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Bạn đã nhập sai mật khẩu hiện tại!", null));
                }
                g.password = acc.password;
                db.Entry(g).State = EntityState.Modified;
                var lstAcc = new List<account>();
                lstAcc.Add(g);
                var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });


                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Đặt lại mật khẩu cho tài khoản {g.username} thành công", lstUser1));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
            }
        }
        [HttpPut]
        [ResponseType(typeof(account))]
        [Route("api/FogotPassword")]
        public HttpResponseMessage FogotPassword(RegiterModel acc)
        {
            try
            {
                if (acc.username == null || acc.username.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "chưa truyền username", null));
                }
                var GLAcc = db.accounts.Where(i => i.username.Equals(acc.username)).ToList();
                if(GLAcc.Count()< 1)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Không tồn tại username trong hệ thống", null));
                }
                if(acc.password==null||acc.password.Equals("d41d8cd98f00b204e9800998ecf8427e") ||acc.Repassword == null || acc.Repassword.Equals("d41d8cd98f00b204e9800998ecf8427e"))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin!", null));
                }
                if (!acc.password.Equals(acc.Repassword))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Mật khẩu không trùng khớp", null));
                }

                var g = GLAcc.FirstOrDefault();
                g.password = acc.password;
                db.Entry(g).State = EntityState.Modified;
                var lstAcc = new List<account>();
                lstAcc.Add(g);
                var lstUser1 = lstAcc.Select(x => new { x.accountid, x.username, x.roleid, x.createdate, x.active });


                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Đặt lại mật khẩu cho tài khoản {g.username} thành công", lstUser1));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));
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
