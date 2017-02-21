using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Utility
{
    /// <summary>
    /// 属性格式化
    /// </summary>
    public static class PropertyFormater
    {
        /// <summary>
        /// 对象格式化
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="valueType">属性类型</param>
        /// <returns></returns>
        public static string ObjectFormat(string name, object value, ValueTypeEnum valueType)
        {
            object formatValue = null;
            switch (valueType)
            {
                case ValueTypeEnum.DateTime: formatValue = DateTimeFormat(value); break;
                case ValueTypeEnum.Single: formatValue = FloatFormat(value); break;
                case ValueTypeEnum.Double: formatValue = DoubleFormat(value); break;
                case ValueTypeEnum.Decimal: formatValue = DecimalFormat(value); break;
                case ValueTypeEnum.Boolean: formatValue = BooleanFormat(value); break;
                case ValueTypeEnum.Guid: formatValue = GuidFormat(value); break;
                case ValueTypeEnum.SByte:
                case ValueTypeEnum.Int16:
                case ValueTypeEnum.Int32:
                case ValueTypeEnum.Int64:
                case ValueTypeEnum.Byte:
                case ValueTypeEnum.UInt16:
                case ValueTypeEnum.UInt32:
                case ValueTypeEnum.UInt64:
                case ValueTypeEnum.Char:
                case ValueTypeEnum.String:
                    formatValue = value; break;
                default: break;
            }

            return formatValue == null ? string.Empty : string.Format("{0}{1}", name, formatValue);
        }

        #region 各类型格式化方法

        /// <summary>
        /// DateTime格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int DateTimeFormat(object obj)
        {
            DateTime datetime;
            if (obj is DateTime)
                datetime = (DateTime)obj;
            else
                datetime = Convert.ToDateTime(obj);

            TimeSpan span = datetime - new DateTime(1970, 1, 1);
            int timestamp = Convert.ToInt32(span.TotalSeconds);

            return timestamp;
        }

        /// <summary>
        /// Float格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FloatFormat(Object obj)
        {
            Single real;
            if (obj is Single)
                real = (Single)obj;
            else
                real = Convert.ToSingle(obj);
            
            byte[] byteArray = BitConverter.GetBytes(real);
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);

            return ByteArrayToHexString(byteArray);
        }

        /// <summary>
        /// Double格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DoubleFormat(Object obj)
        {
            Double real;
            if (obj is Double)
                real = (Double)obj;
            else
                real = Convert.ToDouble(obj);

            byte[] byteArray = BitConverter.GetBytes(real);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);

            return ByteArrayToHexString(byteArray);
        }

        /// <summary>
        /// Decima格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DecimalFormat(Object obj)
        {
            Double real = Convert.ToDouble(obj);
            return DoubleFormat(real);
        }

        /// <summary>
        /// Boolean格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int BooleanFormat(Object obj)
        {
            Boolean real;
            if (obj is Boolean)
                real = (Boolean)obj;
            else
                real = Convert.ToBoolean(obj);

            return real ? 1 : 0;
        }

        /// <summary>
        /// Guid格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GuidFormat(Object obj)
        {
            Guid real;
            if (obj is Guid)
                real = (Guid)obj;
            else
                real = new Guid(obj.ToString());

            return real.ToString("D");
        }

        private static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().ToUpper();
        }

        #endregion
    }
}
