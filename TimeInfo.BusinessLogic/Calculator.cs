//-----------------------------------------------------------------------
// <copyright file="Calculator.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.BusinessLogic
{
  using System;

  /// <summary>
  /// Processes the work times.
  /// </summary>
  public static class Calculator
  {
    /// <summary>
    /// Calculates the difference from start time to current time.
    /// </summary>
    /// <param name="startTime">Contains date and time of the point in time when work started.</param>
    /// <param name="dailyWorkLength">Contains the pure length of the work day.</param>
    /// <param name="totalBreakLength">Contains the total sum of breaks during the work day.</param>
    /// <param name="currentTime">Defines the current time against which the work day time will be measured.</param>
    /// <returns>Returns a delta time that shows how long has already worked today.</returns>
    public static TimeSpan GetDeltaTime(DateTime startTime, TimeSpan dailyWorkLength, TimeSpan totalBreakLength, DateTime currentTime)
    {
      var targetTime = GetTargetTime(startTime, dailyWorkLength, totalBreakLength);
      var deltaTime = (targetTime - currentTime).Duration();

      if (targetTime > currentTime)
      {
        // In this case we're still working undertime
        deltaTime = deltaTime.Negate();
      }

      return deltaTime;
    }

    /// <summary>
    /// Calculates the time when the work day work will be over.
    /// </summary>
    /// <param name="startTime">Contains date and time of the point in time when work started.</param>
    /// <param name="dailyWorkLength">Contains the pure length of the work day.</param>
    /// <param name="totalBreakLength">Contains the total sum of breaks during the work day.</param>
    /// <returns>Returns the time time when the expected work length is finished.</returns>
    public static DateTime GetTargetTime(DateTime startTime, TimeSpan dailyWorkLength, TimeSpan totalBreakLength)
    {
      var targetTime = startTime.Add(dailyWorkLength)
                                .Add(totalBreakLength);

      return targetTime;
    }
  }
}