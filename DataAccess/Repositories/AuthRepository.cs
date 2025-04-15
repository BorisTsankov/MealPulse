using MealPulse.Data;
using System.Data;

public class AuthRepository : IAuthRepository
{
    private readonly DbHelper _dbHelper;

    public AuthRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public bool UserExists(string email)
    {
        var parameters = new Dictionary<string, object> { { "@email", email } };
        string sql = "SELECT COUNT(*) FROM [User] WHERE email = @email";
        DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
        return dt.Rows.Count > 0 && (int)dt.Rows[0][0] > 0;
    }

    public int RegisterUser(Dictionary<string, object> parameters)
    {
        string sql = @"
            INSERT INTO [User] (FirstName, LastName, email, password, date_of_birth, gender_id, height_cm, activityLevel_id, metric_id, role_id)
            VALUES (@FirstName, @LastName, @email, @password, @date_of_birth, @gender_id, @height_cm, @activityLevel_id, @metric_id, @role_id)";
        return _dbHelper.ExecuteNonQuery(sql, parameters);
    }

    public DataTable AuthenticateUser(string email, string hashedPassword)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@email", email },
            { "@password", hashedPassword }
        };
        string sql = "SELECT user_id, email FROM [User] WHERE email = @email AND password = @password";
        return _dbHelper.ExecuteQuery(sql, parameters);
    }

    public DataTable GetSelectListData(string tableName, string valueField, string textField)
    {
        string sql = $"SELECT {valueField}, {textField} FROM {tableName}";
        return _dbHelper.ExecuteQuery(sql);
    }
}
