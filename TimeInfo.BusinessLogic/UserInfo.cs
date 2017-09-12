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
    /// <returns>Returns latest logon time in LOCAL time.</returns>
    public static DateTime GetLastLoginToMachine()
    {
      var logOn = EventLogReader.GetLogOn();
      return logOn;
    }
  }
}