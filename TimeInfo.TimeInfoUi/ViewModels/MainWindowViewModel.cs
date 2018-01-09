//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Deployment.Application;
  using System.Linq;
  using BusinessLogic;
  using Contracts;
  using DataAccess;
  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.CommandWpf;
  using GalaSoft.MvvmLight.Messaging;
  using Infrastructure;
  using Properties;

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
    /// Gets the collection that contains all work days in the current month.
    /// </summary>
    private string appVersion;

    /// <summary>
    /// Gets or sets the collection that contains all work days in the current month.
    /// </summary>
    private ObservableCollection<WorkdayViewModel> allDaysInMonth;

    /// <summary>
    /// Contains the fully qualified path and file name of the database.
    /// </summary>
    private string database;

    /// <summary>
    /// Gets or sets the <see cref="WorkdayViewModel"/> instance that represents the current day.
    /// </summary>
    private WorkdayViewModel today;

    /// <summary>
    /// Gets or sets the command object that updates the UI.
    /// </summary>
    private RelayCommand updateCommand;

    /// <summary>
    /// Represents the viewModel of the editor view.
    /// </summary>
    // TODO: Is this really needed?
    private WorkDayEditorViewModel workDayEditorViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// <inheritdoc />
    /// </summary>
    public MainWindowViewModel()
    {
      this.workDayEditorViewModel = new WorkDayEditorViewModel();
    }

    /// <summary>
    /// Gets the collection that contains all work days in the current month.
    /// </summary>
    public string AppVersion
  {
      get
      {
        return this.appVersion;
      }

      private set
      {
        this.Set(ref this.appVersion, value);
      }
    }

    /// <summary>
    /// Gets the collection that contains all work days in the current month.
    /// </summary>
    public ObservableCollection<WorkdayViewModel> AllDaysInMonth
    {
      get
      {
        return this.allDaysInMonth;
      }

      private set
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
    public WorkdayViewModel Today
    {
      get
      {
        return this.today;
      }

      set
      {
        this.Set(ref this.today, value);
        Messenger.Default.Send(new SelectedDayChangedMessage(this.Today));
      }
    }

    /// <summary>
    /// Saves the current states and values.
    /// </summary>
    public void Save()
    {
      using (var access = new WorkdayAccess(this.database))
      {
        access.Save(this.AllDaysInMonth);
      }
    }

    /// <summary>
    /// Initializes data that is needed fot this class.
    /// </summary>
    internal void Initialize()
    {
      // Initialize other stuff.
      this.database = FileSystem.FindDatabaseFile();

      this.AllDaysInMonth = new ObservableCollection<WorkdayViewModel>();
      this.GetAllDays(DateTime.Now.Year, DateTime.Now.Month);

      var todayDate = DateTime.Now.Date;
      this.Today = this.AllDaysInMonth.Single(day => day.StartTime.Date == todayDate);

      if (this.Today.StartTime.TimeOfDay.Equals(DateTime.MinValue.TimeOfDay))
      {
        var logon = UserInfo.GetLastLogOnToMachine(this.Today.StartTime.Date);
        this.Today.StartTime = logon;
      }

      this.Today = this.AllDaysInMonth.Single(day => DateTime.Now.Date.Equals(day.StartTime.Date));

      this.AppVersion = GetPublishedVersion();

      this.UpdateCommand = new RelayCommand(
        () =>
        {
          this.Today.RemainingTime = Calculator.GetDeltaTime(
            this.Today.StartTime,
            this.Today.DefaultWorkLength,
            this.Today.TotalBreakLength,
            DateTime.Now);
        });
    }

    /// <summary>
    /// Finds the application version that is set during ClickOnce publish.
    /// </summary>
    /// <returns>Returns the found version number or a notification text if the version could be found.</returns>
    private static string GetPublishedVersion()
    {
      if (ApplicationDeployment.IsNetworkDeployed)
      {
        return ApplicationDeployment.CurrentDeployment.
                      CurrentVersion.ToString();
      }

      return "Version number only visible after publishing the application.";
    }

    /// <summary>
    /// Assign the available work day objects to the collection that is bound to the UI.
    /// </summary>
    /// <param name="workdayInfos">The available work day objects.</param>
    private void AssignToUi(IWorkdayInfo[] workdayInfos)
    {
      // I prefer clearing the collection because every now and then something goes wrong
      // if the observalble collection is newly instanciated and then bound again to the UI.
      this.AllDaysInMonth.Clear();

      foreach (var info in workdayInfos)
      {
        var viewModel = new WorkdayViewModel
                          {
                            DefaultWorkLength = info.DefaultWorkLength,
                            EndTime = info.EndTime,
                            StartTime = info.StartTime,
                            TotalBreakLength = info.TotalBreakLength,
                            WorkdayId = info.WorkdayId
                          };

        this.AllDaysInMonth.Add(viewModel);
      }
    }

    /// <summary>
    /// Loads all available days of the month into memory.
    /// </summary>
    /// <param name="year">The year for which the work day information are collected.</param>
    /// <param name="month">The month for which the work day information are collected.</param>
    private void GetAllDays(int year, int month)
    {
      using (var access = new WorkdayAccess(this.database))
      {
        var workdayInfos = this.GetOrCreate(year, month, access);

        this.AssignToUi(workdayInfos);
      }
    }

    /// <summary>
    /// Fetches already existing work day objects from the data source or creates them as needed.
    /// </summary>
    /// <param name="year">The current year for which the data is retreved.</param>
    /// <param name="month">The current month for which the data is retreved.</param>
    /// <param name="access">The data source collection.</param>
    /// <returns>Returns all available work day objects for the specified
    /// <paramref name="year"/> and <paramref name="month"/>.</returns>
    private IWorkdayInfo[] GetOrCreate(int year, int month, WorkdayAccess access)
    {
      // TODO: This method might be better suited for a dedicated data access class.
      // TODO: Move into another class.
      var workdayInfos = access.GetDays(year, month)
                               .ToArray();

      var expectedNumberOfDays = DateTime.DaysInMonth(year, month);

      if (workdayInfos.Length != expectedNumberOfDays)
      {
        workdayInfos = access.CreateMonth(
                               year,
                               month,
                               this.appSettings.DefaultWorkLength,
                               this.appSettings.DefaultBreakLength)
                             .ToArray();
      }

      return workdayInfos;
    }
  }
}