//-----------------------------------------------------------------------
// <copyright file="TimeSpanToStringConverter.cs" company="Copyright (c) Jens Hellmann. All rights reserved.">
// Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Infrastructure
{
  using System;
  using System.Globalization;
  using System.Windows;
  using System.Windows.Data;

  /// <summary>
  /// Converts the TimeSpan value into a string for databinding.
  /// </summary>
  public class TimeSpanToStringConverter : IValueConverter
  {
    /// <summary>Converts the TimeSpan value into a string for databinding.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
      {
        return DependencyProperty.UnsetValue;
      }

      var remainingTime = (TimeSpan)value;

      // FormatStrings for TimeSpan see:
      // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings
      return remainingTime.ToString(@"\-hh\:mm\:ss");
    }

    /// <summary>Converts a value. </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}