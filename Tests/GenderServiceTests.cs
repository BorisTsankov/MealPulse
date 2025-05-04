using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Moq;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class GenderServiceTests
    {
        private readonly Mock<IGenderRepository> _mockGenderRepo;
        private readonly GenderService _genderService;

        public GenderServiceTests()
        {
            _mockGenderRepo = new Mock<IGenderRepository>();
            _genderService = new GenderService(_mockGenderRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfGenders()
        {
            // Arrange
            var genders = new List<Gender>
            {
                new Gender { GenderId = 1, GenderName = "Male" },
                new Gender { GenderId = 2, GenderName = "Female" }
            };

            _mockGenderRepo.Setup(repo => repo.GetAll()).Returns(genders);

            // Act
            var result = _genderService.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Male", result[0].GenderName);
            _mockGenderRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetGenderName_ReturnsCorrectName()
        {
            // Arrange
            int genderId = 1;
            string expectedName = "Male";

            _mockGenderRepo.Setup(repo => repo.GetGenderNameById(genderId)).Returns(expectedName);

            // Act
            var result = _genderService.GetGenderName(genderId);

            // Assert
            Assert.Equal(expectedName, result);
            _mockGenderRepo.Verify(repo => repo.GetGenderNameById(genderId), Times.Once);
        }
    }
}