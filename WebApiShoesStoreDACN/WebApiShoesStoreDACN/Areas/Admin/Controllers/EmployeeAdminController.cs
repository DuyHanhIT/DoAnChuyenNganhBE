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
    public class EmployeeAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(employee))]
        [Route("api/Admin/GetAllEmployee/{keysearch}")]
        public HttpResponseMessage GetAllEmployee(string keysearch)
        {
            try
            {
                var d = db.employees.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();
                var lstEmployee = s.Where(x => x.firstName.ToLower().Contains(keysearch.ToLower()) || x.epid.ToString().ToLower().Contains(keysearch.ToLower()))
                    .Select(ss => new
                    {
                        ss.epid,
                        ss.accountid,
                        ss.firstName,
                        ss.lastName,
                        ss.birthday,
                        ss.gender,
                        ss.phone,
                        ss.address,
                        ss.avatar,
                        ss.account.username
                    });
                if (lstEmployee.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả nhân viên thành công", lstEmployee));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có nhân viên nào nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1")]
        [HttpGet]
        [ResponseType(typeof(employee))]
        [Route("api/Admin/GetEmployeeByEmployeeId/{epid}")]
        public HttpResponseMessage GetEmployeeByEmployeeId(int epid)
        {
            try
            {
                var d = db.employees.AsQueryable();

                var s = d.Where(x => x.epid == epid).ToList();
                var lstEmp = s.Select(ss => new
                {
                    ss.epid,
                    ss.accountid,
                    ss.firstName,
                    ss.lastName,
                    ss.birthday,
                    ss.gender,
                    ss.phone,
                    ss.address,
                    ss.avatar,
                    ss.account.username
                });

                if (lstEmp.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy nhân viên by epid thành công", lstEmp));
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Không có nhân viên nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        public int daysBetween(DateTime StartDate, DateTime EndDate)
            {
                DateTime d = EndDate;
                DateTime g = StartDate;
            /*return (EndDate - StartDate).TotalDays;*/
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = EndDate-StartDate;
                int years = (zeroTime + span).Year - 1;
            return years;
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ResponseType(typeof(employee))]
        [Route("api/Admin/AddEmployee")]
        public HttpResponseMessage AddEmployee(employee ep)
        {
            try
            {
                if (ep.accountid == null || ep.firstName == null || ep.firstName.Equals("") || ep.lastName.Equals("")
                    || ep.lastName == null || ep.birthday == null || ep.birthday.Equals("") || ep.gender == null
                    || ep.phone == null || ep.phone.Equals("") || ep.address == null || ep.address.Equals(""))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng nhập đầy đủ thông tin", null));
                }
                DateTime NS = (DateTime)ep.birthday;
                var checkexistsac = db.employees.Where(o => o.accountid == ep.accountid).ToList();
                if (checkexistsac.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Tài khoản này đã được đăng ký", null));
                }
                if (ep.avatar == null || ep.avatar.Equals(""))
                {
                    ep.avatar = "https://sv3.anh365.com/images/2022/10/25/image7d52779535c30442.png";
                }
                if (ep.birthday > DateTime.Now)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Nhân viên của shop không đến từ tương lai :)))", null));
                }
                if (daysBetween((DateTime)ep.birthday, DateTime.Now) < 18)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Nhân viên chưa đủ 18 tuổi", null));

                }
                else
                {
                    try
                    {
                        employee e = new employee();
                        e.accountid = ep.accountid;
                        e.firstName = ep.firstName;
                        e.lastName = ep.lastName;
                        e.birthday = ep.birthday;
                        e.gender = ep.gender;
                        e.phone = ep.phone;
                        e.address = ep.address;
                        e.avatar = ep.avatar;
                        db.employees.Add(e);
                        db.SaveChanges();
                        var empid = e.epid;
                        
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Thêm thành công nhân viên có mã {empid}", null));

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
        [Authorize(Roles = "1")]
        [HttpPut]
        [ResponseType(typeof(employee))]
        [Route("api/Admin/UpdateEmployee/{epid}")]
        public HttpResponseMessage UpdateEmployee(int epid, employee ep)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (epid != ep.epid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy nhân viên", null));
                }
                else
                {
                    if (ep.accountid == null || ep.firstName.Equals("") || ep.firstName == null || ep.lastName.Equals("") || ep.lastName == null || ep.birthday.Equals("") || ep.birthday == null
                        || ep.gender.Equals("") || ep.gender == null || ep.phone.Equals("") || ep.phone == null || ep.address.Equals("") || ep.address == null )
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    if (ep.avatar == null || ep.avatar.Equals(""))
                    {
                        ep.avatar = "https://sv3.anh365.com/images/2022/10/25/image7d52779535c30442.png";
                    }
                    if(ep.birthday> DateTime.Now)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Nhân viên của shop không đến từ tương lai :)))", null));
                    }
                    if (daysBetween((DateTime)ep.birthday, DateTime.Now) < 18)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Nhân viên chưa đủ 18 tuổi", null));

                    }
                    var u = db.employees.Where(i => i.accountid== ep.accountid).ToList();
                    var o = db.employees.Where(i => i.epid == ep.epid).ToList();
                    if (u.Count() > 0)
                    {
                        var n = u[0];
                        var m = o[0];
                        if (n.epid != m.epid)
                        {
                            u.Add(ep);
                        }
                        if (u.Count() > 1)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Tài khoản này đã được sử dụng", null));
                        }
                    }
                    var g = db.employees.FirstOrDefault(i => i.epid == epid);
                    //g.accountid = ep.accountid;
                    g.firstName = ep.firstName;
                    g.lastName = ep.lastName;
                    g.birthday = ep.birthday;
                    g.gender = ep.gender;
                    g.phone = ep.phone;
                    g.address = ep.address;
                    g.avatar = ep.avatar;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công nhân viên có mã \"{ep.epid}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
            
        }
    }
}
