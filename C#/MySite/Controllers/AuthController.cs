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
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View("Login");
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult GetToken(string code)
        {
            try
            {
                AuthInfo authInfo = OpenApiClient.Authorize(code);

                if (string.IsNullOrEmpty(authInfo.ErrMsg))
                {
                    Session.Add("auth", authInfo);

                    return new RedirectResult("/loan");
                }

                return Content(authInfo.ErrMsg);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult Print() {
            AuthInfo authInfo = Session["auth"] as AuthInfo;

            return Content(JsonConvert.SerializeObject(authInfo));
        }
    }
}