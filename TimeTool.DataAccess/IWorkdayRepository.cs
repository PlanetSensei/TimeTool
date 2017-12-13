//-----------------------------------------------------------------------
// <copyright file="IWorkdayRepository.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System.Collections.Generic;
  using Contracts;

  /// <summary>
  /// Provides a public interface for data access when handling <see cref="Workday"/> objects.
  /// </summary>
  public interface IWorkdayRepository
  {
    /// <summary>
    /// Returns all days of the underlying collection.
    /// </summary>
    /// <returns>Returns all avalaible day objects.</returns>
    Workday[] GetDays();

    /// <summary>
    /// Adds a new <see cref="IWorkdayInfo"/> object into the database.
    /// </summary>
    /// <param name="day">The object that will be saved.</param>
    void Insert(IEnumerable<Workday> day);

    /// <summary>
    /// Saves the values of the specified work day instance.
    /// </summary>
    /// <param name="day">The current workday instance.</param>
    void Update(IWorkdayInfo day);
  }
}