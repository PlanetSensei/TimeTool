//-----------------------------------------------------------------------
// <copyright file="DatesTest.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.UnitTests.DataAccess
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using TimeTool.DataAccess;

  /// <summary>
  /// This is a test class for Dates and is intended to contain all relevant unit tests.
  /// </summary>
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class DatesTest
  {
    /// <summary>
    /// Test for method GetFirstOfMonth().
    /// </summary>
    [TestMethod]
    public void GetFirstOfMonthUsingValidValuesExpectedNewInstance()
    {
      // Arrange
      const int ExpectedYear = 2017;
      const int ExpectedMonth = 12;
      const int ExpectedDay = 1;
      const int ExpectedHour = 0;
      const int ExpectedMinute = 0;

      // Act
      DateTime actual = Dates.GetFirstOfMonth(2017, 12);

      // Assert
      Assert.AreEqual(ExpectedYear, actual.Year);
      Assert.AreEqual(ExpectedMonth, actual.Month);
      Assert.AreEqual(ExpectedDay, actual.Day);
      Assert.AreEqual(ExpectedHour, actual.Hour);
      Assert.AreEqual(ExpectedMinute, actual.Minute);
    }
  }
}