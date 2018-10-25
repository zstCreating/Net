using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using WebInterface.Model;

namespace WebInterface
{
    public class JuHeService
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 加油卡充值接口
        /// </summary>
        /// <param name="money">(必填)充值金额，必须为100，200，500，1000中一个</param>

        /// <param name="game_userid">(必填)加油卡卡号，中石化：以100011开头的19位卡号、中石油：以90开头的16位卡号</param>
        /// <param name="gasCardTel">(必填)持卡人手机号码</param>
        /// <param name="gasCardName">(非必填)持卡人姓名,可为空</param>
        /// <param name="orderid">(非必填)订单ID(可为空，为空系统默认生成)</param>
        /// <returns></returns>
        public JuHeBase<string> SorderSta(int money, string game_userid, string gasCardTel, string gasCardName="",string orderid="")
        {
            log.InfoFormat("{0}调用加油卡充值接口",DateTime.Now);
            #region 处理接口字段
            StringBuilder sb=new StringBuilder();
            GasCard gasCard = new GasCard();
            if (money != 100 || money != 200 || money != 500 || money != 1000)
            {
                sb.Append("充值的金额必须是100，200，500，1000中一个\r\n");
            }
            if (string.IsNullOrEmpty(orderid))
            {
                orderid = "T" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
            if (game_userid.Length == 19 && game_userid.Substring(0, 6) != "100011")
            {
                sb.Append("输入的中石化卡号不准确，中石化开头为100011\r\n");
            }
            else if (game_userid.Length == 16 && game_userid.Substring(0, 2) != "90")
            {
                sb.Append("输入的中石油卡号不准确，中石油开头为90\r\n");
            }
            else if (game_userid.Length == 19 && game_userid.Substring(0, 6) == "100011")
            {
                gasCard.game_userid = game_userid;
                gasCard.chargeType = 1;
            }
            else if (game_userid.Length == 16 && game_userid.Substring(0, 2) == "90")
            {
                gasCard.game_userid = game_userid;
                gasCard.chargeType = 2;
            }
            else
            {
                sb.Append("输入的加油卡号不准确，中石油开头为90，且为16位，中石化开头为100011，且为19位\r\n");
            }
            if (sb.Length > 0)
            {
                log.InfoFormat("{0}字段验证不通过，原因{1}",DateTime.Now, sb.ToString());
                return new JuHeBase<string>() { reason = sb.ToString(), result = {""}, error_code=-1};
            }
            log.InfoFormat("{0}字段验证通过",DateTime.Now);

            #endregion

            //中石化
            if (gasCard.chargeType == 1)
            {
                gasCard.cardnum = "1";
                if (money == 100)
                {
                    gasCard.proid = 10001;
                }
                else if (money == 200)
                {
                    gasCard.proid = 10002;
                }
                else if (money == 500)
                {
                    gasCard.proid = 10003;
                }
                else if (money == 1000)
                {
                    gasCard.proid = 10004;
                }
            }
            //中石油
            else
            {
                gasCard.cardnum = money.ToString();
                gasCard.proid = 10008;
            }

            string key = ConfigurationManager.AppSettings["juhekey"].ToString();
            string Openid = ConfigurationManager.AppSettings["juheopenid"].ToString();
            string juheUrl = ConfigurationManager.AppSettings["juheUrl"].ToString();
            string sign = CommonHelper.MD5(Openid + key + gasCard.proid + gasCard.cardnum + gasCard.game_userid+gasCard.orderid).ToString().ToLower();
            string strtemp = "proid={0}&cardnum={1}&orderid={2}&game_userid={3}&gasCardTel={4}&gasCardName={5}&chargeType={6}&sign={7}&key={8}";
            strtemp = string.Format(strtemp, gasCard.proid, gasCard.cardnum, gasCard.orderid, gasCard.game_userid, gasCard.gasCardTel,gasCard.gasCardName,gasCard.chargeType,sign,key);
            log.InfoFormat("{0}开始调用接口，URL：{1},参数{2}",DateTime.Now,juheUrl,strtemp);
            string result = CommonHelper.HttpGet(juheUrl, strtemp);
            log.InfoFormat("{0}返回结果：{1}",DateTime.Now,result);
            JuHeBase<string> resultJson=new JuHeBase<string>();
            try {
                resultJson = JsonConvert.DeserializeObject<JuHeBase<string>>(result);
            }
            catch (Exception ex)
            {
                resultJson.result = new List<string> { ex.Message};
                resultJson.reason = "";
                resultJson.error_code = -2;
            }
            return resultJson;
        }
    }
}