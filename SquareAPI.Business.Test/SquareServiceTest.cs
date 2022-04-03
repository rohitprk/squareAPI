using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        SquarePoint expectedResult = GetExpectedResult();

        var mockUserPointsRepo = new Mock<IUserPointsRepository>();
        mockUserPointsRepo.Setup(x => x.GetUserPoints(userId)).Returns(Task.FromResult(GetTestData()));

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
    public async Task GetPointsTest()
    {
        var csvData = GetTestPointsCSV();
        var encodedData = Encoding.UTF8.GetBytes(csvData);
        using (MemoryStream ms = new MemoryStream(encodedData))
        {
            using (StreamReader sr = new StreamReader(ms))
            {
                var mockUserPointsRepo = new Mock<IUserPointsRepository>();
                var result = new SquareService(mockUserPointsRepo.Object).GetPoints(sr);

                Assert.IsTrue(result.Count() == 15);
                int i = 0;
                foreach (var point in GetTestData())
                {
                    Assert.AreEqual(point, result.ElementAt(i));
                    i++;
                }
            }
        }

    }

    private SquarePoint GetExpectedResult()
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

    private IEnumerable<UserPoint> GetTestData()
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

    private string GetTestPointsCSV()
    {
        StringBuilder builder = new StringBuilder("");
        builder.AppendLine("X,Y");
        foreach (var point in GetTestData())
        {
            builder.AppendLine(point.GetCSV());
        }

        return builder.ToString();
    }
}