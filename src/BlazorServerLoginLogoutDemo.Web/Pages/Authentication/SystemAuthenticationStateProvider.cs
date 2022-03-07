using Microsoft.AspNetCore.Components.Authorization;
using BlazorServerLoginLogoutDemo.Web.Models;
using System.Security.Claims;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

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

                identity = new ClaimsIdentity(claims, AuthenticationServiceKey.AuthenticationType);
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="token"></param>
        public async Task LoginAsync(string token)
        {
            await _localStorageService.SetItemAsync(AuthenticationServiceKey.Token, token);
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
            string? token;

            try
            {
                //浏览器还未加载完js时，不能使用LocalStorage
                token = await _localStorageService.GetItemAsync<string>(AuthenticationServiceKey.Token);
            }
            catch (Exception)
            {
                return user;
            }

            if (!string.IsNullOrEmpty(token))
            {
                var claims = GetJwtDeserialize(token);
                if (claims != null)
                {
                    var id = claims.GetValueOrDefault("ID");
                    if (id != null)
                    {
                        user.ID = id;
                        user.IsAuthenticated = true;
                    }
                    user.Roles = GetParseClaimsFromJwt(token);
                    var userName = claims.GetValueOrDefault(JwtRegisteredClaimNames.Name);
                    if (userName != null)
                    {
                        user.UserName = userName;
                    }
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
        private static IEnumerable<Claim> GetParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var keyValuePairs = GetJwtDeserialize(jwt);

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

                keyValuePairs.TryGetValue(ClaimTypes.Name, out string? userName);
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    claims.Add(new Claim(ClaimTypes.Name, userName));
                }
            }

            return claims;
        }

        /// <summary>
        /// 解析来自Jwt的声明
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        private static Dictionary<string, string>? GetJwtDeserialize(string jwt)
        {
            var jsonBytes = Decode(JwtParts(jwt));
            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonBytes);
        }

        /// <summary>
        /// Creates a new instance of <see cref="JwtParts" /> from the string representation of a JWT
        /// </summary>
        /// <param name="token">The string representation of a JWT</param>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentOutOfRangeException" />
        public static string JwtParts(string token)
        {
            return token.Split('.')[1];
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException" />
        /// <exception cref="FormatException" />
        public static byte[] Decode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new FormatException("Illegal base64url string.");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
