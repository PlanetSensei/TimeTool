//-----------------------------------------------------------------------
// <copyright file="WorkDayViewModel.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeInfo.TimeInfoUi.ViewModels
{
  using System;
  using GalaSoft.MvvmLight;
  using TimeInfo.BusinessLogic;
  using TimeInfo.Contracts;

  /// <summary>
  /// Represents a single day's work time information.
  /// </summary>
  public class WorkDayViewModel : ViewModelBase, IWorkDayInfo
  {
    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    private TimeSpan defaultDailyWorkLenth;

    /// <summary>
    /// Gets or sets the calculated difference time that shows the user whether he worked overtime or undertime.
    /// </summary>
    private TimeSpan remainingTime;

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    private DateTime startTime;

    /// <summary>
    /// Gets or sets the time when the expected work time is finished.
    /// </summary>
    private DateTime targetTime;

    /// <summary>
    /// Gets or sets the total combined time of breaks during this day.
    /// </summary>
    private TimeSpan totalBreak;

    // TODO: Berechne die Pausenzeit
    // TODO: Wenn Pausenzeit automatisch, dann kann sie aus den Settings entfernt werden
    // TODO: Ändere Startzeit in UserSettings und speichere sie beim Beenden der App
    // TODO: Implementiere NotifyIcon
    // TODO: Aktualisiere die Verbleibende Zeit auch bei Events vom TimeSpinner

    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    public TimeSpan DailyWorkLength
    {
      get
      {
        return this.defaultDailyWorkLenth;
      }

      set
      {
        this.Set(ref this.defaultDailyWorkLenth, value);
        this.UpdateTimeValues();
      }
    }

    /// <summary>
    /// Gets or sets the calculated difference time that shows the user whether he worked overtime or undertime.
    /// </summary>
    public TimeSpan RemainingTime
    {
      get
      {
        return this.remainingTime;
      }

      set
      {
        this.Set(ref this.remainingTime, value);
      }
    }

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    public DateTime StartTime
    {
      get
      {
        return this.startTime;
      }

      set
      {
        this.Set(ref this.startTime, value);
        this.UpdateTimeValues();
      }
    }

    /// <summary>
    /// Gets or sets the time when the expected work time is finished.
    /// </summary>
    public DateTime TargetTime
    {
      get
      {
        return this.targetTime;
      }

      set
      {
        this.Set(ref this.targetTime, value);
      }
    }

    /// <summary>
    /// Gets or sets the total combined time of breaks during this day.
    /// </summary>
    public TimeSpan TotalBreakLength
    {
      get
      {
        return this.totalBreak;
      }

      set
      {
        this.Set(ref this.totalBreak, value);
        this.UpdateTimeValues();
      }
    }

    /// <summary>
    /// Processes the current values to update the displayed values.
    /// </summary>
    private void UpdateTimeValues()
    {
      this.RemainingTime = Calculator.GetDeltaTime(this, DateTime.Now);
      this.TargetTime = Calculator.GetTargetTime(this);
    }
  }
}