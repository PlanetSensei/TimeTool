//-----------------------------------------------------------------------
// <copyright file="IWorkDayInfo.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Contracts
{
  using System;

  /// <summary>
  /// Provides a public interface for an object that represents a single work day time info.
  /// </summary>
  public interface IWorkdayInfo
  {
    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    TimeSpan DailyWorkLength { get; set; }

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the total combined time of breaks during this day.
    /// </summary>
    TimeSpan TotalBreakLength { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the current work day.
    /// </summary>
    int WorkdayId { get; set; }
  }
}