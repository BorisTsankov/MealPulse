namespace Common
{
    public static class ValidationConstraints
    {
        public static class User
        {
            public const int FirstnameMaxLength = 50;
            public const int LastnameMaxLength = 50;
            public const int EmailMaxLength = 100;
            public const int Auth0IdMaxLength = 100;
            public const int AgeMin = 0;
            public const int AgeMax = 120;
            public const double HeightMin = 30.0;   // in cm
            public const double HeightMax = 300.0;  // in cm
        }

        public static class Role
        {
            public const int RoleNameMaxLength = 50;
        }

        public static class Gender
        {
            public const int GenderNameMaxLength = 50;
        }

        public static class ActivityLevel
        {
            public const int ActivityLevelNameMaxLength = 100;
        }

        public static class Metric
        {
            public const int MetricNameMaxLength = 50;
        }

        public static class FoodItem
        {
            public const int FoodItemNameMaxLength = 255; 
            public const int BrandMaxLength = 100;

            public const int DefaultServingQuantityMin = 1;
            public const int DefaultServingQuantityMax = 1000;

            public const double CaloriesMin = 0;
            public const double ProteinMin = 0;
            public const double FatsMin = 0;
            public const double CarbsMin = 0;
            public const double FiberMin = 0;
            public const double SugarsMin = 0;
            public const double SodiumMin = 0;
            public const double PotassiumMin = 0;
            public const double IronMin = 0;
            public const double CalciumMin = 0;
        }

        public static class Goal
        {
            public const double WeightMin = 40.0;
            public const double WeightMax = 300.0;
        }

        public static class MealType
        {
            public const int MealTypeNameMaxLength = 50;
        }
    }
}
