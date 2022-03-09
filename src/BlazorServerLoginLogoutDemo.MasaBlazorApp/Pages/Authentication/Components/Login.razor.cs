namespace BlazorServerLoginLogoutDemo.MasaBlazorApp.Pages.Authentication.Components;

public partial class Login
{
    [Parameter]
    public EventCallback<LoginViewModel> OnLogin { get; set; }

    private LoginViewModel _loginViewModel = new();

    private async Task HandleOnLogin()
    {
        if (OnLogin.HasDelegate)
        {
            ButtonLoading = true;
            await OnLogin.InvokeAsync(_loginViewModel);
            ButtonLoading = false;
        }
    }

    /// <summary>
    /// 按钮是否加载
    /// </summary>
    private bool ButtonLoading = false;

    /// <summary>
    /// 登录密码显示
    /// </summary>
    private bool LoginPasswordShow = false;
}

