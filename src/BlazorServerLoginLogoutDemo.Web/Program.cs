using BlazorServerLoginLogoutDemo.Web.Pages.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Blazor ��������ӷ���
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGlobalForServer();
//LocalStorage������Web������������ݴ洢
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

#region Masa ��������ӷ���
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

#region Ӧ��������ر�
var hostApplicationLifetime = app.Services.GetService<IHostApplicationLifetime>();
hostApplicationLifetime?.ApplicationStarted.Register(() =>
{

    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -> Blazor Server �����ɹ�");
});
hostApplicationLifetime?.ApplicationStopping.Register(() =>
{
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} -> Blazor Server �ѹر�");
});
#endregion

#region �ܵ�����
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
//��֤��Authentication����������֤��¼Ȩ�ޣ�һ���ǵ�¼�˺���������ȷ�ϵ���֤����֤�û���֤�ʸ�
app.UseAuthentication();
//��Ȩ��Authorization������Ȩ������ܶ��̨����Ҫ���ֹ���Ա���ּ���������ͨ����Ȩ���ƣ����ԺܺõĽ���Ȩ�޹���
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
#endregion