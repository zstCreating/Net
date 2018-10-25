using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebInterface.Model
{
    public class JuHeBase<T>
    {
        /// <summary>
        /// 返回状态 sucess 表示成功
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        /// 返回接口内容
        /// </summary>
        public List<T> result { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public int error_code { get; set; }
    }
}