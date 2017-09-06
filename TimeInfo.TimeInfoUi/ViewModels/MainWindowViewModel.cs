//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.TimeToolUi.ViewModels
{
  using System;

  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.CommandWpf;

  using TimeTool.BusinessLogic;

  /// <summary>
  /// Provides interaction logic for the MainWindow.xaml view.
  /// </summary>
  public class MainWindowViewModel : ViewModelBase
  {
    /// <summary>
    /// Gets or sets the <see cref="WorkDayViewModel"/> instance that represents the current day.
    /// </summary>
    private WorkDayViewModel today;

    /// <summary>
    /// Gets or sets the command object that updates the UI.
    /// </summary>
    private RelayCommand updateCommand;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
      var logon = UserInfo.GetLastLoginToMachine();
      
      this.Today = new WorkDayViewModel
                     {
                       DailyWorkLength = new TimeSpan(7, 42, 0),
                       StartTime = new DateTime(logon.Year, logon.Month, logon.Day, logon.Hour, logon.Minute, logon.Second),
                       TotalBreakLength = new TimeSpan(0, 30, 0)
                     };

      this.UpdateCommand = new RelayCommand(
                             () =>
                               {
                                 this.Today.RemainingTime = Calculator.GetDeltaTime(this.Today, DateTime.Now);
                               });
    }

    /// <summary>
    /// Gets or sets the command object that updates the UI.
    /// </summary>
    public RelayCommand UpdateCommand
    {
      get
      {
        return this.updateCommand;
      }

      set
      {
        this.Set(ref this.updateCommand, value);
      }
    }

    /// <summary>
    /// Gets or sets the time when the user started work on this day.
    /// </summary>
    public WorkDayViewModel Today
    {
      get
      {
        return this.today;
      }

      set
      {
        this.Set(ref this.today, value);
      }
    }
  }
}