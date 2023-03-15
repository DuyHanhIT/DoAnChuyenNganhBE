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
    public class SizeTableAdminController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(employee))]
        [Route("api/Admin/GetAllSizeTable/{keysearch}")]
        public HttpResponseMessage GetAllSizeTable(string keysearch)
        {
            try
            {
                var d = db.sizetables.AsQueryable();
                if (keysearch.Equals("="))
                {
                    keysearch = "";
                }
                var s = d.ToList();
                var lstST = s.Where(x =>x.sho.active==true && (x.sho.shoename.ToLower().Contains(keysearch.ToLower()) || x.shoeid.ToString().ToLower().Contains(keysearch.ToLower())))
                    .Select(ss => new
                    {
                        ss.stid,
                        ss.shoeid,
                        ss.sho.shoename,
                        ss.s38,
                        ss.s39,
                        ss.s40,
                        ss.s41,
                        ss.s42,
                        ss.s43,
                        ss.s44,
                        ss.s45,
                        ss.s46,
                        ss.s47,
                        ss.s48
                    });
                if (lstST.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả bảng size thành công", lstST));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có bảng size nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpGet]
        [ResponseType(typeof(sizetable))]
        [Route("api/Admin/GetSizeTableBySizeId/{stid}")]
        public HttpResponseMessage GetSizeTableBySizeId(int stid)
        {
            try
            {
                var d = db.sizetables.AsQueryable();
               
                var s = d.ToList();
                var lstST = s.Where(x => x.stid==stid)
                    .Select(ss => new
                    {
                        ss.stid,
                        ss.shoeid,
                        ss.sho.shoename,
                        ss.s38,
                        ss.s39,
                        ss.s40,
                        ss.s41,
                        ss.s42,
                        ss.s43,
                        ss.s44,
                        ss.s45,
                        ss.s46,
                        ss.s47,
                        ss.s48
                    });
                if (lstST.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy bảng size có mã {stid} thành công", lstST));

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có bảng size nào!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Error server", null));

            }
        }
        [Authorize(Roles = "1,2")]
        [HttpPut]
        [ResponseType(typeof(sizetable))]
        [Route("api/Admin/UpdateSizeTable/{stid}")]
        public HttpResponseMessage UpdateSizeTable(int stid, sizetable st)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Failed", null));
                }

                if (stid != st.stid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không tìm thấy bảng size", null));
                }
                else
                {
                    if (st.shoeid==null || st.s38==null|| st.s39 == null || st.s40 == null|| st.s41 == null || st.s41 == null || st.s42 == null || st.s43 == null
                        || st.s44 == null || st.s45 == null || st.s46 == null || st.s47 == null || st.s48 == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Vui lòng điền đầy đủ thông tin", null));
                    }
                    if (st.shoeid==null || st.s38 <0|| st.s39 <0|| st.s40 <0|| st.s41 < 0 || st.s41 < 0 || st.s42 < 0 || st.s43 < 0
                        || st.s44 < 0 || st.s45 < 0 || st.s46 < 0 || st.s47 < 0 || st.s48 < 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, "Số lượng giày phải lớn hơn 0", null));
                    }
                    
                    var g = db.sizetables.FirstOrDefault(i => i.stid == stid);
                    g.s38 = st.s38;
                    g.s39 = st.s39;
                    g.s40 = st.s40;
                    g.s41 = st.s41;
                    g.s42 = st.s42;
                    g.s43 = st.s43;
                    g.s44 = st.s44;
                    g.s45 = st.s45;
                    g.s46 = st.s46;
                    g.s47 = st.s47;
                    g.s48 = st.s48;
                                        
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Cập nhật thành công bảng size của sản phẩm \"{g.sho.shoename}\"", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }

        }
    }
}
