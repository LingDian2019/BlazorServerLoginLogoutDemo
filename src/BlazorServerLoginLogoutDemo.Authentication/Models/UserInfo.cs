namespace BlazorServerLoginLogoutDemo.Authentication
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string LoginAccount { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPassword { get; set; } = string.Empty;
    }
}
