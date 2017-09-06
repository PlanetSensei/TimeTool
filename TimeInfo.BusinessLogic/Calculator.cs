//-----------------------------------------------------------------------
// <copyright file="Calculator.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.BusinessLogic
{
  using System;

  using TimeTool.Contracts;

  /// <summary>
  /// Processes the work times.
  /// </summary>
  public static class Calculator
  {
    /// <summary>
    /// Calculates the difference from start time to current time.
    /// </summary>
    /// <param name="workDay">Contains the time information of a single work day.</param>
    /// <param name="currentTime">Defines the current time against which the work day time will be measured.</param>
    /// <returns>Returns a delta time that shows how long has already worked today.</returns>
    public static TimeSpan GetDeltaTime(IWorkDayInfo workDay, DateTime currentTime)
    {
      if (workDay == null)
      {
        throw new ArgumentNullException(nameof(workDay));
      }

      var targetTime = GetTargetTime(workDay);
      var deltaTime = (targetTime - currentTime).Duration();

      if (targetTime > currentTime)
      {
        // In this case we're still working undertime
        deltaTime = deltaTime.Negate();
      }

      return deltaTime;
    }

    /// <summary>
    /// Calculates the time when the <paramref name="workDay"/> work will be over.
    /// </summary>
    /// <param name="workDay">Contains the time information of a single work day.</param>
    /// <returns>Returns the time time when the expected work length is finished.</returns>
    public static DateTime GetTargetTime(IWorkDayInfo workDay)
    {
      if (workDay == null)
      {
        throw new ArgumentNullException(nameof(workDay));
      }

      var targetTime = workDay.StartTime.Add(workDay.DailyWorkLength)
                              .Add(workDay.TotalBreakLength);

      return targetTime;
    }
  }
}