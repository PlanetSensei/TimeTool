//-----------------------------------------------------------------------
// <copyright file="OvertimeStyleSelector.cs" company="Jens Hellmann">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeInfo.TimeInfoUi.Infrastructure
{
  using System.Windows;
  using System.Windows.Controls;

  using TimeInfo.TimeInfoUi.ViewModels;

  /// <summary>
  /// No description available.
  /// </summary>
  public class OvertimeStyleSelector : StyleSelector
  {
    /// <summary>
    /// Gets or sets the control <see cref="Style"/> for overtime work.
    /// </summary>
    public Style Overtime { get; set; }

    /// <summary>
    /// Gets or sets the control <see cref="Style"/> for undertime work.
    /// </summary>
    public Style Undertime { get; set; }

    /// <summary>
    /// Selects the signal color for overtime or undertime work times.
    /// </summary>
    /// <returns>Returns an application-specific style to apply; otherwise, null.</returns>
    /// <param name="item">The content.</param>
    /// <param name="container">The element to which the style will be applied.</param>
    public override Style SelectStyle(object item, DependencyObject container)
    {
      var mainWindowViewModel = item as MainWindowViewModel;
      return base.SelectStyle(item, container);
    }
  }
}