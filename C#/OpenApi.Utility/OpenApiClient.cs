using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenApi.Utility
{
    public static class OpenApiClient
    {
        /// <summary>
        /// 授权地址
        /// </summary>
        private const string AUTHORIZE_URL = "https://ac.ppdai.com/oauth2/authorize";

        /// <summary>
        /// 刷新Token地址
        /// </summary>
        private const string REFRESHTOKEN_URL = "https://ac.ppdai.com/oauth2/refreshtoken ";

        /// <summary>
        /// 开始时间
        /// </summary>
        private static readonly DateTime beginDate = new DateTime(1970, 1, 1);

        private static string appid;
        private static RsaCryptoHelper rsaCryptoHelper;

        public static RsaCryptoHelper RsaCryptoHelper
        {
            get
            {
                return rsaCryptoHelper;
            }
        }

        public static void Init(string appid, PKCSType pkcsType, string publicKey, string privateKey)
        {
            OpenApiClient.appid = appid;
            rsaCryptoHelper = new RsaCryptoHelper(pkcsType, publicKey, privateKey);
        }

        /// <summary>
        /// 向拍拍贷网关发送请求
        /// </summary>
        /// <param name="url">调用地址</param>
        /// <param name="appid">应用ID</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="propertyObjects">请求对象属性集合</param>
        /// <returns></returns>
        public static Result Send(string url, params PropertyObject[] propertyObjects)
        {
            return Send(url, 1, null, propertyObjects);
        }

        /// <summary>
        /// 向拍拍贷网关发送请求
        /// </summary>
        /// <param name="url">调用地址</param>
        /// <param name="appid">应用ID</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="version">版本</param>
        /// <param name="propertyObjects">请求对象属性集合</param>
        /// <returns></returns>
        public static Result Send(string url, double version, params PropertyObject[] propertyObjects)
        {
            return Send(url, version, null, propertyObjects);
        }

        /// <summary>
        /// 向拍拍贷网关发送请求
        /// </summary>
        /// <param name="url">调用地址</param>
        /// <param name="appid">应用ID</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="propertyObjects">请求对象属性集合</param>
        /// <returns></returns>
        public static Result Send(string url, string accessToken, params PropertyObject[] propertyObjects)
        {
            return Send(url, 1, accessToken, propertyObjects);
        }

        /// <summary>
        /// 向拍拍贷网关发送请求
        /// </summary>
        /// <param name="url">调用地址</param>
        /// <param name="appid">应用ID</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="version">版本</param>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="propertyObjects">请求对象属性集合</param>
        /// <returns></returns>
        public static Result Send(string url, double version, string accessToken, params PropertyObject[] propertyObjects)
        {
            if (string.IsNullOrEmpty(appid)) throw new Exception("OpenApiClient未初始化");

            WebClient client = new WebClient();
            /*========================这部分为公共请求参数===========================*/
            client.Headers.Add("Content-Type", "application/json;charset=utf-8");
            client.Headers.Add("X-PPD-SIGNVERSION", "1");
            client.Headers.Add("X-PPD-SERVICEVERSION", version.ToString());
            string timestamp = Convert.ToUInt32((DateTime.UtcNow - OpenApiClient.beginDate).TotalSeconds).ToString();
            client.Headers.Add("X-PPD-TIMESTAMP", timestamp);
            client.Headers.Add("X-PPD-TIMESTAMP-SIGN", rsaCryptoHelper.SignByPrivateKey(appid + timestamp));
            client.Headers.Add("X-PPD-APPID", appid);
            client.Headers.Add("X-PPD-SIGN", rsaCryptoHelper.SignByPrivateKey(ObjectDigitalSignHelper.GetObjectHashString(propertyObjects)));
            if (!string.IsNullOrEmpty(accessToken)) client.Headers.Add("X-PPD-ACCESSTOKEN", accessToken);
            /*======================================================================*/

            try
            {
                string request = PropertysToJson(propertyObjects);
                byte[] responseData = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(request));//得到返回字符流  
                string strResponse = Encoding.UTF8.GetString(responseData);
                return new Result() { IsSucess = true, Context = strResponse };
            }
            catch (WebException ex)
            {
                WebResponse response = ex.Response;
                using (Stream errdata = response.GetResponseStream())
                {
                    if (errdata != null)
                    {
                        using (var reader = new StreamReader(errdata))
                        {
                            return new Result() { ErrorMessage = reader.ReadToEnd() };
                        }
                    }
                    return new Result() { ErrorMessage = ex.Message, Code = response.Headers["X-PPD-CODE"] };
                }
            }
            catch (Exception ex)
            {
                return new Result() { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 属性数组转换成json
        /// </summary>
        /// <param name="propertyObjects"></param>
        /// <returns></returns>
        public static string PropertysToJson(params PropertyObject[] propertyObjects)
        {
            JObject jObj = new JObject();
            foreach (var property in propertyObjects)
            {
                //特殊处理,JObject不支持Char;
                jObj.Add(new JProperty(property.Name, property.Value is Char ? property.Value.ToString() : property.Value));
            }
            return jObj.ToString();
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AuthInfo Authorize(string code)
        {
            if (string.IsNullOrEmpty(appid)) throw new Exception("OpenApiClient未初始化");
            string request = string.Format("{{\"AppID\":\"{0}\",\"code\":\"{1}\"}}", appid, code);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json;charset=utf-8");
            byte[] responseData = client.UploadData(AUTHORIZE_URL, "POST", Encoding.UTF8.GetBytes(request));//得到返回字符流  
            string strResponse = Encoding.UTF8.GetString(responseData);
            return JsonConvert.DeserializeObject<AuthInfo>(strResponse);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="openId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static AuthInfo RefreshToken(string openId, string refreshToken)
        {
            if (string.IsNullOrEmpty(appid)) throw new Exception("OpenApiClient未初始化");
            string request = string.Format("{{\"AppID\":\"{0}\",\"OpenID\":\"{1}\",\"RefreshToken\":\"{2}\"}}", appid, openId, refreshToken);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json;charset=utf-8");
            byte[] responseData = client.UploadData(REFRESHTOKEN_URL, "POST", Encoding.UTF8.GetBytes(request));//得到返回字符流  
            string strResponse = Encoding.UTF8.GetString(responseData);
            return JsonConvert.DeserializeObject<AuthInfo>(strResponse);
        }
    }
}
