/****************************************
// 定义了 微信 需要的基础配置 区别与web.config 
// 避免的配置混乱 增加易读性    
// 但是易变动配置依然选择配置在web.confog中,方便线上的环境变动
****************************************/

using Common;
using System;
using System.Configuration;

namespace Model.WeChatModel
{
    public class WeChatConfig
    {
        /// <summary>
        /// 开发者ID
        /// </summary>
        public static string AppId => ConfigurationManager.AppSettings["AppId"];

        /// <summary>
        /// 开发者密钥
        /// </summary>
        public static string AppSecret => ConfigurationManager.AppSettings["AppSecret"];

        /// <summary>
        /// 商户号
        /// </summary>
        public static string MchId => ConfigurationManager.AppSettings["MchId"];

        /// <summary>
        /// 支付密钥
        /// </summary>
        public static string PayKey => ConfigurationManager.AppSettings["PayKey"];

        /// <summary>
        /// 微信服务器地址
        /// </summary>
        public static string WeChatHost => ConfigurationManager.AppSettings["WeChatHost"];

        /// <summary>
        /// 微信服务器地址
        /// </summary>
        public static string WeChatHome => ConfigurationManager.AppSettings["WeChatHome"];

        /// <summary>
        /// 价格变更的模板
        /// </summary>
        public static string TemplatePrice => ConfigurationManager.AppSettings["TemplatePrice"];

        /// <summary>
        /// 获取 AccessToken  api 地址
        /// @param appid
        /// @param secret
        /// </summary>
        public const string AccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 获取微信Code api 地址 (code 是获取微信 openId 必备参数)
        /// @param appid
        /// @param redirect_uri 回掉地址
        /// </summary>
        public const string WeChatCodeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";

        /// <summary>
        /// 获取OpenId api 地址 
        /// @param appid
        /// @param secret
        /// @param code
        /// </summary>
        public const string OpenIdUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 发送模板消息 Url
        /// @param access_token
        /// </summary>
        public const string SendTemplateUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// 获取用户列表 Url
        /// @param access_token
        /// @param next_openid
        /// </summary>
        public const string UserListUrl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";

        /// <summary>
        /// 获取 JsApiTicketUrl Url
        /// @param access_token
        /// </summary>
        public const string JsApiTicketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        /// <summary>
        /// 获取预支付Id Url
        /// </summary>
        public const string PrepayInfoUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// AccessToken 的存放位置, 每次使用之前需要验证的 token 是否过期
        /// </summary>
        public static TokenModel AccessToken = null;

        /// <summary>
        /// JsApiTicket 的存放位置, 每次使用之前需要验证的 token 是否过期
        /// </summary>
        public static TokenModel JsApiTicket = null;
    }

    public class TokenModel
    {
        public string Value { get; set; } = string.Empty;
        public DateTime Time { get; set; } = "1900-1-1".ToDate();
    }

    public class AccessTokenModel
    {
        // ReSharper disable once InconsistentNaming
        public string access_token { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string expires_in { get; set; } = string.Empty;
    }

    public class OpenIdModel
    {
        // ReSharper disable once InconsistentNaming
        public string access_token { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string expires_in { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string refresh_token { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string openid { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string scope { get; set; } = string.Empty;
    }

    public class JsApiTicketModel
    {
        // ReSharper disable once InconsistentNaming
        public string errcode { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string errmsg { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string ticket { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string expires_in { get; set; } = string.Empty;
    }

    /// <summary>
    /// 预支付信息
    /// </summary>
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public class PrepayInfo
    {
        // ReSharper disable once InconsistentNaming
        public string return_code { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string return_msg { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string appid { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string mch_id { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string nonce_str { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string sign { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string result_code { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string prepay_id { get; set; } = string.Empty;
        // ReSharper disable once InconsistentNaming
        public string trade_type { get; set; } = string.Empty;
    }

    public class AllUser
    {
        // ReSharper disable once InconsistentNaming
        public int total { get; set; }
        // ReSharper disable once InconsistentNaming
        public int count { get; set; }
        // ReSharper disable once InconsistentNaming
        public AllUserOpenId data { get; set; }
        // ReSharper disable once InconsistentNaming
        public string next_openid { get; set; }
    }

    public class AllUserOpenId
    {
        // ReSharper disable once InconsistentNaming
        public string[] openid { get; set; }
    }

    public class TemplateResponse
    {
        // ReSharper disable once InconsistentNaming
        public string errcode { get; set; }
        // ReSharper disable once InconsistentNaming
        public string errmsg { get; set; }
        // ReSharper disable once InconsistentNaming
        public string msgid { get; set; }
    }
}
