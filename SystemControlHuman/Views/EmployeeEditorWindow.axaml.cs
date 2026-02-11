using Avalonia.Controls;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class EmployeeEditorWindow : Window
{
    public EmployeeEditorWindow(ApiService api, EmployeeDto? employee = null)
    {
        InitializeComponent();
        DataContext = new EmployeeEditorViewModel(api, employee);
    }
}