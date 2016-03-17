using System;

namespace YYT.Windows
{
  /// <summary>
  /// Event args for an action.
  /// </summary>
  public class YYTActionEventArgs : EventArgs
  {
    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="commandName">
    /// Name of the command.
    /// </param>
    public YYTActionEventArgs(string commandName)
    {
      _commandName = commandName;
    }

    private string _commandName;

    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    public string CommandName
    {
      get { return _commandName; }
    }
  }
}
