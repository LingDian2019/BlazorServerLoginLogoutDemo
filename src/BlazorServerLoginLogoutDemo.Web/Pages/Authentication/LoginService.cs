using BlazorServerLoginLogoutDemo.Web.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorServerLoginLogoutDemo.Web.Pages.Authentication
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public  class LoginService
    {
        List<UserInfo> userInfos = new()
        {
            new UserInfo(){ID = "1111111111111111111111111111111",UserName="零点一号",LoginAccount = "admin1",LoginPassword = "admin1"},
            new UserInfo(){ID = "2222222222222222222222222222222",UserName="零点二号",LoginAccount = "admin3",LoginPassword = "admin2"},
            new UserInfo(){ID = "3333333333333333333333333333333",UserName="零点三号",LoginAccount = "admin3",LoginPassword = "admin3"},
        };

        public UserInfo? GetUserInfo(string loginAccount, string loginPassword)
        {
            return userInfos.FirstOrDefault(x => x.LoginAccount == loginAccount && x.LoginPassword == loginPassword);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="key">授权账户名称</param>
        /// <param name="id">用户ID</param>
        /// <param name="idkey">用户ID key</param>
        /// <returns></returns>
        public JwtInfo GetToken(string key, string id, string idkey = "ID")
        {
            var dictionarys = new Dictionary<string, string>
            {
               { idkey, id }
            };
            return CreateToken(key, dictionarys);
        }

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="key">授权账户名称</param>
        /// <param name="dictionarys">其他授权信息</param>
        /// <returns></returns>
        private JwtInfo CreateToken(string key, Dictionary<string, string> dictionarys)
        {
            var currentTime = DateTime.Now;
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti,key),
                // 令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(currentTime).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(currentTime).ToUnixTimeSeconds()}"),
                //过期时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(currentTime.AddSeconds(28800)).ToUnixTimeSeconds()}"),
                //签发者
                new Claim(JwtRegisteredClaimNames.Iss,"LingDian"), 
                //接收者
                new Claim(JwtRegisteredClaimNames.Aud,"Client"),
            };

            foreach (var item in dictionarys)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            //密钥长度不少于128位，也就是至少16个字符（128位指二进制位数是128。 1个字节是8位2进制。所以等于128/8 = 16个字节。）
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LingDianTestTokenKey".PadLeft(16, '0')));

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var accessTokenInfo = new JwtInfo
            {
                ExpirationTimeUnixTimeSeconds = new DateTimeOffset(currentTime.AddDays(1)).ToUnixTimeSeconds(),
                Token = jwtToken,
                TokenType = "Bearer"
            };

            return accessTokenInfo;
        }
    }
}
