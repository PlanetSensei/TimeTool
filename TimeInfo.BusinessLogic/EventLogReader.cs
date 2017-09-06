//-----------------------------------------------------------------------
// <copyright file="EventLogReader.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.BusinessLogic
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  /// <summary>
  /// Provides access to information within the windows event logs.
  /// </summary>
  internal static class EventLogReader
  {
    /// <summary>
    /// Represents the ID of the WinLogOn event that we are looking for in the SECURITY event log.
    /// </summary>
    private const long WinLogIdSecurity = 4624;

    /// <summary>
    /// Represents the ID of the WinLogOn event that we are looking for in the SYSTEM event log.
    /// </summary>
    private const long WinLogIdSystem = 7001;

    /// <summary>
    /// The actual name of SECURITY event log.
    /// </summary>
    private const string EventLogSecurity = "Security";

    /// <summary>
    /// The actual name of the SYSTEM event log.
    /// </summary>
    private const string EventLogSystem = "System";

    /////// <summary>
    /////// Reads the first WinLogOn event of the current day from the event log.
    /////// </summary>
    /////// <returns>Returns the first LoOn timestamp of the local machine in LOCAL time.</returns>
    ////internal static DateTime GetLogOn(string logName, int eventId)
    ////{
    ////  var todayMidnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

    ////  EventLog eventLog = new EventLog(logName);
    ////  IList<EventLogEntry> entries = new List<EventLogEntry>();

    ////  for (var i = eventLog.Entries.Count - 1; eventLog.Entries[i].TimeWritten > todayMidnight; i--)
    ////  {
    ////    var entry = eventLog.Entries[i];

    ////    if (entry.InstanceId == eventId)
    ////    {
    ////      entries.Add(eventLog.Entries[i]);
    ////    }
    ////  }

    ////  DateTime firstLogon = entries.Count > 0 ? entries.Min(e => e.TimeGenerated) : DateTime.UtcNow;

    ////  var localTime = firstLogon.ToLocalTime();
    ////  return localTime;
    ////}

    /// <summary>
    /// Reads the first WinLogOn event of the current day from the event log.
    /// </summary>
    /// <returns>Returns the first LoOn timestamp of the local machine in LOCAL time.</returns>
    internal static DateTime GetLogOn()
    {
      var entries = GetLogOnEntries(EventLogSystem, WinLogIdSystem);

      // The easy solution save for later
      ////var firstLogon = entries.Count > 0 ? entries.Min(e => e.TimeGenerated) : DateTime.UtcNow;

      if (entries.Count <= 0)
      {
        // Less accurate, but on windows 10 there is no return value from the Securityevent log.
        entries = GetLogOnEntries(EventLogSecurity, WinLogIdSecurity);
      }

      var firstLogon = entries.Count > 0 ? entries.Min(e => e.TimeGenerated) : DateTime.UtcNow;

      var localTime = firstLogon.ToLocalTime();
      return localTime;
    }

    private static IList<EventLogEntry> GetLogOnEntries(string logName, long eventId)
    {
      var todayMidnight = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

      EventLog eventLog = new EventLog(logName);
      IList<EventLogEntry> entries = new List<EventLogEntry>();

      for (var i = eventLog.Entries.Count - 1; eventLog.Entries[i].TimeWritten > todayMidnight; i--)
      {
        var entry = eventLog.Entries[i];

        if (entry.InstanceId == eventId)
        {
          entries.Add(eventLog.Entries[i]);
        }
      }

      return entries;
    }
  }
}
