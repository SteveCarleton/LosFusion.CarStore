using System.Text;
using System.Text.Json;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit.Abstractions;
using LosFusion.CarStore.BusinessLogicLayer.Entities;
using LosFusion.CarStore.BusinessLogicLayer.Interfaces;
//using Microsoft.AspNetCore.JsonPatch;

namespace LosFusion.CarStore.ServiceLayer.IntegrationTest;

public class CarTestValues
{
    public static readonly int carId = 1;
    public static readonly int carCount = 2;
    public static readonly int year = 2022;
    public static readonly Colors color = Colors.Blue;
    public static readonly string model = "Sport";
    public static readonly string modelChange = "Luxury";
};

public class CarIntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    const string route = "api/car";
    TestWebHelpers testWebHelpers;
    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public CarIntegrationTest(TestWebApplicationFactory factory, ITestOutputHelper output)
    {
        testWebHelpers = new TestWebHelpers(typeof(ICarRepository), typeof(CarRepositoryMock));
    }

    [Fact]
    public async Task TestGetCars()
    {
        var content = await testWebHelpers.GetTestHelper(route);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<List<CarEntity>>(content);
        data.Should().NotBeNull();
        data?.Count.Should().Be(CarTestValues.carCount);
    }

    [Fact]
    public async Task TestGetCarByYear()
    {
        var content = await testWebHelpers.GetByYearTestHelper(route, CarTestValues.year);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<List<CarEntity>>(content, jsonSerializerOptions);
        data.Should().NotBeNull();
        data?.Count.Should().Be(CarTestValues.carCount);
        data?[0].Year.Should().Be(CarTestValues.year);
        data?[1].Year.Should().Be(CarTestValues.year);
    }

    [Fact]
    public async Task TestPostCar()
    {
        var car = GenerateCar();
        var content = await testWebHelpers.PostTestHelper(route, car);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<CarEntity>(content, jsonSerializerOptions);
        data.Should().NotBeNull();
        data?.Id.Should().Be(CarTestValues.carId);
        data?.Year.Should().Be(CarTestValues.year);
        data?.Color.Should().Be(CarTestValues.color);
        data?.Model.Should().Be(CarTestValues.model);
    }

    [Fact]
    public async Task TestPutCar()
    {
        var car = GenerateCar();
        var content = await testWebHelpers.PutTestHelper(route, CarTestValues.carId, car);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<CarEntity>(content, jsonSerializerOptions);
        data.Should().NotBeNull();
        data?.Id.Should().Be(CarTestValues.carId);
        data?.Year.Should().Be(CarTestValues.year);
        data?.Color.Should().Be(CarTestValues.color);
        data?.Model.Should().Be(CarTestValues.modelChange);
    }

    [Fact]
    public async Task TestPatchCar()
    {
        //var car = GenerateCar();
        //var patch = new JsonPatchDocument<CarEntity>();
        //patch.Replace(e => e.Model, "Luxury");
        //string json = JsonSerializer.Serialize(patch);
        string json = $"[{{\"op\":\"replace\", \"path\":\"/Model\",\"value\":\"{CarTestValues.modelChange}\"}}]";

        var requestContent = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
        var content = await testWebHelpers.PatchTestHelper(route, CarTestValues.carId, requestContent);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<CarEntity>(content, jsonSerializerOptions);
        data.Should().NotBeNull();
        data?.Model.Should().Be(CarTestValues.modelChange);
    }

    [Fact]
    public async Task TestDeleteCar()
    {
        var content = await testWebHelpers.DeleteTestHelper<string>(route, CarTestValues.carId);
        content.Should().NotBeNull();
        var data = JsonSerializer.Deserialize<int>(content);
        data.Should().Be(1);
    }

    CarEntity GenerateCar()
    {
        return new CarEntity
        {
            Id = CarTestValues.carId,
            Model = CarTestValues.model,
            Year = CarTestValues.year,
            Color = CarTestValues.color
        };
    }
}


public class CarRepositoryMock : ICarRepository
{
    public Task<CarEntity> AddAsync(CarEntity item)
    {
        return Task.FromResult(item);
    }

    public Task DeleteAsync(int id)
    {
        return Task.FromResult(id);
    }

    public Task<CarEntity?> GetAsync(int id)
    {
        return Task.FromResult(Builder<CarEntity?>.CreateNew().With(c => c!.Id = id).Build());
    }

    public Task<List<CarEntity>> GetByYearAsync(int year)
    {
        return Task.FromResult(Builder<CarEntity>
            .CreateListOfSize(CarTestValues.carCount)
            .All()
            .With(c => c.Year = year)
            .And(c => c.Model = CarTestValues.model)
            .Build()
            .ToList());
    }

    public Task<List<CarEntity>> GetAsync()
    {
        return Task.FromResult(Builder<CarEntity>
            .CreateListOfSize(CarTestValues.carCount)
            .Build()
            .ToList());
    }

    public Task<CarEntity?> UpdateAsync(int id, CarEntity item)
    {
        return Task.FromResult(Builder<CarEntity?>
            .CreateNew()
            .With(c => c!.Id = id)
            .And(c => c!.Year = item.Year)
            .And(c => c!.Color = item.Color)
            .And(c => c!.Model = CarTestValues.modelChange)
            .Build());
    }
}
