using System.Data;

public interface IAuthRepository
{
    bool UserExists(string email);
    int RegisterUser(Dictionary<string, object> parameters);
    DataTable AuthenticateUser(string email, string hashedPassword);
    DataTable GetSelectListData(string tableName, string valueField, string textField);
}
