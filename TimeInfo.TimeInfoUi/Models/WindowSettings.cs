using System;
using System.Drawing;
using TimeTool.Contracts;

namespace TimeTool.Models
{
  /// <summary>
  /// Provides properties about the main window.
  /// </summary>
  [Serializable]
  public class WindowSettings : IDeepCloneable
  {
    #region Constructor

    /// <summary>
    /// Creates a new instance of type WindowSettings.
    /// </summary>
    public WindowSettings()
    {
      this.Location = new Point(1050, 870);
    }

    #endregion Constructor

    /// <summary>
    /// Gets or sets the current location of the main window.
    /// </summary>
    public Point Location { get; set; }
  }
}
