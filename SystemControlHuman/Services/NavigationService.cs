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
            var current = desktop.MainWindow;

            var main = new MainWindow(api, auth);
            desktop.MainWindow = main;
            main.Show();

            current?.Close();
        }
    }

    public static void OpenLogin(ApiService api, AuthService auth)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var current = desktop.MainWindow;

            var login = new LoginWindow(api, auth);
            desktop.MainWindow = login;
            login.Show();

            current?.Close();
        }
    }
}

