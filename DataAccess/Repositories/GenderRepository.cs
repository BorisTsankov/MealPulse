using DataAccess.Repositories.Interfaces;
using MealPulse.Data;
using MealPulse.Models.Models;
using System.Data;

public class GenderRepository : IGenderRepository
{
    private readonly DbHelper _dbHelper;

    public GenderRepository(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public List<Gender> GetAll()
    {
        string query = "SELECT * FROM Gender";
        var dt = _dbHelper.ExecuteQuery(query);

        var result = new List<Gender>();
        foreach (DataRow row in dt.Rows)
        {
            result.Add(new Gender
            {
                GenderId = Convert.ToInt32(row["gender_id"]),
                GenderName = row["gender"].ToString()!
            });
        }

        return result;
    }

    public Gender? GetById(int id)
    {
        string query = "SELECT * FROM Gender WHERE gender_id = @gender_id";
        var parameters = new Dictionary<string, object> { { "@gender_id", id } };
        var dt = _dbHelper.ExecuteQuery(query, parameters);

        if (dt.Rows.Count == 0) return null;

        var row = dt.Rows[0];
        return new Gender
        {
            GenderId = Convert.ToInt32(row["gender_id"]),
            GenderName = row["gender"].ToString()!
        };
    }

    public string GetGenderNameById(int id)
    {
        return GetById(id)?.GenderName ?? "Unknown";
    }
}
