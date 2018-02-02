//-----------------------------------------------------------------------
// <copyright file="WorkDayViewModel.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.ViewModels
{
  using System;

  using GalaSoft.MvvmLight;

  using TimeTool.BusinessLogic;
  using TimeTool.Contracts;

  /// <summary>
  /// Represents a single day's work time information.
  /// </summary>
  public class WorkdayViewModel : ViewModelBase, IWorkdayInfo
  {
    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    private TimeSpan defaulWorkLength;

    /// <summary>
    /// Gets or sets the time when the user finished work on this day.
    /// </summary>
    private DateTime endTime;

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

    /// <summary>
    /// Gets or sets the expected length of work on this day.
    /// </summary>
    public TimeSpan DefaultWorkLength
    {
      get
      {
        return this.defaulWorkLength;
      }

      set
      {
        this.Set(ref this.defaulWorkLength, value);
        this.UpdateTimeValues();
      }
    }

    /// <summary>
    /// Gets or sets the time when the user finished work on this day.
    /// </summary>
    public DateTime EndTime
    {
      get
      {
        return this.endTime;
      }

      set
      {
        this.endTime = value;
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
    /// Gets or sets the unique identifier of the current work day.
    /// </summary>
    public int WorkdayId { get; set; }

    /// <summary>
    /// Processes the current values to update the displayed values.
    /// </summary>
    private void UpdateTimeValues()
    {
      var currentTime = DateCalculator.GetCurrentTime(this);

      this.RemainingTime = DateCalculator.GetDeltaTime(this, currentTime);

      this.TargetTime = DateCalculator.GetTargetTime(this.StartTime, this.DefaultWorkLength, this.TotalBreakLength);
    }
  }
}