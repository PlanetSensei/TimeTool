//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Views
{
  using System.Windows;

  using TimeTool.ViewModels;

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

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.Save();
    }
  }
}
