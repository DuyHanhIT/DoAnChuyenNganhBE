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
    public class SearchHistoryController : ApiController
    {
        DBShoesStore db = new DBShoesStore();
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(searchhistory))]
        // GET: api/ACCOUNTs
        [Route("api/AddSearchHistory")]
        public HttpResponseMessage AddSearchHistory(searchhistory s)
        {
            try
            {

                var getkey = db.searchhistories.Where(i => i.accountid == s.accountid).FirstOrDefault();
                if (getkey != null)
                {
                    getkey.keyword = s.keyword;
                    db.Entry(getkey).State = EntityState.Modified;
                    db.SaveChanges();
                    var query2 = getshoesAfterAddSearchHistory(getkey.accountid, getkey.keyword);
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Cập nhật keysearch thành công!!", query2));
                }
                else
                {
                    searchhistory r = new searchhistory();
                    r.accountid = s.accountid;
                    r.keyword = s.keyword;
                    db.searchhistories.Add(r);
                    db.SaveChanges();
                    var query2 = getshoesAfterAddSearchHistory(r.accountid, r.keyword);
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Thêm keysearch thành công!!", query2));

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(searchhistory))]
        // GET: api/ACCOUNTs
        [Route("api/GetShoesByKeyOfSearchHistory/{accountid}")]
        public HttpResponseMessage GetShoesByKeyOfSearchHistory( int accountid)
        {
            try
            {

                var getkey = db.searchhistories.Where(i => i.accountid == accountid).FirstOrDefault();
                if (getkey != null)
                {
                    var query2 = getshoesAfterAddSearchHistory(getkey.accountid, getkey.keyword);
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy shoes by keysearch thành công!!", query2));
                }
                else
                {                    
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(true, 404, "Không có keysearch nào!!", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }
        [Authorize]
        public object getshoesAfterAddSearchHistory(int accountid, string keysearch)
        {
            try
            {
                var s = db.shoes.AsQueryable();
                var sss = s.ToList();
                var shoesActive = sss.Where(x => x.active == true && x.shoename.ToLower().Contains(keysearch.ToLower())).ToList();
                var inSaleDetails = db.saleDetails.Where(i => i.active == true).ToList();
                var inSale = db.sales.Where(i => i.endday >= DateTime.Now).ToList();
                var inFavorite = db.favorites.Where(x => x.accountid == accountid).ToList();
                var query = from ss in inSaleDetails
                            join sd in inSale on ss.saleid equals sd.saleid into sd
                            from subsaledetail in sd.DefaultIfEmpty()
                            select new
                            {
                                ss.saleid,
                                ss.shoeid,
                                ss.saleprice,
                                ss.active,
                                ss.updateby,
                                percent = subsaledetail?.percent ?? null,
                                salename = subsaledetail?.salename ?? null,
                                startday = subsaledetail?.startday ?? null,
                                endday = subsaledetail?.endday ?? null,
                            };


                var query1 = from ss in shoesActive
                             join sd in query on ss.shoeid equals sd.shoeid into sd
                             from subsaledetail in sd.DefaultIfEmpty()

                             select new
                             {
                                 ss.shoeid,
                                 ss.shoename,
                                 ss.price,
                                 ss.description,
                                 ss.stock,
                                 ss.purchased,
                                 ss.shoenew,
                                 ss.createdate,
                                 ss.createby,
                                 ss.updateby,
                                 ss.dateupdate,
                                 ss.active,
                                 ss.brandid,
                                 ss.categoryid,
                                 ss.image1,
                                 ss.image2,
                                 ss.image3,
                                 ss.image4,
                                 ss.rate,
                                 saleprice = subsaledetail?.saleprice ?? null,
                                 percent = subsaledetail?.percent ?? null,
                                 salename = subsaledetail?.salename ?? null,
                                 startday = subsaledetail?.startday ?? null,
                                 endday = subsaledetail?.endday ?? null
                             };
                var query2 = from ss in query1
                             join sd in inFavorite on ss.shoeid equals sd.shoeid into sf
                             from subFavorite in sf.DefaultIfEmpty()

                             select new
                             {
                                 ss.shoeid,
                                 ss.shoename,
                                 ss.price,
                                 ss.description,
                                 ss.stock,
                                 ss.purchased,
                                 ss.shoenew,
                                 ss.createdate,
                                 ss.createby,
                                 ss.updateby,
                                 ss.dateupdate,
                                 ss.active,
                                 ss.brandid,
                                 ss.categoryid,
                                 ss.image1,
                                 ss.image2,
                                 ss.image3,
                                 ss.image4,
                                 ss.saleprice,
                                 ss.percent,
                                 ss.salename,
                                 ss.startday,
                                 ss.endday,
                                 ss.rate,
                                 isfavorite = subFavorite != null ? true : false

                             };


                if (query2.Count() > 0)
                {
                    return query2.ToList();

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
