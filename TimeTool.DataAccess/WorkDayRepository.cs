//-----------------------------------------------------------------------
// <copyright file="WorkDayRepository.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
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
  /// Provides access to the actual databaseand abstracts it from the rest of the code.
  /// </summary>
  public class WorkdayRepository : IWorkdayRepository, IDisposable
  {
    /// <summary>
    /// The name of the work days collection in the database.
    /// </summary>
    private const string CollectionName = "Workday";

    /// <summary>
    /// The database instance that is managed by this class.
    /// </summary>
    private readonly LiteDatabase database;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkdayRepository"/> class.
    /// </summary>
    /// <param name="databaseLocation">Contains the fully qualified location and name of the database file.</param>
    /// <exception cref="ArgumentException">The specified <paramref name="databaseLocation"/> value was one of the following values:
    ///   <list type="bullet">
    ///     <item><see langword="null" /></item>
    ///     <item><see langword="string.Empty"/></item>
    ///     <item>Consists of only whitespaces</item>
    ///   </list>
    /// </exception>
    public WorkdayRepository(string databaseLocation)
    {
      if (string.IsNullOrWhiteSpace(databaseLocation))
      {
        throw new ArgumentException("Argument must not be NULL, empty, or consist of only whitespaces.", nameof(databaseLocation));
      }

      this.database = new LiteDatabase(databaseLocation);
    }

    /// <summary>
    /// Gets a collection of all available <see cref="IWorkdayInfo"/> instances.
    /// </summary>
    private LiteCollection<Workday> Days => this.database.GetCollection<Workday>(CollectionName);

    /// <inheritdoc />
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
    /// Returns all days of the underlying collection.
    /// </summary>
    /// <returns>Returns all avalaible day objects.</returns>
    public Workday[] GetDays()
    {
      // TODO: Refine this for just a single month/ year.
      var workdays = this.Days;

      return workdays.FindAll()
                     .ToArray();
    }

    /// <summary>
    /// Adds a new <see cref="IWorkdayInfo"/> object into the database.
    /// </summary>
    /// <param name="days">Contais a collection of <see cref="Workday"/> instances that should be saved.</param>
    public void Insert(IEnumerable<Workday> days)
    {
      this.Days.InsertBulk(days);
    }

    /// <summary>
    /// Persists the specifed <see cref="IWorkdayInfo"/> instance into the database.
    /// </summary>
    /// <param name="day">The current workday instance.</param>
    public void Update(IWorkdayInfo day)
    {
      var targetDay = this.Days.FindOne(d => d.WorkdayId == day.WorkdayId);

      MapValues(day, targetDay);

      this.Days.Update(targetDay);
    }

    /// <summary>
    /// Persists the specifed <see cref="IWorkdayInfo"/> instances into the database in a bulk operation.
    /// </summary>
    /// <param name="days">Contains a collection of <see cref="IWorkdayInfo"/> instances that should be saved.</param>
    public void Update(IEnumerable<IWorkdayInfo> days)
    {
      var workdays = CreateConcreteInstances(days);
      this.Days.Update(workdays);
    }

    /// <summary>
    /// Maps the objects that are referenced by interfaces into objects with a concrete type.
    /// The underlying database needs concrete types to work correctly.
    /// </summary>
    /// <param name="sourceDays">The collection of days that should be saved.</param>
    /// <returns>Returns a collection of concrete typed objects with the same values as the specified <paramref name="sourceDays"/>.</returns>
    private static IEnumerable<Workday> CreateConcreteInstances(IEnumerable<IWorkdayInfo> sourceDays)
    {
      // TODO: Evaluate if this step is really neccessary?
      var days = sourceDays.ToArray();

      foreach (var source in days)
      {
        var target = new Workday();
        MapValues(source, target);

        yield return target;
      }
    }

    /// <summary>
    /// Assigns the values of the source object to the corresponding properties of the target object.
    /// </summary>
    /// <param name="source">Contains the values that will be assigned to the target object.</param>
    /// <param name="target">Receives the values of the source object.</param>
    private static void MapValues(IWorkdayInfo source, Workday target)
    {
      if (source.WorkdayId > 0)
      {
        target.WorkdayId = source.WorkdayId;
      }

      target.StartTime = source.StartTime;
      target.EndTime = source.EndTime;
      target.DefaultWorkLength = source.DefaultWorkLength;
      target.TotalBreakLength = source.TotalBreakLength;
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
        if (this.database != null)
        {
          this.database.Dispose();
        }
      }

      // release native ressources here, if necessary.
    }
  }
}