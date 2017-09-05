//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeInfo.TimeInfoUi.Views
{
  using System;
  using System.Windows;
  using System.Windows.Controls;

  using TimeInfo.TimeInfoUi.ViewModels;

  using Xceed.Wpf.Toolkit;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
      this.InitializeComponent();
    }

    private void TimePicker_TextChanged(object sender, TextChangedEventArgs e)
    {
      var picker = (TimePicker)sender;
      //picker.Focus();
      
      var viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.Today.StartTime = (DateTime)picker.Value;

      //viewModel.Today.TargetTime = Calculator.GetTargetTime(viewModel.Today);
      //viewModel.Today.RemainingTime = Calculator.GetDeltaTime(viewModel.Today, DateTime.Now);
    }
  }
}
