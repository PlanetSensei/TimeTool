//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.TimeToolUi.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Linq;

  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.CommandWpf;

  using TimeTool.BusinessLogic;
  using TimeTool.DataAccess;
  using TimeTool.TimeToolUi.Properties;

  //using TimeTool.TimeToolUi.Properties;

  /// <summary>
  /// Provides interaction logic for the MainWindow.xaml view.
  /// </summary>
  public class MainWindowViewModel : ViewModelBase
  {
    /// <summary>
    /// Represents the application settings from the application configuration file.
    /// </summary>
    private readonly Settings appSettings = new Settings();

    /// <summary>
    /// Gets or sets the collection that contains all work days in the current month.
    /// </summary>
    private ObservableCollection<WorkDayViewModel> allDaysInMonth;

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
      this.AllDaysInMonth = new ObservableCollection<WorkDayViewModel>();
      this.GetAllDays(DateTime.Now.Year, DateTime.Now.Month);

      var todayDate = DateTime.Now.Date;
      this.Today = this.AllDaysInMonth.Single(day => day.Date == todayDate);

      if (this.Today.StartTime.Equals(DateTime.MinValue))
      {
        var logon = UserInfo.GetLastLoginToMachine();
        this.Today.StartTime = logon;
      }

      this.UpdateCommand = new RelayCommand(
                             () =>
                               {
                                 this.Today.RemainingTime = Calculator.GetDeltaTime(
                                   this.Today.StartTime,
                                   this.Today.DailyWorkLength,
                                   this.Today.TotalBreakLength,
                                   DateTime.Now);
                               });
    }

    /// <summary>
    /// Gets or sets the collection that contains all work days in the current month.
    /// </summary>
    public ObservableCollection<WorkDayViewModel> AllDaysInMonth
    {
      get
      {
        return this.allDaysInMonth;
      }

      set
      {
        this.Set(ref this.allDaysInMonth, value);
      }
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

    /// <summary>
    /// Saves the current states and values.
    /// </summary>
    public void Save()
    {
      using (var access = new WorkDayAccess(this.appSettings.DatabaseLocation))
      {
        access.Save(this.Today);
      }
    }

    /// <summary>
    /// Loads all available days of the month into memory.
    /// </summary>
    /// <param name="year">The year for which the work day information are collected.</param>
    /// <param name="month">The month for which the work day information are collected.</param>
    private void GetAllDays(int year, int month)
    {
      using (var access = new WorkDayAccess(this.appSettings.DatabaseLocation))
      {
        var workDayInfos = access.GetDays(year, month)
                                 .ToArray();

        if (workDayInfos.Length == 0)
        {
          workDayInfos = access.CreateMonth(
                                 year,
                                 month,
                                 this.appSettings.DailyWorkLength,
                                 this.appSettings.DefaultBreakLength)
                               .ToArray();
        }

        this.AllDaysInMonth.Clear();
        foreach (var info in workDayInfos)
        {
          var viewModel = new WorkDayViewModel
                            {
                              DailyWorkLength = info.DailyWorkLength,
                              StartTime = info.StartTime,
                              Date = info.Date,
                              TotalBreakLength = info.TotalBreakLength,
                              WorkDayId = info.WorkDayId
                            };

          this.AllDaysInMonth.Add(viewModel);
        }
      }
    }
  }
}