//-----------------------------------------------------------------------
// <copyright file="OpenEditorMessage.cs" company="Jens Hellmann">
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
  public class OpenEditorMessage : MessageBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenEditorMessage"/> class.
    /// </summary>
    public OpenEditorMessage()
    {
    }
  }
}