//-----------------------------------------------------------------------
// <copyright file="SelectedDayChangedMessage.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Infrastructure
{
  using System;

  using Contracts;
  using ViewModels;

  /// <summary>
  /// Notifies subscribers that the specified <see cref="IWorkdayInfo"/> instance was selected in the UI.
  /// </summary>
  public class SelectedDayChangedMessage
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectedDayChangedMessage"/> class.
    /// </summary>
    /// <param name="day">The <see cref="IWorkdayInfo"/> instance that will be edited in the editor.</param>
    public SelectedDayChangedMessage(WorkdayViewModel day)
    {
      this.Day = day ?? throw new ArgumentNullException(nameof(day));
    }

    /// <summary>
    /// Gets the current <see cref="IWorkdayInfo"/> instance.
    /// </summary>
    public WorkdayViewModel Day { get; }
  }
}