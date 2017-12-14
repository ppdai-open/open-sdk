using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySite.Models;
using Newtonsoft.Json;
using OpenApi.Utility;

namespace MySite.Controllers
{
    public class LoanController : Controller
    {
        /// <summary>
        /// 接口请求地址
        /// </summary>
        public const string Get_LoanList_URL = "https://openapi.ppdai.com/invest/BidproductlistService/LoanList";

        /// <summary>
        /// 可投标列表获取
        /// </summary>
        /// <returns></returns>
        // GET: Loan
        public ActionResult Index()
        {
            try
            {
                AuthInfo auth = Session["auth"] as AuthInfo;
                if (auth == null)
                    return Redirect("/auth");
                
                Result result = OpenApiClient.Send(Get_LoanList_URL, auth.AccessToken,
                    new PropertyObject("timestamp", DateTime.Now.ToString(), ValueTypeEnum.String));

                if (result.IsSucess)
                {

                    var model = JsonConvert.DeserializeObject<LoanListModels>(result.Context);

                    if (model.Result == 0)
                    {
                        string param = Request.QueryString["query"];

                        if (!string.IsNullOrEmpty(param))
                        {
                            param = param.Trim();

                            if (param == "AAA")
                            {
                                model.LoanList = model.LoanList.Where(p => p.CreditCode == "AAA").ToList();
                            }
                            else if (param == "rate")
                            {
                                model.LoanList = model.LoanList.Where(p => p.Rate >= 22).ToList();
                            }
                        }

                        model.LoanList = model.LoanList.OrderByDescending(p => p.Rate).ToList();
                        return View("List", model.LoanList);
                    }
                    else
                        return Content(model.ResultMessage);
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


    }
}