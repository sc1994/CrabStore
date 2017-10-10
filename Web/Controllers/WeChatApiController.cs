using Common;
using System;
using System.Xml;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Model.WeChatModel;
using System.Collections;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class WeChatApiController : Controller
    {
        public ActionResult GetOpenId(string currentPage)
        {
            var url = string.Format(WeChatConfig.WeChatCodeUrl, WeChatConfig.AppId, HttpUtility.UrlEncode($"http://{Request.Url?.Authority}/WeChatApi/RedirectUrL?currentPage={currentPage}"));
            ViewBag.CodeUrl = url;
            return View();
        }

        public ActionResult RedirectUrL(string code, string currentPage)
        {
            var data = HttpHelper.HttpGet(string.Format(WeChatConfig.OpenIdUrl, WeChatConfig.AppId, WeChatConfig.AppSecret, code));
            var openId = data.JsonToObject<OpenIdModel>().openid;
            if (openId.Length < 20)
            {
                LogHelper.Log(openId, "可能错误的openId");
            }
            currentPage = HttpUtility.UrlDecode(currentPage);
            ViewBag.RedirectUrL = $"{currentPage}{(currentPage?.IndexOf("?", StringComparison.Ordinal) > -1 ? "&" : "?")}openId={openId}";
            return View();
        }

        public string GetAccessToken()
        {
            if (WeChatConfig.AccessToken == null ||
                WeChatConfig.AccessToken.Value.IsNullOrEmpty() ||
                DateTime.Now.AddHours(-2) > WeChatConfig.AccessToken.Time)
            {
                var data = HttpHelper.HttpGet(string.Format(WeChatConfig.AccessTokenUrl, WeChatConfig.AppId, WeChatConfig.AppSecret));
                var access = data.JsonToObject<AccessTokenModel>();
                LogHelper.Log(data, "记录获取AccessToken");
                if (!access.access_token.IsNullOrEmpty())
                {
                    WeChatConfig.AccessToken = new TokenModel
                    {
                        Value = access.access_token,
                        Time = DateTime.Now
                    };
                }
                return access.access_token;
            }
            return WeChatConfig.AccessToken.Value;
        }

        public string GetJsApiTicket()
        {
            if (WeChatConfig.JsApiTicket == null ||
                DateTime.Now.AddHours(-2) > WeChatConfig.JsApiTicket.Time)
            {
                var url = string.Format(WeChatConfig.JsApiTicketUrl, GetAccessToken());
                var data = HttpHelper.HttpGet(url);
                var access = data.JsonToObject<JsApiTicketModel>();
                LogHelper.Log(data, "记录获取JsApiTicket");
                if (!access.ticket.IsNullOrEmpty())
                {
                    WeChatConfig.JsApiTicket = new TokenModel
                    {
                        Value = access.ticket,
                        Time = DateTime.Now
                    };
                }
                return access.ticket;
            }
            return WeChatConfig.JsApiTicket.Value;
        }

        /// <summary>
        /// 获取JsApi配置信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public ActionResult GetJsApiConfig(string currentPage)
        {
            var nonceStr = ConvertHelper.GetNonce(16);
            var timestamp = ConvertHelper.GetTimeStamp();
            var str = "jsapi_ticket=" + GetJsApiTicket()
                      + "&noncestr=" + nonceStr
                      + "&timestamp=" + timestamp
                      + "&url=" + currentPage;
            return Json(new
            {
                appId = WeChatConfig.AppId,
                timestamp,
                nonceStr,
                signature = str.ToSha1(),
                jsApiList = new ArrayList
                {
                    "onMenuShareTimeline",
                    "onMenuShareAppMessage",
                    "onMenuShareQQ",
                    "onMenuShareWeibo",
                    "onMenuShareQZone",
                    "startRecord",
                    "stopRecord",
                    "onVoiceRecordEnd",
                    "playVoice",
                    "pauseVoice",
                    "stopVoice",
                    "onVoicePlayEnd",
                    "uploadVoice",
                    "downloadVoice",
                    "chooseImage",
                    "previewImage",
                    "uploadImage",
                    "downloadImage",
                    "translateVoice",
                    "getNetworkType",
                    "openLocation",
                    "getLocation",
                    "hideOptionMenu",
                    "showOptionMenu",
                    "hideMenuItems",
                    "showMenuItems",
                    "hideAllNonBaseMenuItem",
                    "showAllNonBaseMenuItem",
                    "closeWindow",
                    "scanQRCode",
                    "chooseWXPay",
                    "openProductSpecificView",
                    "addCard",
                    "chooseCard",
                    "openCard"
                }
            });

        }

        /// <summary>
        /// 获取发起支付配置
        /// </summary>
        /// <param name="prepayId"></param>
        /// <returns></returns>
        public ActionResult GetBrandWcPay(string prepayId)
        {
            var timeStamp = ConvertHelper.GetTimeStamp();
            var nonceStr = ConvertHelper.GetNonce(32);
            var str = $"appId={WeChatConfig.AppId}&nonceStr={nonceStr}&package=prepay_id={prepayId}&signType=MD5&timeStamp={timeStamp}&key={WeChatConfig.PayKey}";
            return Json(new
            {
                appId = WeChatConfig.AppId,
                timeStamp = ConvertHelper.GetTimeStamp(),
                nonceStr = ConvertHelper.GetNonce(32),
                package = "prepay_id=" + prepayId,
                signType = "MD5",
                paySign = str.ToMd5().ToUpper()
            });
        }

        /// <summary>
        /// 获取预支付 Id
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="orderNumber">订单号</param>
        /// <param name="total">支付总金额</param>
        /// <param name="notify">回掉页面</param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult GetPrepayId(string body, string orderNumber, string total, string notify, string openId)
        {
            var nonce = ConvertHelper.GetNonce(32);
            string[] getPr =
            {
                "appid=" + WeChatConfig.AppId,
                "mch_id=" + WeChatConfig.MchId,
                "nonce_str=" + nonce,
                "body=" + body,
                "out_trade_no=" + orderNumber,
                "total_fee=" + total,
                "spbill_create_ip=" + HttpContext.Request.UserHostAddress,
                "notify_url=" + notify,
                "trade_type=JSAPI",
                "attach=attach", // 用户数据 将会原样返回给回掉页面, 暂时不需要
                "openid=" + openId,
            };
            var sign = (string.Join("&", getPr.OrderBy(x => x)) + "&key=" + WeChatConfig.PayKey).ToMd5().ToUpper();
            var xml = $@"
                        <xml>
                            <appid>{WeChatConfig.AppId}</appid>
                            <attach>attach</attach>
                            <body>{body}</body>
                            <mch_id>{WeChatConfig.MchId}</mch_id> 
                            <nonce_str>{nonce}</nonce_str> 
                            <notify_url>{notify}</notify_url> 
                            <openid>{openId}</openid> 
                            <out_trade_no>{orderNumber}</out_trade_no> 
                            <spbill_create_ip>{HttpContext.Request.UserHostAddress}</spbill_create_ip> 
                            <total_fee>{total}</total_fee> 
                            <trade_type>JSAPI</trade_type> 
                            <sign>{sign}</sign> 
                        </xml>";
            var data = HttpHelper.HttpPost(WeChatConfig.PrepayInfoUrl, xml);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var rootNode = xmlDoc.SelectSingleNode("xml");
            if (rootNode == null) // 可能发生请求不通的错误
            {
                LogHelper.Log(xml, "预支付请求参数");
                LogHelper.Log(data, "预支付响应参数");
                return Json(new
                {
                    code = 0,
                    data,
                    msg = "微信支付请求异常, 请查看网络请求信息"
                });
            }
            var childs = rootNode.ChildNodes;
            var prepayId = string.Empty;
            for (var i = 0; i < childs.Count; i++)
            {
                if (childs[i].Name == "prepay_id")
                {
                    prepayId = childs[i].InnerText;
                    break;
                }
            }
            if (prepayId.IsNullOrEmpty())
            {
                LogHelper.Log(xml, "预支付请求参数");
                LogHelper.Log(data, "预支付响应参数");
                return Json(new
                {
                    code = 0,
                    data,
                    msg = "请求成功但是返回并非预期值,请检查日志(预支付请求参数/预支付响应参数)"
                });
            }
            return Json(new
            {
                code = 1,
                data = prepayId
            });
        }

        [HttpPost]
        public ActionResult GetAllOpenId()
        {
            var userList = new List<string>();
            var url = string.Format(WeChatConfig.UserListUrl, GetAccessToken(), "");
            var data = HttpHelper.HttpGet(url);
            LogHelper.Log(data);
            var model = data.JsonToObject<AllUser>();
            LogHelper.Log(model.ToJson());
            userList.AddRange(model.data.openid);
            return Json(userList);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="body">请求微信api 的json 包 为string类型</param>
        /// <param name="openId">记录以及返回数据发送结果用, body 依然需要传入此参数</param>
        /// <returns></returns>
        public ActionResult SendTemplateMsg(string body, string openId)
        {
            var access = GetAccessToken();
            var url = string.Format(WeChatConfig.SendTemplateUrl, access);
            var res = HttpHelper.HttpPost(url, body);
            var data = res.JsonToObject<TemplateResponse>();
            if (data.errcode.IsNullOrEmpty())
            {
                LogHelper.Log(res, "模板消息发送失败----data:" + data.ToJson());
                return Json(new
                {
                    code = 0,
                    data = $"消息发送给{openId}时发生异常, 请求微信服务器失败",
                    msg = res
                });
            }
            if (data.errcode.ToInt() == 0)
            {
                return Json(new
                {
                    code = 1,
                    data = $"成功消息发送给{openId}"
                });
            }
            if (data.errcode.ToInt() == -1)
            {
                LogHelper.Log(res, "模板消息发送失败----data:" + data.ToJson());
                return Json(new
                {
                    code = 0,
                    data = $"消息发送给{openId}时发生异常, 系统繁忙"
                });
            }
            LogHelper.Log(res, "模板消息发送失败----data:" + data.ToJson());
            return Json(new
            {
                code = 0,
                data = $"消息发送给{openId}时发生异常, 请查看日志",
                msg = res
            });
        }

        /// <summary>
        /// 支付测试
        /// </summary>
        /// <returns></returns>
        public ActionResult PayTest()
        {
            return View();
        }
    }
}