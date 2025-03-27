using LiteDB;
using System.IO;
using Xunit;
using Cars_Management.Pages;


public class CarTests
{
    private ILiteDatabase GetInMemoryDatabase()
    {
        return new LiteDatabase(new MemoryStream());
    }

    [Fact]
    public void GetCarById_ShouldReturnCorrectCar()
    {
        var db = GetInMemoryDatabase();
        var carsCollection = db.GetCollection<Car>("cars");
        var car = new Car { Id = 1, CarBrand = "Toyota", Model = "Corolla", ProductionYear = 2020 };
        carsCollection.Insert(car);

        var retrievedCar = carsCollection.FindById(1);

        Assert.NotNull(retrievedCar);
        Assert.Equal("Toyota", retrievedCar.CarBrand);
        Assert.Equal("Corolla", retrievedCar.Model);
        Assert.Equal(2020, retrievedCar.ProductionYear);
    }

    [Fact]
    public void UpdateCar_ShouldModifyCarDetails()
    {
        var db = GetInMemoryDatabase();
        var carsCollection = db.GetCollection<Car>("cars");
        var car = new Car { Id = 1, CarBrand = "Toyota", Model = "Corolla", ProductionYear = 2020 };
        carsCollection.Insert(car);

        car.Model = "Camry";
        car.ProductionYear = 2022;
        carsCollection.Update(car);

        var updatedCar = carsCollection.FindById(1);

        Assert.Equal("Camry", updatedCar.Model);
        Assert.Equal(2022, updatedCar.ProductionYear);
    }

    [Fact]
    public void DeleteCar_ShouldRemoveCarFromDatabase()
    {
        var db = GetInMemoryDatabase();
        var carsCollection = db.GetCollection<Car>("cars");
        var car = new Car { Id = 1, CarBrand = "Toyota", Model = "Corolla", ProductionYear = 2020 };
        carsCollection.Insert(car);

        carsCollection.Delete(1);

        Assert.Empty(carsCollection.FindAll());
    }

}
