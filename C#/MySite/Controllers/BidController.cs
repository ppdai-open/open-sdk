using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MySite.Models;
using Newtonsoft.Json;
using OpenApi.Utility;

namespace MySite.Controllers
{
    public class BidController : Controller
    {

        /// <summary>
        /// 接口请求地址
        /// </summary>
        private const string Bid_URL = "https://openapi.ppdai.com/invest/BidService/Bidding";

        // GET: Bid
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 投标动作
        /// </summary>
        /// <returns></returns>
        public ActionResult DoBid()
        {
            try
            {
                AuthInfo auth = Session["auth"] as AuthInfo;
                if (auth == null)
                    return Redirect("/auth");

                int id = int.Parse(Request.QueryString["id"]);
                int amount = int.Parse(Request.QueryString["amount"]);

                string request = string.Format("{{ \"ListingId\": {0},  \"Amount\": {1} }}", id, amount);

                Result result = OpenApiClient.Send(Bid_URL, auth.AccessToken,
                    new PropertyObject("ListingId", id, ValueTypeEnum.Int32), new PropertyObject("Amount", amount, ValueTypeEnum.Int32));

                if (result.IsSucess)
                {
                    return Content(result.Context);
                }
                else
                {
                    return Content(result.ErrorMessage);
                }
            }
            catch (FormatException ex)
            {
                return Content("密钥无效，请更换有效密钥");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult DoBid2(BidModel model) {

            AuthInfo auth = Session["auth"] as AuthInfo;
            if (auth == null)
                return Redirect("/auth");
            
            Result result = OpenApiClient.Send(LoanController.Get_LoanList_URL, auth.AccessToken,
                new PropertyObject("timestamp", DateTime.Now, ValueTypeEnum.DateTime));

            if (result.IsSucess)
            {
                var loanList = JsonConvert.DeserializeObject<LoanListModels>(result.Context);
                bool isCertificateValidate = model.Tags.Any(p => p == "CertificateValidate");
                bool isMobileRealnameValidate = model.Tags.Any(p => p == "MobileRealnameValidate");
                bool isRate22 = model.Tags.Any(p => p == "rate22");
                bool isCreditCodeAAA = model.Tags.Any(p => p == "CreditCodeAAA");
                bool isCreditCodeD = model.Tags.Any(p => p == "CreditCodeD");

                StringBuilder sb = new StringBuilder();

                foreach (var loan in loanList.LoanList.OrderByDescending(p => p.Rate))
                {
                    if (isCertificateValidate && loan.CertificateValidate != 1) continue;

                    if (isMobileRealnameValidate && loan.MobileRealnameValidate != 1) continue;

                    if (isRate22 && loan.Rate < 22) continue;

                    if (isCreditCodeAAA && loan.CreditCode != "AAA") continue;

                    if (isCreditCodeD && (loan.CreditCode == "E" || loan.CreditCode == "F")) continue;

                    try
                    {
                        Result bidResp = OpenApiClient.Send(Bid_URL, auth.AccessToken,
                            new PropertyObject("ListingId", loan.ListingId, ValueTypeEnum.Int32), new PropertyObject("Amount", 50, ValueTypeEnum.Int32));

                        if (bidResp.IsSucess)
                        {
                            BidResult bidResult = JsonConvert.DeserializeObject<BidResult>(bidResp.Context);
                            sb.AppendLine(string.Format("投标：{0}  ，结果： {1}", loan.ListingId, bidResult.ResultMessage));
                        }
                        else
                        {
                            sb.AppendLine(string.Format("投标：{0}  ，结果： {1}", loan.ListingId, bidResp.ErrorMessage));
                        }

                        model.Amount -= 50;
                        if (model.Amount < 50) break;

                    }
                    catch (Exception ex) {
                        sb.AppendLine(string.Format("投标：{0}  ，结果： {1}", loan.ListingId, ex.Message));
                    }
                }

                if (sb.Length == 0) return Content("没有满足条件的标的");

                return Content(sb.ToString());

            }

                return Content("获取投资列表失败");
        }
    }
}