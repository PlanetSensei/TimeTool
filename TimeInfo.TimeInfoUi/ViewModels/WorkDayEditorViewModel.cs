//-----------------------------------------------------------------------
// <copyright file="WorkDayEditorViewModel.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.ViewModels
{
  using System.Windows;

  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.Messaging;

  using TimeTool.Infrastructure;
  using TimeTool.Views;

  /// <summary>
  /// Provides intaeraction logic for <see cref="WorkDayEditorView"/> dialog.
  /// </summary>
  public class WorkDayEditorViewModel : ViewModelBase
  {
    /// <summary>
    /// Gets or sets a value indicating whether this window is
    /// visible (<see langword="true"/>) or not (<see langword="false"/>).
    /// </summary>
    private Visibility visibility;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkDayEditorViewModel"/> class.
    /// </summary>
    public WorkDayEditorViewModel()
    {
      Messenger.Default.Register<OpenEditorMessage>(this, this.ShowEditor);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this window is
    /// visible (<see langword="true"/>) or not (<see langword="false"/>).
    /// </summary>
    public Visibility Visibility
    {
      get
      {
        return this.visibility;
      }

      set
      {
        this.Set(ref this.visibility, value);
      }
    }

    /// <summary>
    /// Opens the editor window.
    /// </summary>
    /// <param name="message">The received message instance.</param>
    private void ShowEditor(OpenEditorMessage message)
    {
      this.Visibility = Visibility.Visible;
    }
  }
}