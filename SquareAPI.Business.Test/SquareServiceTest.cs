using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SquareAPI.Business.Models;
using SquareAPI.Data;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business.Test;

[TestClass]
public class SquareServiceTest
{
    [TestMethod]
    public async Task GetSquareTest()
    {
        int userId = 1;
        SquarePoint expectedResult = getExprectedResult();

        var mockUserPointsRepo = new Mock<IUserPointsRepository>();
        mockUserPointsRepo.Setup(x => x.GetUserPoints(userId)).Returns(Task.FromResult(getTestData()));

        var actualResult = await new SquareService(mockUserPointsRepo.Object).GetSquare(userId);
        Assert.AreEqual(expectedResult.Count, actualResult.Count);

        int i = 0;
        foreach (var square in expectedResult.Square)
        {
            Assert.AreEqual(square, actualResult.Square[i]);
            i++;
        }
    }

    [TestMethod]
    public void GetSquareTimeTest()
    {
        int userId = 1;
        SquarePoint expectedResult = getExprectedResult();

        var mockUserPointsRepo = new Mock<IUserPointsRepository>();
        mockUserPointsRepo.Setup(x => x.GetUserPoints(userId)).Returns(Task.FromResult(get2KTestData()));
        var startTime = DateTime.Now;
        var actualResult = new SquareService(mockUserPointsRepo.Object).GetSquare(userId).Result;
        var endTime = DateTime.Now;

        Console.Write(actualResult.Count);
        var latency = endTime.Subtract(startTime).Seconds;
        Console.Write(latency);
        Assert.IsTrue(latency < 4);

    }

    private IEnumerable<UserPoint> get2KTestData()
    {
        List<UserPoint> userPoints = new List<UserPoint>();
        for (int i = 0; i < 2000; i++)
        {
            userPoints.Add(new UserPoint { X = i, Y = i });
        }

        return userPoints;
    }

    private SquarePoint getExprectedResult()
    {
        return new SquarePoint
        {
            Count = 4,
            Square = 
            new List<Square> {
                new Square { Points = new[]{
                    new UserPoint{X = 0, Y=0},
                    new UserPoint {X = 1, Y = 0},
                    new UserPoint {X = 0, Y = 1},
                    new UserPoint {X = 1, Y = 1},
                }},
                new Square{ Points = new[]{
                    new UserPoint{X = 0, Y=0},
                    new UserPoint {X = 2, Y = 0},
                    new UserPoint {X = 0, Y = 2},
                    new UserPoint {X = 2, Y = 2},
                }},
                new Square{ Points = new[]{
                    new UserPoint{X = 0, Y=0},
                    new UserPoint {X = 3, Y = 0},
                    new UserPoint {X = 0, Y = 3},
                    new UserPoint {X = 3, Y = 3},
                }},
                new Square{ Points = new[]{
                    new UserPoint{X = 1, Y=0},
                    new UserPoint {X = 2, Y = 0},
                    new UserPoint {X = 1, Y = 1},
                    new UserPoint {X = 2, Y = 1},
                }}
            }
        };
    }

    private IEnumerable<UserPoint> getTestData()
    {
        return new List<UserPoint> {
            new UserPoint {X = 0, Y = 0},
            new UserPoint {X = 1, Y = 0},
            new UserPoint {X = 2, Y = 0},
            new UserPoint {X = 3, Y = 0},
            new UserPoint {X = 4, Y = 0},
            new UserPoint {X = 0, Y = 1},
            new UserPoint {X = 0, Y = 2},
            new UserPoint {X = 0, Y = 3},
            new UserPoint {X = 0, Y = 4},
            new UserPoint {X = 1, Y = 1},
            new UserPoint {X = 2, Y = 1},
            new UserPoint {X = 2, Y = 2},
            new UserPoint {X = 3, Y = 3},
            new UserPoint {X = 3, Y = 4},
            new UserPoint {X = 0, Y = -2}
        };
        
    }
}