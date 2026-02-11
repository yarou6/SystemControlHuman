using Avalonia.Controls;
using SystemControlHuman.Models.Shifts;
using SystemControlHuman.ViewModels;

namespace SystemControlHuman.Views;

public partial class ShiftEditorWindow : Window
{
    public ShiftEditorWindow(ApiService api, ShiftDto? shift = null)
    {
        InitializeComponent();
        DataContext = new ShiftEditorViewModel(api, shift);
    }
}