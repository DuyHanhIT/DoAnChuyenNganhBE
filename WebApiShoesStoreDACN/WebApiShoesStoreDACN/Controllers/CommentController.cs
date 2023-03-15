using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Controllers
{
    public class CommentController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpPost]
        [ResponseType(typeof(comment))]
        [Route("api/AddComment")]
        public HttpResponseMessage AddComment(comment c)
        {
            try
            {
                if (c.shoeid == null || c.accountid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));
                }
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
                comment cmt = new comment();
                cmt.accountid = c.accountid;
                cmt.shoeid = c.shoeid;
                cmt.content = c.content;
                cmt.image = c.image;
                cmt.createdate = DateTime.Now;
                db.comments.Add(cmt);
                db.SaveChanges();
                var lCmt = db.comments.Where(i => i.shoeid == cmt.shoeid).ToList();
                var lAcc = db.accounts.ToList();
                var luser = db.users.ToList();
                var lstCmtByShoesid = (from ss in lCmt
                                       join a in lAcc on ss.accountid equals a.accountid
                                       join b in luser on a.accountid equals b.accountid
                                       select new
                                       {
                                           ss.cmtid,
                                           ss.accountid,
                                           ss.shoeid,
                                           ss.content,
                                           ss.image,
                                           ss.createdate,
                                           a.username,
                                           b.avatar
                                       }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Bình luận thành công!", lstCmtByShoesid));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(comment))]
        [Route("api/getCommentByShoesId/{shoesid}")]
        public HttpResponseMessage getCommentByShoesId(int shoesid)
        {
            try
            {
                var lCmt = db.comments.Where(i => i.shoeid == shoesid).ToList();
                var lAcc = db.accounts.ToList();
                var luser = db.users.ToList();
                var lstCmtByShoesid = (from ss in lCmt
                                       join a in lAcc on ss.accountid equals a.accountid
                                       join b in luser on a.accountid equals b.accountid
                                       select new
                                       {
                                           ss.cmtid,
                                           ss.accountid,
                                           ss.shoeid,
                                           ss.content,
                                           ss.image,
                                           ss.createdate,
                                           a.username,
                                           b.avatar
                                       }).ToList();
                if (lstCmtByShoesid.Count() > 0)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả bình luận thành công", lstCmtByShoesid));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có bình luận nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }

        }
    }
}
