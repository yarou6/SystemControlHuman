using Avalonia.Controls;
 using SystemControlHuman.ViewModels;
 
 namespace SystemControlHuman.Views;
 
 public partial class EmployeesWindow : Window
 {
     public EmployeesWindow(EmployeesViewModel vm)
     {
         InitializeComponent();
         DataContext = vm;
     }
 }