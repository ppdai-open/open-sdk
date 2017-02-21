using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Utility
{
    /// <summary>
    /// 键值对象
    /// </summary>
    public class PropertyObject
    {
        private string name;
        private string lowerName;

        private object value;
        private ValueTypeEnum valueType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="valueType">属性类型</param>
        public PropertyObject(string name, object value, ValueTypeEnum valueType)
        {
            this.name = name;
            this.lowerName = name.ToLower();
            this.value = value;
            this.valueType = valueType;
        }

        /// <summary>
        /// 是否参与签名验证
        /// </summary>
        public bool IsSign {
            get { return value != null && valueType != ValueTypeEnum.Other; }
        }

        /// <summary>
        /// 获取小写名称
        /// </summary>
        public string LowerName {
            get { return lowerName; }
        }

        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public object Value {
            get { return value; }
        }

        /// <summary>
        /// 生成格式化字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return PropertyFormater.ObjectFormat(lowerName, value, valueType);
        }
    }
}
