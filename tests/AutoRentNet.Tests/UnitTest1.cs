using AutoRentNet.Models;

namespace AutoRentNet.tests.AutoRentNet.Tests;

public class VehicleTests
{
    [Fact]
    public void Ctor_Sets_All_Props()
    {
        var v = new Vehicle
        {
            Id = 1,
            Name = "BMW 3",
            Seats = 5
        };

        Assert.Equal(5, v.Seats);
    }
}