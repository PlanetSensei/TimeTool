//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Views
{
  using System;
  using System.Windows;

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

      var editor = new CellEditor();
      editor.EditTemplate = new DataTemplate(typeof(TimePicker));

      if (this.timeTable.DefaultCellEditors.ContainsKey(typeof(DateTime)))
      {
        editor = this.timeTable.DefaultCellEditors[typeof(DateTime)];
        editor.EditTemplate = new DataTemplate(new TimePicker());
      }
      else
      {
        editor = new CellEditor();
        editor.EditTemplate = new DataTemplate(typeof(TimePicker));

        this.timeTable.DefaultCellEditors.Add(typeof(DateTime), editor);
      }

      this.timeTable.Columns["StartTime"]
          .CellEditor = editor;
    }

    /// <summary>
    /// Acts on the event that the main window is closed.
    /// Saves current user edit values.
    /// </summary>
    /// <param name="sender">The object that triggered this event.</param>
    /// <param name="e">Contains the event specific information.</param>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.Save();
    }
  }
}
