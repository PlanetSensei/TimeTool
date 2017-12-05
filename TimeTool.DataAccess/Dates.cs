//-----------------------------------------------------------------------
// <copyright file="Dates.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System;

  /// <summary>
  /// Provides functionality for working with the <see cref="DateTime"/> type.
  /// </summary>
  internal static class Dates
  {
    /// <summary>
    /// Creates a new <see cref="DateTime"/> object that represents the first day of the specified <paramref name="month"/>
    /// of the specified <paramref name="year"/>.
    /// </summary>
    /// <param name="year">The year of the date object.</param>
    /// <param name="month">The month of the date object.</param>
    /// <returns>Returns a new <see cref="DateTime"/> object that represents the specified day, starting at midnight.</returns>
    internal static DateTime GetFirstOfMonth(int year, int month)
    {
      return new DateTime(year, month, 1);
    }
  }
}