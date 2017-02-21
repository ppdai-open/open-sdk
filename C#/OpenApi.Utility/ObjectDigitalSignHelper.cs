using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenApi.Utility
{
    public class ObjectDigitalSignHelper
    {
        /// <summary>
        /// 获取签名字符串排序
        /// </summary>
        /// <param name="propertyObjects">属性对象</param>
        /// <returns></returns>
        public static string GetObjectHashString(params PropertyObject[] propertyObjects)
        {
            StringBuilder sb = new StringBuilder();
            var sortPropertys = propertyObjects.Where(p => p.IsSign).OrderBy(p => p.LowerName).GetEnumerator();
            while (sortPropertys.MoveNext()) {
                sb.Append(sortPropertys.Current.ToString());
            }
            return sb.ToString();
        }
    }
}
