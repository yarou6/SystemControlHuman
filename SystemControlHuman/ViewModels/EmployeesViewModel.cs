using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Tools;
using SystemControlHuman.Views;


namespace SystemControlHuman.ViewModels;

public class EmployeesViewModel : BaseVM
{
    private readonly ApiService api;

    public ObservableCollection<EmployeeDto> Employees { get; } = new();
    

    private EmployeeDto? _selectedEmployee;
    public EmployeeDto? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            SetField(ref _selectedEmployee, value);
            EditEmployeeCommand.RaiseCanExecuteChanged();
            DeleteEmployeeCommand.RaiseCanExecuteChanged();
        }
    }
    public RelayCommand AddEmployeeCommand { get; }
    public RelayCommand EditEmployeeCommand { get; }
    public RelayCommand DeleteEmployeeCommand { get; }

    public EmployeesViewModel(ApiService api)
    {
        this.api = api;

        AddEmployeeCommand = new RelayCommand(async () => await AddEmployee());
        EditEmployeeCommand = new RelayCommand(async () => await EditEmployee(), () => SelectedEmployee != null);
        DeleteEmployeeCommand = new RelayCommand(async () => await DeleteEmployee(), () => SelectedEmployee != null); 
        LoadEmployees();
    }


    private async void LoadEmployees()
    {
        try
        {
            Employees.Clear();
            var list = await api.GetEmployeesAsync();

            Console.WriteLine("Employees count: " + list?.Count);

            foreach (var emp in list)
                Employees.Add(emp.Employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки сотрудников: " + ex);
        }
    }
    
    private async Task AddEmployee()
    {
        var editor = new EmployeeEditorWindow(api);
        var parentWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parentWindow != null)
        {
            var result = await editor.ShowDialog<bool>(parentWindow);
            if (result) LoadEmployees();
        }
    }

    private async Task EditEmployee()
    {
        if (SelectedEmployee == null) return;

        var employeeWithRole = await api.GetEmployeeAsync(SelectedEmployee.Id);

        var editor = new EmployeeEditorWindow(api, employeeWithRole);

        var parentWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parentWindow != null)
        {
            var result = await editor.ShowDialog<bool>(parentWindow);
            if (result) LoadEmployees();
        }
    }


    private async Task DeleteEmployee()
    {
        if (SelectedEmployee == null) return;
        await api.DeleteEmployeeAsync(SelectedEmployee.Id);
        LoadEmployees();
    }
}