using MealPulse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public bool UserExists(string email)
    {
        return _authRepository.UserExists(email);
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public int RegisterUser(Dictionary<string, object> parameters)
    {
        return _authRepository.RegisterUser(parameters);
    }

    public DataTable AuthenticateUser(string email, string password)
    {
        string hashed = HashPassword(password);
        return _authRepository.AuthenticateUser(email, hashed);
    }

    public List<SelectListItem> GetSelectListData(string tableName, string valueField, string textField)
    {
        DataTable dt = _authRepository.GetSelectListData(tableName, valueField, textField);
        return dt.AsEnumerable()
                 .Select(row => new SelectListItem
                 {
                     Value = row[valueField].ToString(),
                     Text = row[textField].ToString()
                 })
                 .ToList();
    }
}
