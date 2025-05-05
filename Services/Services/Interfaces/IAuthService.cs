using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Services.Services.Interfaces
{
    public interface IAuthService
    {
        bool UserExists(string email);
        string HashPassword(string password);
        int RegisterUser(Dictionary<string, object> parameters);
        DataTable AuthenticateUser(string email, string password);
        List<SelectListItem> GetSelectListData(string tableName, string valueField, string textField);
        string GetCurrentUserId();

    }
}
