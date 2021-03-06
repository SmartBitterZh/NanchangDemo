﻿using System;

namespace YYT.Windows
{
  /// <summary>
  /// Event args indicating an error.
  /// </summary>
  public class ErrorEncounteredEventArgs : YYTActionEventArgs
  {
    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="commandName">
    /// Name of the command.
    /// </param>
    /// <param name="ex">
    /// Reference to the exception.
    /// </param>
    public ErrorEncounteredEventArgs(string commandName, Exception ex)
      : base(commandName)
    {
      _ex = ex;
    }

    private Exception _ex;

    /// <summary>
    /// Gets a reference to the exception object.
    /// </summary>
    public Exception Ex
    {
      get { return _ex; }
    }
  }
}
