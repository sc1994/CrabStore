/****************************************
// 定义了 微信 需要的基础配置 区别与web.config 
// 避免的配置混乱 增加易读性    
****************************************/

namespace Model.WeChatModel
{
    public class WeChatConfig
    {
        /// <summary>
        /// 开发者ID
        /// </summary>
        public const string AppId = "wx9e7797521695b5d7";

        /// <summary>
        /// 开发者密钥
        /// </summary>
        public const string AppSecret = "52f27e040b80b82eb677f30e66555a05";

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
        /// AccessToken 的存放位置, 每次使用之前需要验证的 token 是否过期
        /// </summary>
        public static Token AccessToken = new Token();
    }
}
