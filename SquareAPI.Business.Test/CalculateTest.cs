using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquareAPI.Business.Test;

[TestClass]
public class CalculateTest
{
    [TestMethod]
    public void GetSquareTest()
    {
        int userId = 1;
        IEnumerable<Point> testData = getTestData();
        SquarePoint expectedResult = getExprectedResult();
        var actualResult = new Calculate().GetSquare(userId, testData);
        Assert.AreEqual(expectedResult.UserId, actualResult.UserId);
        Assert.AreEqual(expectedResult.Count, actualResult.Count);

        int i = 0;
        foreach (var square in expectedResult.Square)
        {
            Assert.AreEqual(square, actualResult.Square[i]);
            i++;
        }
    }

    private SquarePoint getExprectedResult()
    {
        return new SquarePoint
        {
            UserId = 1,
            Count = 4,
            Square = 
            new List<Square> {
                new Square { Points = new[]{
                    new Point{X = 0, Y=0},
                    new Point {X = 1, Y = 0},
                    new Point {X = 0, Y = 1},
                    new Point {X = 1, Y = 1},
                }},
                new Square{ Points = new[]{
                    new Point{X = 0, Y=0},
                    new Point {X = 2, Y = 0},
                    new Point {X = 0, Y = 2},
                    new Point {X = 2, Y = 2},
                }},
                new Square{ Points = new[]{
                    new Point{X = 0, Y=0},
                    new Point {X = 3, Y = 0},
                    new Point {X = 0, Y = 3},
                    new Point {X = 3, Y = 3},
                }},
                new Square{ Points = new[]{
                    new Point{X = 1, Y=0},
                    new Point {X = 2, Y = 0},
                    new Point {X = 1, Y = 1},
                    new Point {X = 2, Y = 1},
                }}
            }
        };
    }

    private IEnumerable<Point> getTestData()
    {
        return new List<Point> {
            new Point {X = 0, Y = 0},
            new Point {X = 1, Y = 0},
            new Point {X = 2, Y = 0},
            new Point {X = 3, Y = 0},
            new Point {X = 4, Y = 0},
            new Point {X = 0, Y = 1},
            new Point {X = 0, Y = 2},
            new Point {X = 0, Y = 3},
            new Point {X = 0, Y = 4},
            new Point {X = 1, Y = 1},
            new Point {X = 2, Y = 1},
            new Point {X = 2, Y = 2},
            new Point {X = 3, Y = 3}
        };
    }
}