using System.Security.Claims;

namespace BlazorServerLoginLogoutDemo.Web.Models
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 是否身份验证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        public string ID { get; set; }  = Guid.Empty.ToString("N");

        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<Claim> Roles { get; set; } = new List<Claim>();
    }
}
