//-----------------------------------------------------------------------
// <copyright file="WorkDay.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Contracts
{
  using System;

  /// <summary>
  /// Represents a single work day without the calculated time values.
  /// </summary>
  public class Workday : IWorkdayInfo
  {
    /// <summary>
    /// Gets or sets the expected length of the current work day.
    /// </summary>
    public TimeSpan DefaultWorkLength { get; set; }

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the total of all breaks during the current work day.
    /// </summary>
    public TimeSpan TotalBreakLength { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the current work day.
    /// </summary>
    public int WorkdayId { get; set; }
  }
}
