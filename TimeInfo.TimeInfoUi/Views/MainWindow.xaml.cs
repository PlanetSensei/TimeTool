//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Views
{
  using System;
  using System.Windows;

  using GalaSoft.MvvmLight.Messaging;

  using TimeTool.Infrastructure;
  using TimeTool.ViewModels;

  using Xceed.Wpf.DataGrid;
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

    /// <summary>
    /// Acts on the event that the main window is closed.
    /// Saves current user edit values.
    /// </summary>
    /// <param name="sender">The object that triggered this event.</param>
    /// <param name="e">Contains the event specific information.</param>
    private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.Save();
    }

    /// <summary>
    /// Sends out a message if the user double-clicked onto the time table grid control.
    /// </summary>
    /// <param name="sender">The object that triggered this event.</param>
    /// <param name="e">Contains the event specific information.</param>
    private void OnTimeTableMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Messenger.Default.Send<OpenEditorMessage>(new OpenEditorMessage());
    }
  }
}
