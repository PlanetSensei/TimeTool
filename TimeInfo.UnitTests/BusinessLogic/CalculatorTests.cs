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

  using Moq;

  using TimeTool.BusinessLogic;
  using TimeTool.Contracts;
  using TimeTool.UnitTests.Properties;

  /// <summary>
  /// This is a test class for Calculator and is intended to contain all relevant unit tests.
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

      var workDayMock = new Mock<IWorkDayInfo>();
      workDayMock.SetupProperty(prop => prop.StartTime, new DateTime(2017, 8, 15, 9, 0, 0));
      workDayMock.SetupProperty(prop => prop.DailyWorkLength, new TimeSpan(7, 42, 0));
      workDayMock.SetupProperty(prop => prop.TotalBreakLength, new TimeSpan(0, 30, 0));

      // Act
      TimeSpan actual = Calculator.GetDeltaTime(workDayMock.Object, currentTime);

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

      var workDayMock = new Mock<IWorkDayInfo>();
      workDayMock.SetupProperty(prop => prop.StartTime, new DateTime(2017, 8, 15, 9, 0, 0));
      workDayMock.SetupProperty(prop => prop.DailyWorkLength, new TimeSpan(7, 42, 0));
      workDayMock.SetupProperty(prop => prop.TotalBreakLength, new TimeSpan(0, 30, 0));

      // Act
      TimeSpan actual = Calculator.GetDeltaTime(workDayMock.Object, currentTime);

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

      var workDayMock = new Mock<IWorkDayInfo>();
      workDayMock.SetupProperty(prop => prop.StartTime, new DateTime(2017, 8, 15, 9, 0, 0));
      workDayMock.SetupProperty(prop => prop.DailyWorkLength, new TimeSpan(7, 42, 0));
      workDayMock.SetupProperty(prop => prop.TotalBreakLength, new TimeSpan(0, 30, 0));

      // Act
      TimeSpan actual = Calculator.GetDeltaTime(workDayMock.Object, currentTime);

      // Assert
      Assert.AreEqual(expectedDelta, actual);
    }

    /// <summary>
    /// Test for method GetTargetTime().
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetDeltaTimeUsingNullReferenceThrowsException()
    {
      // Arrange
      // n/a

      // Act
      Calculator.GetDeltaTime(null, DateTime.MinValue);

      // Assert
      Assert.Fail(Resources.ThisPointShoulNotBeReached);
    }

    /// <summary>
    /// Test for method GetTargetTime().
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetTargetTimeUsingNullReferenceThrowsException()
    {
      // Arrange
      // n/a

      // Act
      Calculator.GetTargetTime(null);

      // Assert
      Assert.Fail(Resources.ThisPointShoulNotBeReached);
    }

    /// <summary>
    /// Test for method GetTargetTime().
    /// </summary>
    [TestMethod]
    public void GetTargetTimeUsingValidInputReturnsTargetTime()
    {
      // Arrange
      DateTime expectedTime = new DateTime(2017, 08, 15, 17, 45, 0);
      var workDayMock = new Mock<IWorkDayInfo>();
      workDayMock.SetupProperty(prop => prop.StartTime, new DateTime(2017, 08, 15, 9, 33, 0));
      workDayMock.SetupProperty(prop => prop.DailyWorkLength, new TimeSpan(7, 42, 0));
      workDayMock.SetupProperty(prop => prop.TotalBreakLength, new TimeSpan(0, 30, 0));

      // Act
      var actual = Calculator.GetTargetTime(workDayMock.Object);

      // Assert
      Assert.AreEqual(expectedTime, actual);
    }
  }
}