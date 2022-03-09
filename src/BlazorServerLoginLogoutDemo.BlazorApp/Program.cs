using Blazored.LocalStorage;
using BlazorServerLoginLogoutDemo.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthenticationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<LoginService>();
builder.Services.AddScoped<AuthenticationStateProvider, SystemAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
//��֤��Authentication����������֤��¼Ȩ�ޣ�һ���ǵ�¼�˺���������ȷ�ϵ���֤����֤�û���֤�ʸ�
app.UseAuthentication();
//��Ȩ��Authorization������Ȩ������ܶ��̨����Ҫ���ֹ���Ա���ּ���������ͨ����Ȩ���ƣ����ԺܺõĽ���Ȩ�޹���
app.UseAuthorization();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
