using System.ComponentModel.DataAnnotations;

namespace BlazorServerLoginLogoutDemo.Web
{
    /// <summary>
    /// 登录 Dto
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        public string LoginAccount { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        public string LoginPassword { get; set; } = string.Empty;
    }
}
