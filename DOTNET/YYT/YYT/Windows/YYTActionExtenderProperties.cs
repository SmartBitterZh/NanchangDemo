#region Namespace imports

using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using YYT;

#endregion

namespace YYT.Windows
{
  internal class YYTActionExtenderProperties
  {
    #region Defaults

    static internal YYTFormAction ActionTypeDefault = YYTFormAction.None;
    static internal PostSaveActionType PostSaveActionDefault = PostSaveActionType.None;
    static internal bool RebindAfterSaveDefault = true;
    static internal bool DisableWhenCleanDefault = false;
    static internal string CommandNameDefault = string.Empty;

    #endregion

    #region Member variables

    protected YYTFormAction _actionType = ActionTypeDefault;
    protected PostSaveActionType _postSaveAction = PostSaveActionDefault;
    protected bool _rebindAfterSave = RebindAfterSaveDefault;
    protected bool _disableWhenClean = DisableWhenCleanDefault;
    protected string _commandName = CommandNameDefault;

    #endregion

    #region Public properties

    public YYTFormAction ActionType
    {
      get { return _actionType; }
      set { _actionType = value; }
    }

    public PostSaveActionType PostSaveAction
    {
      get { return _postSaveAction; }
      set { _postSaveAction = value; }
    }

    public bool RebindAfterSave
    {
      get { return _rebindAfterSave; }
      set { _rebindAfterSave = value; }
    }

    public bool DisableWhenClean
    {
      get { return _disableWhenClean; }
      set { _disableWhenClean = value; }
    }

    public string CommandName
    {
      get { return _commandName; }
      set { _commandName = value; }
    }

    #endregion
  }
}
