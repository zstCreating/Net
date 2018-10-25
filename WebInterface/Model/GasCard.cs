using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebInterface.Model
{
    /// <summary>
    /// 加油卡输入实体类
    /// </summary>
    public class GasCard
    {
        /// <summary>
        /// 充值产品代码编号
        /// 10001(中石化100元加油卡)
        /// 10002(中石化200元加油卡)
        /// 10003(中石化500元加油卡)
        /// 10004(中石化1000元加油卡)
        /// 10008(中石油任意金额充值)
        /// </summary>
        public int proid { get; set; }
        /// <summary>
        /// 充值金额
        /// 1，当proid为10008时【中石油】，只支持100\200\500\1000元充值
        /// 2，当proid为【中石化时】，此处填写1
        /// </summary>
        public string cardnum { get; set; }
        /// <summary>
        /// 商家订单号，8-32位字母数字组合
        /// </summary>
        public string orderid { get; set; }
        /// <summary>
        /// 加油卡卡号，中石化：以100011开头的19位卡号、中石油：以90开头的16位卡号
        /// </summary>
        public string game_userid { get; set; }
        /// <summary>
        /// 持卡人手机号码,可以填写一个固定格式的手机号码，如:18900000000
        /// </summary>
        public string gasCardTel { get; set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string gasCardName { get; set; }
        /// <summary>
        /// 	加油卡类型 （1:中石化、2:中石油；默认为1)
        /// </summary>
        public int chargeType { get; set; }
    }
}