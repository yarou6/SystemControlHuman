using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Tools;

namespace SystemControlHuman.ViewModels;

public class EmployeeEditorViewModel : BaseVM
{
    private readonly ApiService api;
    private bool isEdit;
    public bool IsCreateMode => !isEdit;
    public string WindowTitle => isEdit ? "Редактирование сотрудника" : "Создание сотрудника";

    public EmployeeDto Employee { get; set; }
    public CredentialDto Credential { get; set; } = new CredentialDto();

    public ObservableCollection<RoleDto> Roles { get; } = new();
    private RoleDto selectedRole;
    public RoleDto SelectedRole
    {
        get => selectedRole;
        set
        {
            selectedRole = value;
            OnPropertyChanged(nameof(SelectedRole));
            if (selectedRole != null)
                Credential.RoleId = selectedRole.Id;
        }
    }

    public RelayCommand SaveCommand { get; }

    public EmployeeEditorViewModel(ApiService api, EmployeeWithRoleDto? employee = null)
    {
        this.api = api;
        isEdit = employee != null;

        Employee = employee?.Employee ?? new EmployeeDto { HireDate = DateTime.Now, IsActive = true };
        if (employee?.Role != null)
        {
            Credential.RoleId = employee.Role.Id;
        }

        SaveCommand = new RelayCommand(async () => await Save());

        if (!isEdit)
        {
            _ = LoadRoles();
        }
    }



    private async Task LoadRoles()
    {
        try
        {
            var employees = await api.GetEmployeesAsync();

            var uniqueRoles = employees
                .Select(e => e.Role)
                .Where(r => r != null)
                .DistinctBy(r => r.Id)
                .ToList();

            Roles.Clear();
            foreach (var role in uniqueRoles)
                Roles.Add(role);

            if (Roles.Count == 0)
            {
                Console.WriteLine("Роли не найдены. Добавьте хотя бы одну роль в БД.");
                return;
            }

            if (isEdit)
            {
                SelectedRole = Roles.FirstOrDefault(r => r.Id == Credential.RoleId) ?? Roles[0];
            }
            else
            {
                SelectedRole = Roles[0];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке ролей: {ex.Message}");
        }
    }


    private async Task Save()
    {
        try
        {
            if (isEdit)
            {
                await api.UpdateEmployeeAsync(Employee.Id, Employee);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Credential.Username) 
                    || string.IsNullOrWhiteSpace(Credential.PasswordHash))
                {
                    await ShowError("Имя пользователя и пароль обязательны!");
                    return;
                }

                if (SelectedRole == null)
                {
                    await ShowError("Не выбрана роль!");
                    return;
                }

                Credential.RoleId = SelectedRole.Id;

                var dto = new CreateEmployeeDto
                {
                    Employee = new EmployeeApiDto
                    {
                        Id = Employee.Id,
                        FirstName = Employee.FirstName,
                        LastName = Employee.LastName,
                        Position = Employee.Position,
                        HireDate = Employee.HireDate.DateTime, 
                        IsActive = Employee.IsActive
                    },
                    Credential = Credential
                };
                await api.CreateEmployeeAsync(dto);
            }

            CloseWindow(true);
        }
        catch (Exception ex)
        {
            await ShowError($"Ошибка при сохранении: {ex.Message}");
        }
    }


    private async Task ShowError(string message)
    {
        Console.WriteLine(message);
        await Task.CompletedTask;
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