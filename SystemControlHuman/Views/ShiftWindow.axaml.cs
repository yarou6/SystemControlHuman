using Avalonia.Controls;
using SystemControlHuman.Models.Shifts;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class ShiftWindow : Window
{
    public ShiftWindow(ShiftsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

}