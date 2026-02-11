using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Models.Shifts;
using SystemControlHuman.Tools;

namespace SystemControlHuman.ViewModels;

public class ShiftEditorViewModel : BaseVM
{
    private readonly ApiService api;
    private readonly bool isEdit;

    public ShiftDto Shift { get; set; }

    public ObservableCollection<EmployeeDto> Employees { get; } = new();

    public RelayCommand SaveCommand { get; }

    public ShiftEditorViewModel(ApiService api, ShiftDto? shift = null)
    {
        this.api = api;
        isEdit = shift != null;

        Shift = shift != null
            ? new ShiftDto
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Description = shift.Description
            }
            : new ShiftDto
            {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(1)
            };

        SaveCommand = new RelayCommand(async () => await SaveAsync());

        LoadEmployees();
    }

    private async void LoadEmployees()
    {
        Employees.Clear();
        var list = await api.GetEmployeesAsync();
        foreach (var emp in list)
            Employees.Add(emp.Employee);
    }

    private async Task SaveAsync()
    {
        if (isEdit)
            await api.UpdateShiftAsync(Shift.Id, Shift);
        else
            await api.CreateShiftAsync(Shift);

        CloseWindow(true);
    }

    private void CloseWindow(bool result)
    {
        if (App.Current.ApplicationLifetime is
            Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
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