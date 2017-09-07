//-----------------------------------------------------------------------
// <copyright file="WorkDayAccess.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using LiteDB;

  using TimeTool.Contracts;

  /// <summary>
  /// Provides access to work day information in the database.
  /// </summary>
  public class WorkDayAccess
  {
    /// <summary>
    /// Contains the fully qualified location and name of the database file.
    /// </summary>
    private readonly string databaseLocation;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkDayAccess"/> class.
    /// </summary>
    /// <param name="databaseLocation">Contains the fully qualified location and name of the database file.</param>
    public WorkDayAccess(string databaseLocation)
    {
      if (databaseLocation == null)
      {
        throw new ArgumentNullException(nameof(databaseLocation));
      }

      this.databaseLocation = databaseLocation;
    }

    public IEnumerable<IWorkDayInfo> CreateMonth(int year, int month, TimeSpan dailyLength, TimeSpan breakLength)
    {
      using (var database = new LiteDatabase(this.databaseLocation))
      {
        var workDays = database.GetCollection<WorkDay>("WorkDay");

        // Create empty entries for current month
        var daysInMonth = DateTime.DaysInMonth(year, month);

        for (int dayOfMonth = 1; dayOfMonth <= daysInMonth; dayOfMonth++)
        {
          var currentDay = new DateTime(year, month, dayOfMonth);
          var workDay = workDays.FindOne(d => d.Date == currentDay.Date);

          if (workDay == null)
          {
            workDay = new WorkDay
                        {
                          Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayOfMonth),
                          DailyWorkLength = dailyLength,
                          TotalBreakLength = breakLength
                        };

            workDays.Insert(workDay);
          }

          yield return workDay;
        }
      }
    }

    /// <summary>
    /// Fetches a collection of all entries of the specified month.
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="month">The number of the month from 1 to 12.
    /// Uses the same index as the <see cref="DateTime"/> type.</param>
    /// <returns>Return a collection of found work day objects.</returns>
    public IEnumerable<IWorkDayInfo> GetDays(int year, int month)
    {
      using (var database = new LiteDatabase(this.databaseLocation))
      {
        var workDays = database.GetCollection<WorkDay>("WorkDay");
        //database.DropCollection("WorkDay");

        var monthstart = new DateTime(year, month, 1);
        var nextMonth = new DateTime(year, month + 1, 1);
        var daysInMonth = workDays.Find(day => day.Date >= monthstart && day.Date < nextMonth);

        foreach (var workDay in daysInMonth)
        {
          var info = new WorkDay
                       {
                         DailyWorkLength = workDay.DailyWorkLength,
                         Date = workDay.Date,
                         TotalBreakLength = workDay.TotalBreakLength,
                         WorkDayId = workDay.WorkDayId
                       };

          yield return info;
        }
      }
    }

    /// <summary>
    /// Saves the values of the specified work day instance.
    /// </summary>
    /// <param name="workDay">The current workday instance.</param>
    public void Save(IWorkDayInfo workDay)
    {
      using (var database = new LiteDatabase(this.databaseLocation))
      {
        var workDays = database.GetCollection<WorkDay>("WorkDay");
        var targetDay = workDays.FindOne(d => d.WorkDayId == workDay.WorkDayId);

        MapValues(workDay, targetDay);

        var updateSuccess = workDays.Update(targetDay);
      }
    }

    /// <summary>
    /// Assigns the values of the source object to the corresponding properties of the target object.
    /// </summary>
    /// <param name="source">Contains the values that will be assigned to the target object.</param>
    /// <param name="target">Receives the values of the source object.</param>
    private static void MapValues(IWorkDayInfo source, WorkDay target)
    {
      target.StartTime = source.StartTime;
      target.DailyWorkLength = source.DailyWorkLength;
      target.Date = source.Date;
      target.TotalBreakLength = source.TotalBreakLength;
    }
  }
}