//-----------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.BusinessLogic
{
  using System;

  /// <summary>
  /// Provides information about the current user.
  /// </summary>
  public static class UserInfo
  {
    /// <summary>
    /// Retrieves the (local) time when the user logged into this Windows machine for the last time.
    /// Default value is <see cref="DateTime.Now"/> if the actual value could not be determined.
    /// </summary>
    /// <param name="day">Defines the specific day from which the data will be looked up.</param>
    /// <returns>Returns latest logon time in LOCAL time.</returns>
    public static DateTime GetLastLogOnToMachine(DateTime day)
    {
      var logOn = EventLogReader.GetLogOn(day);
      return logOn;
    }
    
    /// <summary>
    /// Retrieves the (local) time when the user logged into this Windows machine for the last time.
    /// Default value is <see cref="DateTime.Now"/> if the actual value could not be determined.
    /// </summary>
    /// <param name="day">Defines the specific day from which the data will be looked up.</param>
    /// <returns>Returns latest logon time in LOCAL time.</returns>
    public static DateTime GetLastLogOffFromMachine(DateTime day)
    {
      var logOn = EventLogReader.GetLogOn(day);
      return logOn;
    }
  }
}