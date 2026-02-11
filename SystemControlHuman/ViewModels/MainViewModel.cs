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

    public string CurrentUser { get; private set; } = "";
    public string CurrentRole { get; private set; } = "";

    public RelayCommand LogoutCommand { get; }

    public MainViewModel(ApiService api, AuthService auth)
    {
        this.api = api;
        this.auth = auth;

        EmployeesView = new EmployeesViewModel(this.api);
        ShiftsView = new ShiftsViewModel(this.api);

        LogoutCommand = new RelayCommand(Logout);

        LoadProfile();
    }

    private async void LoadProfile()
    {
        try
        {
            var profile = await api.GetProfileAsync();
            CurrentUser = profile.Employee.FirstName + " " + profile.Employee.LastName;
            CurrentRole = profile.Role.Title;
        }
        catch
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