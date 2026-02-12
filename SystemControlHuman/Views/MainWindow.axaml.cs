using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class MainWindow : Window
{
    public MainWindow(ApiService api, AuthService auth)
    {
        InitializeComponent();
        
        DataContext = new MainViewModel(api, auth);
    }


    private void OpenShiftWindow(object? sender, RoutedEventArgs e)
    {
        var vm = ((MainViewModel)DataContext).ShiftsView;
        var window = new ShiftWindow(vm);
        window.Show();
    }

    private void OpenEmployeesWindow(object? sender, RoutedEventArgs e)
    {
        var vm = ((MainViewModel)DataContext).EmployeesView;
        var window = new EmployeesWindow(vm);
        window.Show();
    }
}