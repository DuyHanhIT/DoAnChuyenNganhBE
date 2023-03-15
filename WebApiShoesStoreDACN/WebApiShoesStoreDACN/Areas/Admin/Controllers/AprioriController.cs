using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiShoesStoreDACN.AprioriTest.src;
using WebApiShoesStoreDACN.Models;
namespace WebApiShoesStoreDACN.Areas.Admin.Controllers
{
    public class AprioriController : ApiController
    {

        DBShoesStore db = new DBShoesStore();
        [HttpGet]
        [Route("api/Apriori/{shoeid}/{accountid}")]
        public HttpResponseMessage GetAllAccounts(int shoeid, int accountid)
        {
            try
            {
                var query1 = db.orderdetails.OrderByDescending(i => i.orderid).ToList();
                var query2 = query1.Select(i => new { i.orderid });
                var query3 = query2.Distinct();
                var transactions = new List<List<int>>();
                foreach (var x in query3)
                {
                    var query4 = query1.Where(i => i.orderid == x.orderid).Select(i => (int)i.shoeid).ToList();
                    var query5 = query4.Distinct().ToList();
                    transactions.Add(query4);
                }

                var maxCol = transactions.Max(x => x.Max());

                var dataFields = new DataFields(maxCol, transactions);

                var myApriori = new Apriori(dataFields);
                var minimumSupport = 0.2f; // %20 Minimum Support

                myApriori.CalculateCNodes(minimumSupport);
                var s = new List<string>();
                

                int count = myApriori.Rules.Count(x => x.Confidence >= .5f);

                if (count < 10)
                {
                    foreach (var associationRule in myApriori.Rules.OrderByDescending(x => x.Confidence).Where(x => x.Confidence >= .5f))
                    {
                        s.Add(associationRule.ToDetailedString1(dataFields));
                    }
                }
                else
                {
                    foreach (var associationRule in myApriori.Rules.OrderByDescending(x => x.Confidence).Where(x => x.Confidence >= .5f).Take(10))
                    {
                        s.Add(associationRule.ToDetailedString1(dataFields));
                    }
                }

                var s4 = new List<List<int>>();
                var s5 = new List<int>();
                if (s.Count() > 0)
                {
                    foreach (var ii in s)
                    {
                        string[] s2 = ii.Split(',');
                        var s3 = s2.Select(int.Parse).ToList();
                        if (s3.Contains(shoeid))
                        {
                            s5.AddRange(s3);
                        }
                    }
                    var s6 = s5.Where(val => val != shoeid).Distinct().ToList();
                    if (s6.Count() > 0)
                    {
                        var lstShoes = new List<object>();
                        foreach (var item in s6)
                        {
                            var sh = getShoeById(item, accountid);
                            if (sh != null)
                            {
                                lstShoes.Add(sh);
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResult(true, 200, "Top rules ordered by Confidence (Up to 10)", lstShoes));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(true, 404, "Không có sp nào hay được mua cùng", null));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResult(true, 404, "Không có sp nào hay được mua cùng", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResult(false, 400, ex.ToString(), null));
            }

        }
        [ResponseType(typeof(shoes))]
        public object getShoeById(int shoeid, int accountid)
        {

            try
            {
                var s = db.shoes.AsQueryable();
                //if(!string.IsNullOrEmpty(shoesid))
                var shoesActive = s.Where(a => a.shoeid == shoeid).ToList();
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
                    var lstShoesById = query2.FirstOrDefault();
                    if (query2.Count() > 0)
                    {
                        return lstShoesById;
                    }// $"Get shoes by id = {shoesid} success"
                    else
                    {
                        return null;
                    }
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
