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
    public class FavouriteController : ApiController
    {
        DBShoesStore db = new DBShoesStore();

        [Authorize]
        [HttpPost]
        [ResponseType(typeof(favorite))]
        [Route("api/AddOrDeleteFavourite")]
        public HttpResponseMessage AddOrDeleteFavourite(favorite f)
        {
            try
            {
                if (f.shoeid == null || f.accountid == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, "Vui lòng điền đầy đủ thông tin", null));                    
                }
                var checkFavourite = db.favorites.Where(i => i.shoeid == f.shoeid && i.accountid == f.accountid).ToList();
                if (checkFavourite.Count() > 0)
                {
                    var findF= checkFavourite.FirstOrDefault();
                    db.favorites.Remove(findF);
                    db.SaveChanges();
                    var getLShoes = getshoesAfterAddOrDeleteFavourite((int)f.accountid);
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Bỏ yêu thích sản phẩm thành công!", getLShoes));
                }
                else
                {
                    favorite fv = new favorite();
                    fv.accountid = f.accountid;
                    fv.shoeid = f.shoeid;
                    db.favorites.Add(fv);
                    db.SaveChanges();
                    var getLShoes = getshoesAfterAddOrDeleteFavourite((int)f.accountid);
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Yêu thích sản phẩm thành công!", getLShoes));
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiResult(false, 500, ex.ToString(), null));
            }
        }
        [Authorize]
        public object getshoesAfterAddOrDeleteFavourite(int accountid)
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

                
                if (query2.Count() > 0)
                {
                    return query2;

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
