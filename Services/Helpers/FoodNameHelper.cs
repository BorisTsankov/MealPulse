using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class FoodNameHelper
    {
        public static string Normalize(string name)
        {
            return new string(name
                .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)) // call methods with (c)
                .ToArray())
                .Trim()
                .ToLower();
        }
    }
}