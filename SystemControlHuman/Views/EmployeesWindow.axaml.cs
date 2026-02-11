using Avalonia.Controls;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class EmployeesWindow : Window
{
    public EmployeesWindow(ApiService api)
    {
        InitializeComponent();
        DataContext = new EmployeesViewModel(api);
    }
}