using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.Models;

namespace WebApiShoesStoreDACN.Controllers
{
    public class ShoesController : ApiController
    {
        private DBShoesStore db = new DBShoesStore();

        /*[Authorize(Roles = "1")]*/
        //[Authorize]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        [Route("api/getAllShoesActive/{accountid}")]
        public HttpResponseMessage GetAllShoes(int accountid)
        {

            try
            {
                var shoesActive = db.shoes.Where(x => x.active == true).ToList();
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
                
                /*var lstShoes1 = shoesActive.Select(ss => new
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
                });*/
                if (query2.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả sản phẩm thành công", query2));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        [Route("api/getShoesByBrandId/{brandid}/{accountid}")]
        public HttpResponseMessage getShoesByBrandId(int brandid, int accountid)
        {
            try
            {
                var shoesActive = db.shoes.Where(x => x.brandid == brandid && x.active == true).ToList();
                if (shoesActive.Count > 0)
                {
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
                                     ss.rate,
                                     ss.saleprice,
                                     ss.percent,
                                     ss.salename,
                                     ss.startday,
                                     ss.endday,
                                     isfavorite = subFavorite != null ? true : false
                                 };
                    /*var lstShoes1 = shoesActive.Select(ss => new
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
                    });*/
                    if (query2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả sản phẩm của hãng thành công", query2));

                    }
                    else
                    {

                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm nào", null));
                    }
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm nào", null));
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        [Route("api/getShoeById/{shoesid}/{accountid}")]
        public HttpResponseMessage getShoeById(int shoesid, int accountid)
        {

            try
            {
                var s = db.shoes.AsQueryable();
                //if(!string.IsNullOrEmpty(shoesid))
                var shoesActive = s.Where(a => a.shoeid == shoesid && a.active == true).ToList();
                if (shoesActive.Count() > 0)
                {
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
                                     ss.rate,
                                     ss.saleprice,
                                     ss.percent,
                                     ss.salename,
                                     ss.startday,
                                     ss.endday,
                                     isfavorite = subFavorite != null ? true : false
                                 };
                    /*var lstShoesById = shoesActive.Select(ss => new
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
                    });*/
                    if (query2.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, $"Lấy sản phẩm có mã = {shoesid} thành công", query2));
                    }// $"Get shoes by id = {shoesid} success"
                    else
                    {

                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, $"Không có sản phẩm có mã = {shoesid}", null));
                    }
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, $"Không có sản phẩm có mã = {shoesid}", null));
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(shoes))]
        [Route("api/getAllShoesBySaleId/{saleid}/{accountid}")]
        public HttpResponseMessage getAllShoesBySaleId(int saleid, int accountid)
        {

            try
            {
                var shoesActive = db.shoes.Where(x => x.active == true).ToList();
                var inSaleDetails = db.saleDetails.Where(i => i.saleid == saleid && i.active == true).ToList();
                var inSale = db.sales.Where(i => i.saleid == saleid && i.endday >= DateTime.Now && i.startday <= DateTime.Now).ToList();
                var inFavorite = db.favorites.Where(x => x.accountid == accountid).ToList();

                var query1 = (from sd in inSale
                              join ss in inSaleDetails on sd.saleid equals ss.saleid
                              select new { sd.saleid, sd.salename, sd.startday, sd.endday, sd.percent, sd.imgsale, ss.shoeid, ss.saleprice }).ToList();
                var query2 = (from sd in query1
                              join ss in shoesActive on sd.shoeid equals ss.shoeid
                              select new
                              {
                                  sd.saleid,
                                  sd.salename,
                                  sd.startday,
                                  sd.endday,
                                  sd.percent,
                                  sd.imgsale,
                                  sd.shoeid,
                                  sd.saleprice,
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
                                  ss.rate
                              }).ToList();
                var query3 = (from ss in query2
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
                                  ss.rate,
                                  ss.saleid,
                                  ss.salename,
                                  ss.startday,
                                  ss.endday,
                                  ss.percent,
                                  ss.imgsale,
                                  ss.saleprice,
                                  isfavorite = subFavorite != null ? true : false
                              }).ToList();

                if (query3.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Lấy tất cả sản phẩm khuyến mãi theo chương trình thành công", query3));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(false, 404, "Không có sản phẩm nào", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Đã xảy ra lỗi trong quá trình xử lý", null));

            }
        }

    }
}
