using System;
using System.Threading.Tasks;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Tools;

namespace SystemControlHuman.ViewModels;

public class EmployeeEditorViewModel : BaseVM
{
    private readonly ApiService api;
    private readonly bool isEdit;

    public EmployeeDto Employee { get; set; }

    public RelayCommand SaveCommand { get; }

    public EmployeeEditorViewModel(ApiService api, EmployeeDto? employee = null)
    {
        this.api = api;
        isEdit = employee != null;

        Employee = employee != null ? new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Position = employee.Position,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        } : new EmployeeDto { HireDate = DateTime.Now, IsActive = true };

        SaveCommand = new RelayCommand(async () => await Save());
    }

    private async Task Save()
    {
        if (isEdit)
            await api.UpdateEmployeeAsync(Employee.Id, Employee);
        else
            await api.CreateEmployeeAsync(new CreateEmployeeDto { Employee = Employee, Credential = null });

        CloseWindow(true);
    }

    private void CloseWindow(bool result)
    {
        if (App.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            foreach (var w in desktop.Windows)
            {
                if (w.DataContext == this)
                {
                    w.Close(result);
                    break;
                }
            }
        }
    }
}