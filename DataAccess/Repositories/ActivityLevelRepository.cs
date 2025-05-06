using DataAccess.Data;
using DataAccess.Repositories.Interfaces;
using Models.Models;
using System.Data;

namespace DataAccess.Repositories
{
    public class ActivityLevelRepository : IActivityLevelRepository
    {
        private readonly DbHelper _dbHelper;

        public ActivityLevelRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ActivityLevel> GetAll()
        {
            string query = "SELECT * FROM ActivityLevel";
            var dt = _dbHelper.ExecuteQuery(query);

            var result = new List<ActivityLevel>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new ActivityLevel
                {
                    ActivityLevelId = Convert.ToInt32(row["activityLevel_id"]),
                    ActivityLevelName = row["activityLevel"].ToString()!
                });
            }

            return result;
        }

        public ActivityLevel? GetById(int id)
        {
            string query = "SELECT * FROM ActivityLevel WHERE activityLevel_id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            var dt = _dbHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new ActivityLevel
            {
                ActivityLevelId = Convert.ToInt32(row["activityLevel_id"]),
                ActivityLevelName = row["activityLevel"].ToString()!
            };
        }

        public string GetActivityLevelNameById(int id)
        {
            return GetById(id)?.ActivityLevelName ?? "Unknown";
        }
    }
}
