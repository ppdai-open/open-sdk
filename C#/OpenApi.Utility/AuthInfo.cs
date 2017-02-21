using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Utility
{
    public class AuthInfo
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ErrMsg { get; set; }
        
        public string OpenID { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用于请求刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// access_token的有效期，单位是秒
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
