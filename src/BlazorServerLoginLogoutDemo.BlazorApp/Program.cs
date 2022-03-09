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
//认证（Authentication）：用于认证登录权限，一般是登录账号名和密码确认的认证，保证用户认证资格。
app.UseAuthentication();
//授权（Authorization）：授权，比如很多后台，需要区分管理员，分级管理，这样通过授权机制，可以很好的进行权限管理。
app.UseAuthorization();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
