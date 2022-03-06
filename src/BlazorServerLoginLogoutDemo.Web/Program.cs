using BlazorServerLoginLogoutDemo.Web.Pages.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Blazor 向容器添加服务
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGlobalForServer();
//LocalStorage：用于Web浏览器本地数据存储
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<LoginService>();
builder.Services.AddScoped<AuthenticationStateProvider, SystemAuthenticationStateProvider>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
#endregion

#region Masa 向容器添加服务
builder.Services.AddMasaI18nForServer("wwwroot/i18n");
builder.Services.AddMasaBlazor(builder =>
{
    builder.UseTheme(option =>
    {
        option.Primary = "#4318FF";
        option.Accent = "#4318FF";
    }
    );
});
#endregion

var app = builder.Build();

#region 应用启动与关闭
var hostApplicationLifetime = app.Services.GetService<IHostApplicationLifetime>();
hostApplicationLifetime?.ApplicationStarted.Register(() =>
{

    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -> Blazor Server 启动成功");
});
hostApplicationLifetime?.ApplicationStopping.Register(() =>
{
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -> Blazor Server 已关闭");
});
#endregion

#region 管道配置
if (app.Environment.IsDevelopment())
{

}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//认证（Authentication）：用于认证登录权限，一般是登录账号名和密码确认的认证，保证用户认证资格。
app.UseAuthentication();
//授权（Authorization）：授权，比如很多后台，需要区分管理员，分级管理，这样通过授权机制，可以很好的进行权限管理。
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
#endregion