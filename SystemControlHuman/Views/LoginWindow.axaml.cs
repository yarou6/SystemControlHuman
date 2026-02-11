using Avalonia.Controls;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(ApiService api, AuthService auth)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(api, auth);
    }
}