//-----------------------------------------------------------------------
// <copyright file="IWorkDayInfo.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Contracts
{
  using System;

  /// <summary>
  /// Provides a public interface for an object that represents a single work day time info.
  /// </summary>
  public interface IWorkDayInfo
  {
    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    TimeSpan DailyWorkLength { get; set; }

    /// <summary>
    /// Gets or sets the delta time that shows whether the user worked overtime or undertime.
    /// </summary>
    TimeSpan RemainingTime { get; set; }

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the time when the expected work time is finished.
    /// </summary>
    DateTime TargetTime { get; set; }

    /// <summary>
    /// Gets or sets the total combined time of breaks during this day.
    /// </summary>
    TimeSpan TotalBreakLength { get; set; }
  }
}