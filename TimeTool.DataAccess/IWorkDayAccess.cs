//-----------------------------------------------------------------------
// <copyright file="IWorkDayAccess.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System;
  using System.Collections.Generic;

  using TimeTool.Contracts;

  /// <summary>
  /// Provides an interface to manage work day information.
  /// </summary>
  public interface IWorkDayAccess
  {
    /// <summary>
    /// Creates a collection of work day objects that represent a
    /// specific month and persists it into the underlying data source.
    /// </summary>
    /// <param name="year">The year in which the data will be created.</param>
    /// <param name="month">The month in which the data will be created.</param>
    /// <param name="dailyLength">The default work length per day that will be set for each day during creation.</param>
    /// <param name="breakLength">The default break length per day length that will be set for each day during creation.</param>
    /// <returns>Returns a collection of the created work day objects.</returns>
    IEnumerable<IWorkDayInfo> CreateMonth(int year, int month, TimeSpan dailyLength, TimeSpan breakLength);

    /// <summary>
    /// Fetches a collection of all entries of the specified month.
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="month">The number of the month from 1 to 12.
    /// Uses the same index as the <see cref="DateTime"/> type.</param>
    /// <returns>Return a collection of found work day objects.</returns>
    IEnumerable<IWorkDayInfo> GetDays(int year, int month);

    /// <summary>
    /// Saves the values of the specified work day instance.
    /// </summary>
    /// <param name="workDay">The current workday instance.</param>
    void Save(IWorkDayInfo workDay);
  }
}