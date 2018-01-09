//-----------------------------------------------------------------------
// <copyright file="ApplicationClosingMessage.cs" company="Jens Hellmann">
//   Copyright (c) Jens Hellmann. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimeTool.Infrastructure
{
  using GalaSoft.MvvmLight.Messaging;

  /// <inheritdoc />
  /// <summary>
  /// Represents a message object that is send between viewmodels and views to open a editor window.
  /// </summary>
  public class ApplicationClosingMessage : MessageBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationClosingMessage"/> class.
    /// </summary>
    public ApplicationClosingMessage()
    {
    }
  }
}