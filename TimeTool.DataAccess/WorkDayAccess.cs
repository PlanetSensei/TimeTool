//-----------------------------------------------------------------------
// <copyright file="WorkDayAccess.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.DataAccess
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using TimeTool.Contracts;

  /// <summary>
  /// Provides access to work day information in the database.
  /// </summary>
  public class WorkdayAccess : IWorkdayAccess, IDisposable
  {
    /// <summary>
    /// The database instance that is managed by this class.
    /// </summary>
    private readonly WorkdayRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkdayAccess"/> class.
    /// </summary>
    /// <param name="databaseLocation">Contains the fully qualified location and name of the database file.</param>
    public WorkdayAccess(string databaseLocation)
    {
      if (string.IsNullOrWhiteSpace(databaseLocation))
      {
        throw new ArgumentException("Argument must not be NULL, empty, or consist of only whitespaces.", nameof(databaseLocation));
      }

      this.repository = new WorkdayRepository(databaseLocation);
    }

    /// <inheritdoc />
    public IEnumerable<IWorkdayInfo> CreateMonth(int year, int month, TimeSpan dailyLength, TimeSpan breakLength)
    {
      var workdays = this.repository.GetDays();

      // Create empty entries for current month
      var daysInMonth = DateTime.DaysInMonth(year, month);
      var availableDays = new Workday[daysInMonth];

      for (int dayOfMonth = 1; dayOfMonth <= daysInMonth; dayOfMonth++)
      {
        var currentDay = new DateTime(year, month, dayOfMonth);
        var workday = workdays.Single(d => d.Date == currentDay.Date);

        if (workday == null)
        {
          workday = new Workday
                      {
                        Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayOfMonth),
                        DailyWorkLength = dailyLength,
                        TotalBreakLength = breakLength
                      };

          // Remember that the technical index is 0-based.
          availableDays[dayOfMonth - 1] = workday;
        }
      }

      this.repository.Insert(availableDays);
      return availableDays;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Fetches a collection of all entries of the specified month.
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="month">The number of the month from 1 to 12.
    /// Uses the same index as the <see cref="DateTime"/> type.</param>
    /// <returns>Return a collection of found work day objects.</returns>
    public IEnumerable<IWorkdayInfo> GetDays(int year, int month)
    {
      var workdays = this.repository.GetDays();

      var monthstart = new DateTime(year, month, 1);
      var nextMonth = new DateTime(year, month + 1, 1);
      var daysInMonth = workdays.Where(day => day.Date >= monthstart && day.Date < nextMonth);

      foreach (var workday in daysInMonth)
      {
        var info = new Workday
                     {
                       DailyWorkLength = workday.DailyWorkLength,
                       Date = workday.Date,
                       StartTime = workday.StartTime,
                       TotalBreakLength = workday.TotalBreakLength,
                       WorkdayId = workday.WorkdayId
                     };

        yield return info;
      }
    }

    /// <summary>
    /// Saves the values of the specified work day instance.
    /// </summary>
    /// <param name="day">The current workday instance.</param>
    public void Save(IWorkdayInfo day)
    {
      this.repository.Update(day);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="isDisposing">
    /// Specifies whether this method is called by an explicit call to Dispose,
    /// or it is called by the garbage collector.<c> true </c>if this method is called from Dispose,
    /// otherwise<c> false </c>if called by the destructor of <see cref="WorkdayAccess"/>.
    /// </param>
    /// <filterpriority>2</filterpriority>
    private void Dispose(bool isDisposing)
    {
      if (isDisposing)
      {
        if (this.repository != null)
        {
          this.repository.Dispose();
        }
      }

      // release native ressources here, if necessary.
    }
  }
}