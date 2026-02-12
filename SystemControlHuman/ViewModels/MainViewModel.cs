using System;
using System.Threading.Tasks;
using SystemControlHuman.Services;
using SystemControlHuman.Tools;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Models.Auth;
using SystemControlHuman.Models.Shifts;

namespace SystemControlHuman.ViewModels;

public class MainViewModel : BaseVM
{
    private readonly ApiService api;
    private readonly AuthService auth;

    public EmployeesViewModel EmployeesView { get; }
    public ShiftsViewModel ShiftsView { get; }

    private string _currentUser = "";

    public string CurrentUser
    {
        get => _currentUser;
        private set => SetField(ref _currentUser, value);
    }

    private string _currentRole = "";

    public string CurrentRole
    {
        get => _currentRole;
        private set => SetField(ref _currentRole, value);
    }

    public RelayCommand LogoutCommand { get; }

    public MainViewModel(ApiService api, AuthService auth)
    {
        this.api = api;
        this.auth = auth;

        EmployeesView = new EmployeesViewModel(this.api);
        ShiftsView = new ShiftsViewModel(this.api);

        LogoutCommand = new RelayCommand(Logout);

        LoadProfile().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Console.WriteLine("Ошибка при загрузке профиля: " + task.Exception);
            }
        });
    }

    private async Task LoadProfile()
    {
        try
        {
            var profile = await api.GetProfileAsync();


            CurrentUser = profile?.Employee != null ? $"{profile.Employee.FirstName} {profile.Employee.LastName}" : "Нет данных";
            CurrentRole = profile?.Role?.Title ?? "Нет данных";
        }
        catch (Exception ex)
        {
            CurrentUser = "Ошибка";
            CurrentRole = "Ошибка";
        }
    }

    private async void Logout()
    {
        await auth.ClearTokenAsync();
        NavigationService.OpenLogin(api, auth);
    }
}