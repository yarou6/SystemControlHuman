using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SystemControlHuman.Models.Auth;
using SystemControlHuman.Models.Employees;
using SystemControlHuman.Models.Shifts;
using SystemControlHuman.Tools;

public class ApiService
{
    private readonly HttpClient http;
    private readonly AuthService auth;

    public event Action? OnUnauthorized;

    public ApiService(AuthService auth)
    {
        this.auth = auth;

        http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5246/")
        };
        auth.OnTokenChanged += ApplyAuth;
        ApplyAuth(); 
    }
    private void ApplyAuth()
    {
        http.DefaultRequestHeaders.Authorization = null;

        if (auth.IsAuthorized)
        {
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", auth.Token);
        }
        
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            OnUnauthorized?.Invoke();
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    private async Task HandleResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            OnUnauthorized?.Invoke();
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        response.EnsureSuccessStatusCode();
        await Task.CompletedTask;
    }

    //Регистр

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", dto);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized");

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
    }

    public async Task<EmployeeWithRoleDto> GetProfileAsync()
    {
        ApplyAuth();
        var response = await http.PostAsync("api/auth/profile", null);
        return await HandleResponse<EmployeeWithRoleDto>(response);
    }

    //Люди
    public async Task<List<EmployeeWithRoleDto>> GetEmployeesAsync()
    {
        ApplyAuth();
        var response = await http.GetAsync("api/employees");
        return await HandleResponse<List<EmployeeWithRoleDto>>(response);
    }

    public async Task<EmployeeWithRoleDto> GetEmployeeAsync(int id)
    {
        ApplyAuth();
        var response = await http.GetAsync($"api/employees/{id}");
        return await HandleResponse<EmployeeWithRoleDto>(response);
    }

    public async Task CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        ApplyAuth();
        var response = await http.PostAsJsonAsync("api/employees", dto);
        await HandleResponse(response);
    }

    public async Task UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        ApplyAuth();
        var response = await http.PutAsJsonAsync($"api/employees/{id}", dto);
        await HandleResponse(response);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        ApplyAuth();
        var response = await http.DeleteAsync($"api/employees/{id}");
        await HandleResponse(response);
    }

    //Шифт
    public async Task<List<ShiftWithEmployeeDto>> GetShiftsAsync()
    {
        ApplyAuth();
        var response = await http.GetAsync("api/shifts");
        return await HandleResponse<List<ShiftWithEmployeeDto>>(response);
    }

    public async Task<ShiftWithEmployeeDto> GetShiftAsync(int id)
    {
        ApplyAuth();
        var response = await http.GetAsync($"api/shifts/{id}");
        return await HandleResponse<ShiftWithEmployeeDto>(response);
    }

    public async Task<List<ShiftDto>> GetShiftsByEmployeeAsync(int employeeId)
    {
        ApplyAuth();
        var response = await http.GetAsync($"api/shifts/employee/{employeeId}");
        return await HandleResponse<List<ShiftDto>>(response);
    }

    public async Task CreateShiftAsync(ShiftDto dto)
    {
        ApplyAuth();
        var response = await http.PostAsJsonAsync("api/shifts", dto);
        await HandleResponse(response);
    }

    public async Task UpdateShiftAsync(int id, ShiftDto dto)
    {
        ApplyAuth();
        var response = await http.PutAsJsonAsync($"api/shifts/{id}", dto);
        await HandleResponse(response);
    }

    public async Task DeleteShiftAsync(int id)
    {
        ApplyAuth();
        var response = await http.DeleteAsync($"api/shifts/{id}");
        await HandleResponse(response);
    }
}
