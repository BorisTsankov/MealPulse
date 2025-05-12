using DataAccess.Data;
using DataAccess.Repositories.Interfaces;
using Models.Models;
using System.Data;


namespace DataAccess.Repositories
{

    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly DbHelper _db;

        public FoodItemRepository(DbHelper db)
        {
            _db = db;
        }

        public List<FoodItem> GetAll()
        {
            string sql = "SELECT * FROM FoodItem";
            var dt = _db.ExecuteQuery(sql);
            var result = new List<FoodItem>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(MapRow(row));
            }

            return result;
        }

        public FoodItem? GetById(int id)
        {
            string sql = "SELECT * FROM FoodItem WHERE FoodItemId = @id";
            var dt = _db.ExecuteQuery(sql, new() { { "@id", id } });

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        private FoodItem MapRow(DataRow row)
        {
            return new FoodItem
            {
                FoodItemId = (int)row["FoodItemId"],
                Name = row["Name"].ToString()!,
                Unit = row["Unit"].ToString()!,
                Calories = (decimal)row["Calories"],
                Protein = (decimal)row["Protein"],
                Fat = (decimal)row["Fat"],
                Carbohydrates = (decimal)row["Carbohydrates"],
                Sugars = (decimal)row["Sugars"],
                Fiber = (decimal)row["Fiber"],
                Sodium = (decimal)row["Sodium"],
                Potassium = (decimal)row["Potassium"],
                Iron = (decimal)row["Iron"],
                Calcium = (decimal)row["Calcium"]
            };
        }

        public List<FoodItem> SearchByName(string name)
        {
            string sql = @"
        SELECT TOP 50 * FROM FoodItem
        WHERE name LIKE @term
        ORDER BY 
            CASE 
                WHEN LOWER(name) = LOWER(@exact) THEN 1
                WHEN LOWER(name) LIKE LOWER(@startsWith + '%') THEN 2
                WHEN LOWER(name) LIKE LOWER('%' + @term + '%') THEN 3
                ELSE 4
            END, 
            LEN(name), 
            name";

            var parameters = new Dictionary<string, object>
    {
        { "@term", "%" + name + "%" },
        { "@exact", name },
        { "@startsWith", name }
    };

            var dt = _db.ExecuteQuery(sql, parameters);
            var items = new List<FoodItem>();

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new FoodItem
                {
                    FoodItemId = (int)row["FoodItemId"],
                    Name = row["name"].ToString()!
                });
            }

            return items;
        }
    }
}