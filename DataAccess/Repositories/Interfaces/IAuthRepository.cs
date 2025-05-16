using System.Data;


namespace DataAccess.Repositories.Interfaces
{

    public interface IAuthRepository
    {
        bool UserExists(string email);
        int RegisterUser(Dictionary<string, object> parameters);
        DataTable AuthenticateUser(string email, string hashedPassword);
        DataTable GetSelectListData(string tableName, string valueField, string textField);
        bool ConfirmEmail(string token);
        bool SetPasswordResetToken(string email, string token, DateTime expiry);
        bool ResetPassword(string token, string hashedPassword);

    }
}
