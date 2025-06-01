using DataAccess.Repositories.Interfaces;
using DTOs.DTOs;
using Models.Models;
using Moq;
using Services.Services;
using Services.Services.Interfaces;

public class FoodItemServiceTests
{
    private readonly Mock<IOpenFoodFactsService> _offMock = new();

    private readonly Mock<IFoodItemRepository> _repoMock = new();
    private readonly FoodItemService _service;

    public FoodItemServiceTests()
    {
        _service = new FoodItemService(_repoMock.Object, _offMock.Object);
    }

    [Fact]
    public void GetAll_ReturnsMappedDtos()
    {
        var items = new List<FoodItem>
        {
            new() { FoodItemId = 1, Name = "Egg", Calories = 100 },
            new() { FoodItemId = 2, Name = "Milk", Calories = 200 }
        };
        _repoMock.Setup(r => r.GetAll()).Returns(items);

        var result = _service.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Name == "Egg");
    }

    [Fact]
    public void GetById_ItemExists_ReturnsDto()
    {
        var item = new FoodItem { FoodItemId = 1, Name = "Apple", Calories = 52 };
        _repoMock.Setup(r => r.GetById(1)).Returns(item);

        var result = _service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Apple", result!.Name);
    }

    [Fact]
    public void GetById_ItemMissing_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetById(1)).Returns((FoodItem?)null);

        var result = _service.GetById(1);

        Assert.Null(result);
    }

    [Fact]
    public void SearchByName_MatchesExist_ReturnsDtos()
    {
        var items = new List<FoodItem> { new() { Name = "Egg", Calories = 70 } };
        _repoMock.Setup(r => r.SearchByName("egg")).Returns(items);

        var result = _service.SearchByName("egg");

        Assert.Single(result);
        Assert.Equal("Egg", result[0].Name);
    }

    [Fact]
    public async Task GetByBarcodeOrFetchAsync_LocalExists_ReturnsLocal()
    {
        var local = new FoodItem { FoodItemId = 5, Name = "Cheese", Barcode = "123" };
        _repoMock.Setup(r => r.GetByBarcode("123")).Returns(local);

        var result = await _service.GetByBarcodeOrFetchAsync("123");

        Assert.NotNull(result);
        Assert.Equal("Cheese", result!.Name);
    }

    [Fact]
    public async Task GetByBarcodeOrFetchAsync_FetchesFromApi_AndInserts()
    {
        var apiDto = new FoodItemDto
        {
            Name = "Yogurt",
            Barcode = "999",
            Calories = 80
        };

        _repoMock.Setup(r => r.GetByBarcode("999")).Returns((FoodItem?)null);
        _offMock.Setup(r => r.GetFoodItemByBarcodeAsync("999")).ReturnsAsync(apiDto);
        _repoMock.Setup(r => r.Add(It.IsAny<FoodItem>())).Returns(77);

        var result = await _service.GetByBarcodeOrFetchAsync("999");

        Assert.NotNull(result);
        Assert.Equal("Yogurt", result!.Name);
        Assert.Equal(77, result.FoodItemId);
    }

    [Fact]
    public async Task GetByBarcodeOrFetchAsync_ApiReturnsNull_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByBarcode("notfound")).Returns((FoodItem?)null);
        _offMock.Setup(r => r.GetFoodItemByBarcodeAsync("notfound")).ReturnsAsync((FoodItemDto?)null);

        var result = await _service.GetByBarcodeOrFetchAsync("notfound");

        Assert.Null(result);
    }

    [Fact]
    public async Task SearchByNameOrFetchAsync_LocalMatchFound_ReturnsLocal()
    {
        var local = new List<FoodItem> { new() { Name = "Egg", Calories = 80 } };
        _repoMock.Setup(r => r.SearchByName("egg")).Returns(local);

        var result = await _service.SearchByNameOrFetchAsync("egg");

        Assert.Single(result);
        Assert.Equal("Egg", result[0].Name);
    }

    [Fact]
    public async Task SearchByNameOrFetchAsync_ApiFallback_ReturnsAndInserts()
    {
        _repoMock.Setup(r => r.SearchByName("beetroot")).Returns(new List<FoodItem>());
        _repoMock.Setup(r => r.GetByBarcode(It.IsAny<string>())).Returns((FoodItem?)null);
        _repoMock.Setup(r => r.SearchByName(It.IsAny<string>())).Returns(new List<FoodItem>());

        var apiList = new List<FoodItemDto>
        {
            new() { Name = "Beetroot", Calories = 50, Protein = 1, Fat = 0.1m }
        };

        _offMock.Setup(s => s.SearchByNameAsync("beetroot")).ReturnsAsync(apiList);
        _repoMock.Setup(r => r.Add(It.IsAny<FoodItem>())).Returns(10);

        var result = await _service.SearchByNameOrFetchAsync("beetroot");

        Assert.Single(result);
        Assert.Equal("Beetroot", result[0].Name);
        Assert.Equal(10, result[0].FoodItemId);
    }

    [Fact]
    public async Task SearchByNameOrFetchAsync_ApiReturnsIncompleteItem_SkipsInsertion()
    {
        _repoMock.Setup(r => r.SearchByName("fakefood")).Returns(new List<FoodItem>());

        var apiList = new List<FoodItemDto>
        {
            new() { Name = "FakeFood", Calories = 0, Protein = 0, Fat = 0 }
        };

        _offMock.Setup(s => s.SearchByNameAsync("fakefood")).ReturnsAsync(apiList);

        var result = await _service.SearchByNameOrFetchAsync("fakefood");

        Assert.Empty(result); // item skipped due to being nutritionally incomplete
    }

    [Fact]
    public void GetByName_ValidItem_ReturnsDto()
    {
        var items = new List<FoodItem>
        {
            new() { Name = "Rice", Calories = 120 },
            new() { Name = "Rice", Calories = 0 } // Should be ignored
        };

        _repoMock.Setup(r => r.SearchByName("Rice")).Returns(items);

        var result = _service.GetByName("Rice");

        Assert.NotNull(result);
        Assert.Equal("Rice", result!.Name);
    }

    [Fact]
    public void GetByName_NoValidItems_ReturnsNull()
    {
        _repoMock.Setup(r => r.SearchByName("Void")).Returns(new List<FoodItem>());

        var result = _service.GetByName("Void");

        Assert.Null(result);
    }

    [Fact]
    public void Add_ValidDto_CallsRepoAddAndReturnsId()
    {
        var dto = new FoodItemDto { Name = "Avocado", Calories = 160 };
        _repoMock.Setup(r => r.Add(It.IsAny<FoodItem>())).Returns(123);

        var result = _service.Add(dto);

        Assert.Equal(123, result);
    }
}
