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
        SELECT TOP 20 * FROM FoodItem
        WHERE 
            LOWER(name) = LOWER(@term)
            OR LOWER(name) LIKE LOWER(@term + '%')
        ORDER BY 
            CASE 
                WHEN LOWER(name) = LOWER(@term) THEN 1
                WHEN LOWER(name) LIKE LOWER(@term + '%') THEN 2
                ELSE 3
            END,
            LEN(name),
            name";

            var parameters = new Dictionary<string, object>
    {
        { "@term", name }
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

        public FoodItem? GetByBarcode(string barcode)
        {
            string sql = "SELECT * FROM FoodItem WHERE Barcode = @barcode";
            var dt = _db.ExecuteQuery(sql, new() { { "@barcode", barcode } });

            return dt.Rows.Count > 0 ? MapRow(dt.Rows[0]) : null;
        }
        public int Add(FoodItem item)
        {
            string sql = @"
        INSERT INTO FoodItem 
        (name, calories, protein, fat, carbohydrates, sugars, fiber, sodium, potassium, iron, calcium, unit, description, brand, barcode, source, image_url, created_at)
        VALUES 
        (@Name, @Calories, @Protein, @Fat, @Carbs, @Sugars, @Fiber, @Sodium, @Potassium, @Iron, @Calcium, @Unit, @Description, @Brand, @Barcode, @Source, @ImageUrl, @CreatedAt);
        SELECT SCOPE_IDENTITY();";

            var parameters = new Dictionary<string, object?>
    {
        { "@Name", item.Name },
        { "@Calories", item.Calories },
        { "@Protein", item.Protein },
        { "@Fat", item.Fat },
        { "@Carbs", item.Carbohydrates },
        { "@Sugars", item.Sugars },
        { "@Fiber", item.Fiber },
        { "@Sodium", item.Sodium ?? (object)DBNull.Value },
        { "@Potassium", item.Potassium ?? (object)DBNull.Value },
        { "@Iron", item.Iron ?? (object)DBNull.Value },
        { "@Calcium", item.Calcium ?? (object)DBNull.Value },
        { "@Unit", item.Unit },
        { "@Description", item.Description ?? (object)DBNull.Value },
        { "@Brand", item.Brand ?? (object)DBNull.Value },
        { "@Barcode", item.Barcode ?? (object)DBNull.Value },
        { "@Source", item.Source ?? (object)DBNull.Value },
        { "@ImageUrl", item.ImageUrl ?? (object)DBNull.Value },
        { "@CreatedAt", item.CreatedAt }
    };

            var result = _db.ExecuteScalar<decimal>(sql, parameters);
            return (int)result;
        }




    }
}