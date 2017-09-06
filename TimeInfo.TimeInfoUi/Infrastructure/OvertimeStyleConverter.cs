//-----------------------------------------------------------------------
// <copyright file="OvertimeStyleConverter.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.TimeToolUi.Infrastructure
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;

  /// <summary>
  /// Finds the background color that corresponds to the overtime/ undertime working time.
  /// </summary>
  public class OvertimeStyleConverter : IValueConverter
  {
    /// <summary>
    /// Gets or sets the control <see cref="Style"/> for overtime work.
    /// </summary>
    public Style Overtime { get; set; }

    /// <summary>
    /// Gets or sets the control <see cref="Style"/> for undertime work.
    /// </summary>
    public Style Undertime { get; set; }

    /// <summary>Converts a value. </summary>
    /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      // Check this before the following cast to avoid an exception. :-)
      if (value == null)
      {
        return this.Undertime;
      }

      var remaining = (TimeSpan)value;
      if (remaining < new TimeSpan(0))
      {
        return this.Undertime;
      }

      return this.Overtime;
    }

    /// <summary>Converts a value. </summary>
    /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}