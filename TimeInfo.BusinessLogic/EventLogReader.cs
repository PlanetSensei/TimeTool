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
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;

  /// <summary>
  /// Provides access to information within the windows event logs.
  /// </summary>
  [ExcludeFromCodeCoverage]
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
    /// Represents the ID of the event that is fired at the begin of the log off.
    /// </summary>
    private const long BeginLogOff = 4647;

    // 512 / 4608  STARTUP
    //  * 513 / 4609  SHUTDOWN
    //  * 528 / 4624  LOGON
    //  * 538 / 4634  LOGOFF
    //  * 551 / 4647  BEGIN_LOGOFF <= Sieht am vielversprechendsten aus.Beachte Sicherheits-ID garbsen1\[Benutzername] oder Kontoname = [Benutzernamen]
    //  * N/A / 4778  SESSION_RECONNECTED
    //  * N/A / 4779  SESSION_DISCONNECTED
    //  * N/A / 4800  WORKSTATION_LOCKED
    //  * 4801    WORKSTATION_UNLOCKED
    //  * N/A / 4802  SCREENSAVER_INVOKED
    //  * N/A / 4803  SCREENSAVER_DISMISSED

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
    /// <param name="day">Defines the specific day from which the data will be looked up.</param>
    /// <returns>Returns the first LoOn timestamp of the local machine in LOCAL time.</returns>
    internal static DateTime GetLogOn(DateTime day)
    {
      var entries = GetLogOnEntries(EventLogSystem, WinLogIdSystem, day);

      if (entries.Count <= 0)
      {
        // Less accurate, but on windows 10 there is no return value from the Securityevent log.
        entries = GetLogOnEntries(EventLogSecurity, WinLogIdSecurity, day);
      }

      var firstLogon = entries.Count > 0 ? entries.Min(e => e.TimeGenerated) : DateTime.UtcNow;

      var localTime = firstLogon.ToLocalTime();
      return localTime;
    }

    /// <summary>
    /// Tries to find the time when the user stopped working on the specified <paramref name="day" />.
    /// </summary>
    /// <param name="day">The day for which the inforation is gathered.</param>
    /// <returns>Returns the found most appropriate work end of the day.</returns>
    /// <remarks>
    /// <para>
    /// Tracking User Logon Activity Using Logon Events
    ///   https://blogs.msdn.microsoft.com/ericfitz/2008/08/20/tracking-user-logon-activity-using-logon-events/
    /// </para>
    /// <list type="table">
    ///   <listheader>
    ///     <term>Code</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <term>512 / 4608</term>
    ///     <description>STARTUP</description>
    ///   </item>
    ///   <item>
    ///     <term>513 / 4609</term>
    ///     <description>SHUTDOWN</description>
    ///   </item>
    ///   <item>
    ///     <term>528 / 4624</term>
    ///     <description>LOGON</description>
    ///   </item>
    ///   <item>
    ///     <term>538 / 4634</term>
    ///     <description>LOGOFF</description>
    ///   </item>
    ///   <item>
    ///     <term>551 / 4647</term>
    ///     <description>BEGIN_LOGOFF &lt;= Sieht am vielversprechendsten aus. Beachte Sicherheits-ID garbsen1\[Benutzername] oder Kontoname = [Benutzernamen]</description>
    ///   </item>
    ///   <item>
    ///     <term>N/A / 4778</term>
    ///     <description>SESSION_RECONNECTED</description>
    ///   </item>
    ///   <item>
    ///     <term>N/A / 4779</term>
    ///     <description>SESSION_DISCONNECTED</description>
    ///   </item>
    ///   <item>
    ///     <term>N/A / 4800</term>
    ///     <description>WORKSTATION_LOCKED</description>
    ///   </item>
    ///   <item>
    ///     <term>4801</term>
    ///     <description>WORKSTATION_UNLOCKED</description>
    ///   </item>
    ///   <item>
    ///     <term>N/A / 4802</term>
    ///     <description>SCREENSAVER_INVOKED</description>
    ///   </item>
    ///   <item>
    ///     <term>N/A / 4803</term>
    ///     <description>SCREENSAVER_DISMISSED</description>
    ///   </item>
    /// </list>
    /// </remarks>
    internal static DateTime GetLogOff(DateTime day)
    {
      // Not totally accurate. But at the moment the best I can do.
      IList<EventLogEntry> entries = GetLogOffFromEventLog(EventLogSecurity, BeginLogOff, day);

      var lastLogoff = entries.Count > 0 ? entries.Max(e => e.TimeGenerated) : day;

      var localTime = lastLogoff.ToLocalTime();
      return localTime;
    }

    /// <summary>
    /// Gets all EventLog entries with the specified <paramref name="eventId"/>.
    /// </summary>
    /// <param name="logName">The name of the EventLog from which the entries will be collected.</param>
    /// <param name="eventId">The ID of the events to look for.</param>
    /// <param name="day">Defines the start of the time frame within the events will be searched.</param>
    /// <returns>Returns a collection of found events.</returns>
    private static IList<EventLogEntry> GetLogOnEntries(string logName, long eventId, DateTime day)
    {
      using (EventLog eventLog = new EventLog(logName))
      {
        IList<EventLogEntry> entries = new List<EventLogEntry>();
        var nextDay = day.Date.AddDays(1);

        // TODO: Check a time RANGE instead
        for (var i = eventLog.Entries.Count - 1; eventLog.Entries[i].TimeWritten > day && eventLog.Entries[i].TimeWritten < nextDay; i--)
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

    private static IList<EventLogEntry> GetLogOffFromEventLog(string logName, long eventId, DateTime day)
    {
      using (EventLog eventLog = new EventLog(logName))
      {
        IList<EventLogEntry> entries = new List<EventLogEntry>();
        var nextDay = day.Date.AddDays(1);

        // TODO: Check a time RANGE instead
        for (var i = eventLog.Entries.Count - 1; eventLog.Entries[i].TimeWritten > day; i--)
        {
          var entry = eventLog.Entries[i];
          
          if (entry.InstanceId != eventId)
          {
            continue;
          }

          if (!(entry.TimeWritten < nextDay))
          {
            continue;
          }

          entries.Add(eventLog.Entries[i]);
        }

        return entries;
      }
    }
  }
}
