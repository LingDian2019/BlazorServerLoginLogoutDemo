namespace BlazorServerLoginLogoutDemo.Web.Pages.Authentication
{
    /// <summary>
    /// 身份验证服务 Key
    /// </summary>
    public class AuthenticationServiceKey
    {
        /// <summary>
        /// Token
        /// </summary>
        public const string Token = "Token";

        /// <summary>
        /// 用户名称
        /// </summary>
        public const string UserName = "UserName";

        /// <summary>
        /// Redis用户授权信息
        /// </summary>
        public const string RedisUserInfo = "Authentication:UserInfo:{0}";
    }
}
