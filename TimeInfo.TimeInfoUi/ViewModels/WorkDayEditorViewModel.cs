//-----------------------------------------------------------------------
// <copyright file="WorkDayEditorViewModel.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.ViewModels
{
  using System.Windows;
  using Contracts;
  using GalaSoft.MvvmLight;
  using GalaSoft.MvvmLight.Messaging;
  using Infrastructure;
  using JetBrains.Annotations;
  using Views;

  /// <summary>
  /// Provides intaeraction logic for <see cref="WorkdayEditorView"/> dialog.
  /// </summary>
  public class WorkDayEditorViewModel : ViewModelBase
  {
    /// <summary>
    /// The acutual editor window.
    /// </summary>
    private WorkdayEditorView editorView;

    /// <summary>
    /// Gets or sets the <see cref="IWorkdayInfo"/> instance that is currently edited in the editor view.
    /// </summary>
    private WorkdayViewModel day;

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
      this.editorView = new WorkdayEditorView
      {
        DataContext = this
      };

      this.Visibility = Visibility.Hidden;

      Messenger.Default.Register<ApplicationClosingMessage>(this, this.AppClosing);
      Messenger.Default.Register<OpenEditorMessage>(this, this.ToggleVisibility);
      Messenger.Default.Register<SelectedDayChangedMessage>(this, this.UpdateView);
    }

    /// <summary>
    /// Gets or sets the <see cref="IWorkdayInfo"/> instance that is currently edited in the editor view.
    /// </summary>
    [UsedImplicitly]
    public WorkdayViewModel Day
    {
      get => this.day;

      set => this.Set(ref this.day, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this window is
    /// visible (<see langword="true"/>) or not (<see langword="false"/>).
    /// </summary>
    public Visibility Visibility
    {
      get => this.visibility;

      set => this.Set(ref this.visibility, value);
    }

    /// <summary>
    /// Performs cleanup actions when the application gets closed.
    /// </summary>
    /// <param name="message">The message object that informs about the closing application.</param>
    private void AppClosing(ApplicationClosingMessage message)
    {
      this.editorView?.Close();

      Messenger.Default.Unregister<ApplicationClosingMessage>(this);
      Messenger.Default.Unregister<OpenEditorMessage>(this);
      Messenger.Default.Unregister<SelectedDayChangedMessage>(this);
    }

    /// <summary>
    /// Opens the editor window.
    /// </summary>
    /// <param name="message">The received message instance.</param>
    private void ToggleVisibility(OpenEditorMessage message)
    {
      if (this.Visibility == Visibility.Visible)
      {
        this.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.Visibility = Visibility.Visible;
        this.editorView.Show();
      }
    }

    /// <summary>
    /// Updates the view with the data of the specified <paramref name="message"/>.
    /// </summary>
    /// <param name="message">Contais the currently selected <see cref="WorkDayEditorViewModel"/> instance.</param>
    private void UpdateView(SelectedDayChangedMessage message)
    {
      this.Day = message.Day;
    }
  }
}