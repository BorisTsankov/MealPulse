namespace DTOs.DTOs
{
    public class FoodDiaryItemDto
    {
        public int FoodDiaryItemId { get; set; }
        public int GoalId { get; set; }
        public int FoodId { get; set; }
        public int MealTypeId { get; set; }
        public DateTime DateTime { get; set; }
        public double Quantity { get; set; }

        public string? FoodName { get; set; }
        public string? MealTypeName { get; set; }

        public FoodItemDto? FoodItem { get; set; }
    }
}