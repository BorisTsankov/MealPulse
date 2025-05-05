using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Repositories
{
    public class MealTypeRepository : IMealTypeRepository
    {
        private readonly DbHelper _dbHelper;

        public MealTypeRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<MealType> GetAll()
        {
            var query = "SELECT * FROM MealType";
            var dt = _dbHelper.ExecuteQuery(query);

            var result = new List<MealType>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new MealType
                {
                    MealTypeId = Convert.ToInt32(row["mealType_id"]),
                    MealTypeName = row["mealType"].ToString()!
                });
            }

            return result;
        }
    }
}
