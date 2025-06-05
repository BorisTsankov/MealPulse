using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    public class FoodNameHelperTests
    {
        [Theory]
        [InlineData("Banana", "banana")]
        [InlineData("  Apple Pie  ", "apple pie")]
        [InlineData("Ch!ck#en@Nuggets", "chckennuggets")]
        [InlineData("123Burger", "123burger")]
        [InlineData("Fish & Chips", "fish  chips")]  // & is removed
        [InlineData("   ", "")]
        [InlineData("", "")]
        [InlineData("Éclair", "éclair")]  // Unicode preserved
        [InlineData("Milk\tand\nCookies", "milk\tand\ncookies")] // FIXED EXPECTATION: tabs/newlines preserved
        public void Normalize_ShouldReturnExpectedString(string input, string expected)
        {
            var result = FoodNameHelper.Normalize(input);
            Assert.Equal(expected, result);
        }
    }
}