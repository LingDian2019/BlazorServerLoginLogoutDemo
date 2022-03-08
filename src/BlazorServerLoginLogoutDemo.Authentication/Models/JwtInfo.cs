namespace BlazorServerLoginLogoutDemo.Authentication
{
    /// <summary>
    /// JWT 信息
    /// </summary>
    public class JwtInfo
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token 类型
        /// </summary>
        public string TokenType { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间 时间戳
        /// </summary>
        public long ExpirationTimeUnixTimeSeconds { get; set; }
    }
}
