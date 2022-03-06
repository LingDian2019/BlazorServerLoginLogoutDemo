using Microsoft.AspNetCore.Components.Authorization;
using BlazorServerLoginLogoutDemo.Web.Models;
using System.Security.Claims;

namespace BlazorServerLoginLogoutDemo.Web.Pages.Authentication
{
    /// <summary>
    /// 系统身份验证状态
    /// </summary>
    public class SystemAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localStorage"></param>
        public SystemAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        /// <summary>
        /// 获取授权状态
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            var currentUser = await GetCurrentUserInfo();
            if (currentUser.IsAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, currentUser.UserName)
                };

                foreach (var claim in currentUser.Roles)
                {
                    claims.Add(claim);
                }

                identity = new ClaimsIdentity(claims, AuthenticationServiceKey.Token);
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        public async Task LoginAsync(string token, string userName)
        {
            await _localStorageService.SetItemAsync(AuthenticationServiceKey.Token, token);
            await _localStorageService.SetItemAsync(AuthenticationServiceKey.UserName, userName);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// 注销
        /// </summary>
        public async Task LogoutAsync()
        {
            await _localStorageService.ClearAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUser> GetCurrentUserInfo()
        {
            var user = new CurrentUser() { IsAuthenticated = false };
            string? userName;
            string? token;

            try
            {
                //浏览器还未加载完js时，不能使用LocalStorage
                token = await _localStorageService.GetItemAsync<string>(AuthenticationServiceKey.Token);
                userName = await _localStorageService.GetItemAsync<string>(AuthenticationServiceKey.UserName);
            }
            catch (Exception)
            {
                return user;
            }

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userName))
            {
                var claims = GetParseClaimsFromJwt(token);
                if (claims != null)
                {
                    var id = claims.GetValueOrDefault("ID");
                    if (id != null)
                    {
                        user.ID = id;
                        user.IsAuthenticated = true;
                    }
                    user.Roles = GetRoleParseClaimsFromJwt(token);
                    user.UserName = userName;
                }
            }

            return user;
        }

        /// <summary>
        /// 解析来自Jwt的声明
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static IEnumerable<Claim> GetRoleParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var keyValuePairs = GetParseClaimsFromJwt(jwt);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out string? roles);

                if (!string.IsNullOrWhiteSpace(roles))
                {
                    var jsonJudge = roles.Trim().StartsWith("[");

                    if (!string.IsNullOrWhiteSpace(roles) && jsonJudge)
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles);
                        if (parsedRoles != null)
                        {
                            foreach (var parsedRole in parsedRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                            }
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roles));
                        }
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// 解析来自Jwt的声明
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static Dictionary<string, string>? GetParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonBytes);
        }

        /// <summary>
        /// 解析没有填充的Base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
