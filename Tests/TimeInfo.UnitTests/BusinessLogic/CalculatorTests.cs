//-----------------------------------------------------------------------
// <copyright file="CalculatorTests.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.UnitTests.BusinessLogic
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using TimeTool.BusinessLogic;
  using TimeTool.Contracts;

  /// <summary>
  /// This is a test class for DateCalculator and is intended to contain all relevant unit tests.
  /// </summary>
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class CalculatorTests
  {
    /// <summary>
    /// Test for method GetDeltaTime().
    /// </summary>
    [TestMethod]
    public void GetDeltaTimeUsingValidInputReturnsExaktZeroTime()
    {
      // Arrange
      DateTime currentTime = new DateTime(2017, 8, 15, 17, 12, 0);
      TimeSpan expectedDelta = new TimeSpan(0, 0, 0);

      var workDay = new Workday
      {
        StartTime = new DateTime(2017, 8, 15, 9, 0, 0),
        DefaultWorkLength = new TimeSpan(7, 42, 0),
        TotalBreakLength = new TimeSpan(0, 30, 0)
      };

      // Act
      TimeSpan actual = DateCalculator.GetDeltaTime(workDay, currentTime);

      // Assert
      Assert.AreEqual(expectedDelta, actual);
    }

    /// <summary>
    /// Test for method GetDeltaTime().
    /// </summary>
    [TestMethod]
    public void GetDeltaTimeUsingValidInputReturnsOvertime()
    {
      // Arrange
      DateTime currentTime = new DateTime(2017, 8, 15, 18, 12, 0);
      TimeSpan expectedDelta = new TimeSpan(1, 0, 0);

      var workDay = new Workday
      {
        StartTime = new DateTime(2017, 8, 15, 9, 0, 0),
        DefaultWorkLength = new TimeSpan(7, 42, 0),
        TotalBreakLength = new TimeSpan(0, 30, 0)
      };

      // Act
      TimeSpan actual = DateCalculator.GetDeltaTime(workDay, currentTime);

      // Assert
      Assert.AreEqual(expectedDelta, actual);
    }

    /// <summary>
    /// Test for method GetDeltaTime().
    /// </summary>
    [TestMethod]
    public void GetDeltaTimeUsingValidInputReturnsUndertime()
    {
      // Arrange
      DateTime currentTime = new DateTime(2017, 8, 15, 16, 12, 0);
      TimeSpan expectedDelta = new TimeSpan(-1, 0, 0);

      var workDay = new Workday
      {
        StartTime = new DateTime(2017, 8, 15, 9, 0, 0),
        DefaultWorkLength = new TimeSpan(7, 42, 0),
        TotalBreakLength = new TimeSpan(0, 30, 0)
      };

      // Act
      TimeSpan actual = DateCalculator.GetDeltaTime(workDay, currentTime);

      // Assert
      Assert.AreEqual(expectedDelta, actual);
    }

    /// <summary>
    /// Test for method GetTargetTime().
    /// </summary>
    [TestMethod]
    public void GetTargetTimeUsingValidInputReturnsTargetTime()
    {
      // Arrange
      DateTime expectedTime = new DateTime(2017, 08, 15, 17, 45, 0);

      var startTime = new DateTime(2017, 08, 15, 9, 33, 0);
      var dailyWorkLength = new TimeSpan(7, 42, 0);
      var totalBreakLength = new TimeSpan(0, 30, 0);

      // Act
      var actual = DateCalculator.GetTargetTime(startTime, dailyWorkLength, totalBreakLength);

      // Assert
      Assert.AreEqual(expectedTime, actual);
    }
  }
}