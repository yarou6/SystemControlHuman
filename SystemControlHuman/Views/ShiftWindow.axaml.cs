using Avalonia.Controls;
using SystemControlHuman.Models.Shifts;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class ShiftWindow : Window
{
    public ShiftWindow(ApiService api)
    {
        InitializeComponent();
        DataContext = new ShiftsViewModel(api);
    }

    public ShiftWindow(ApiService api, ShiftDto shift)
    {
        InitializeComponent();
        DataContext = new ShiftsViewModel(api) { SelectedShift = shift };
    }
}