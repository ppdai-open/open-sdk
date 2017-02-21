using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Utility
{
    public class Result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucess { get; set; }

        /// <summary>
        /// 返回码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回报文内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
