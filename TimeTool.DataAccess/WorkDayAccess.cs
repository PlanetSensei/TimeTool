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

  using TimeTool.Contracts;

  /// <summary>
  /// Provides access to work day information in the database.
  /// </summary>
  public class WorkDayAccess : IWorkDayAccess, IDisposable
  {
    /// <summary>
    /// The database instance that is managed by this class.
    /// </summary>
    private readonly WorkDayRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkDayAccess"/> class.
    /// </summary>
    /// <param name="databaseLocation">Contains the fully qualified location and name of the database file.</param>
    public WorkDayAccess(string databaseLocation)
    {
      if (string.IsNullOrWhiteSpace(databaseLocation))
      {
        throw new ArgumentException(nameof(databaseLocation));
      }

      this.repository = new WorkDayRepository(databaseLocation);
    }

    /// <inheritdoc />
    public IEnumerable<IWorkDayInfo> CreateMonth(int year, int month, TimeSpan dailyLength, TimeSpan breakLength)
    {
      var workDays = this.repository.GetDays();

      // Create empty entries for current month
      var daysInMonth = DateTime.DaysInMonth(year, month);
      var availableDays = new WorkDay[daysInMonth];

      for (int dayOfMonth = 1; dayOfMonth <= daysInMonth; dayOfMonth++)
      {
        var currentDay = new DateTime(year, month, dayOfMonth);
        var workDay = workDays.Single(d => d.Date == currentDay.Date);

        if (workDay == null)
        {
          workDay = new WorkDay
                      {
                        Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayOfMonth),
                        DailyWorkLength = dailyLength,
                        TotalBreakLength = breakLength
                      };

          // Remember that the technical index is 0-based.
          availableDays[dayOfMonth - 1] = workDay;
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
    public IEnumerable<IWorkDayInfo> GetDays(int year, int month)
    {
      var workDays = this.repository.GetDays();

      var monthstart = new DateTime(year, month, 1);
      var nextMonth = new DateTime(year, month + 1, 1);
      var daysInMonth = workDays.Where(day => day.Date >= monthstart && day.Date < nextMonth);

      foreach (var workDay in daysInMonth)
      {
        var info = new WorkDay
                     {
                       DailyWorkLength = workDay.DailyWorkLength,
                       Date = workDay.Date,
                       StartTime = workDay.StartTime,
                       TotalBreakLength = workDay.TotalBreakLength,
                       WorkDayId = workDay.WorkDayId
                     };

        yield return info;
      }
    }

    /// <summary>
    /// Saves the values of the specified work day instance.
    /// </summary>
    /// <param name="workDay">The current workday instance.</param>
    public void Save(IWorkDayInfo workDay)
    {
      this.repository.Update(workDay);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="isDisposing">
    /// Specifies whether this method is called by an explicit call to Dispose,
    /// or it is called by the garbage collector.<c> true </c>if this method is called from Dispose,
    /// otherwise<c> false </c>if called by the destructor of <see cref="WorkDayAccess"/>.
    /// </param>
    /// <filterpriority>2</filterpriority>
    private void Dispose(bool isDisposing)
    {
      if (isDisposing)
      {
        this.repository?.Dispose();
      }

      // release native ressources here, if necessary.
    }
  }
}