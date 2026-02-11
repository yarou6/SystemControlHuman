using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using SystemControlHuman.Views;

namespace SystemControlHuman.Services;

public class NavigationService
{
    public static void OpenMain(ApiService api, AuthService auth)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(api, auth); 
            desktop.MainWindow.Show();
        }
    }

    public static void OpenLogin(ApiService api, AuthService auth)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginWindow(api, auth); 
            desktop.MainWindow.Show();
        }
    }
}

