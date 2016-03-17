using System;
using System.Windows.Input;
using System.ComponentModel;

namespace YYT.Wpf
{
  /// <summary>
  /// Implements support for RoutedCommands that can
  /// be executed by the YYTDataProvider control.
  /// </summary>
  /// <remarks>
  /// Use this object as the CommandTarget for command
  /// source objects when you want the YYTDataProvider
  /// to execute the command.
  /// </remarks>
  public class YYTDataProviderCommandManager : System.Windows.UIElement
  {
    private YYTDataProvider _provider;

    private YYTDataProvider Provider
    {
      get { return _provider; }
    }

    internal YYTDataProviderCommandManager(YYTDataProvider provider)
    {
      _provider = provider;
    }

    static YYTDataProviderCommandManager()
    {
      CommandBinding binding;
      
      binding = new CommandBinding(ApplicationCommands.Save, SaveCommand, CanExecuteSave);
      CommandManager.RegisterClassCommandBinding(typeof(YYTDataProviderCommandManager), binding);
      
      binding = new CommandBinding(ApplicationCommands.Undo, UndoCommand, CanExecuteUndo);
      CommandManager.RegisterClassCommandBinding(typeof(YYTDataProviderCommandManager), binding);

      binding = new CommandBinding(ApplicationCommands.New, NewCommand, CanExecuteNew);
      CommandManager.RegisterClassCommandBinding(typeof(YYTDataProviderCommandManager), binding);

      binding = new CommandBinding(ApplicationCommands.Delete, RemoveCommand, CanExecuteRemove);
      CommandManager.RegisterClassCommandBinding(typeof(YYTDataProviderCommandManager), binding);
    }

    private static void CanExecuteSave(object target, CanExecuteRoutedEventArgs e)
    {
      bool result = false;
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
      {
        YYT.Core.IEditableBusinessObject ibiz = ctl.Provider.Data as YYT.Core.IEditableBusinessObject;
        if (ibiz != null)
          result = ibiz.IsSavable;
        else
        {
          YYT.Core.IEditableCollection icol = ctl.Provider.Data as YYT.Core.IEditableCollection;
          if (icol != null)
            result = icol.IsSavable;
        }
      }
      e.CanExecute = result;
    }

    private static void SaveCommand(object target, ExecutedRoutedEventArgs e)
    {
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
        ctl.Provider.Save();
    }

    private static void CanExecuteUndo(object target, CanExecuteRoutedEventArgs e)
    {
      bool result = false;
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
      {
        if (ctl.Provider.Data != null)
        {
          YYT.Core.IEditableBusinessObject ibiz = ctl.Provider.Data as YYT.Core.IEditableBusinessObject;
          if (ibiz != null)
            result = ibiz.IsDirty;
          else
          {
            YYT.Core.IEditableCollection icol = ctl.Provider.Data as YYT.Core.IEditableCollection;
            if (icol != null)
              result = icol.IsDirty;
          }
        }
      }
      e.CanExecute = result;
    }

    private static void UndoCommand(object target, ExecutedRoutedEventArgs e)
    {
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
        ctl.Provider.Cancel();
    }

    private static void CanExecuteNew(object target, CanExecuteRoutedEventArgs e)
    {
      bool result = false;
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
      {
        if (ctl.Provider.Data != null)
        {
          IBindingList list = ctl.Provider.Data as IBindingList;
          if (list != null)
          {
            result = list.AllowNew;
            if (result && !YYT.Security.AuthorizationRules.CanEditObject(ctl.Provider.Data.GetType()))
              result = false;
          }
        }
      }
      e.CanExecute = result;
    }

    private static void NewCommand(object target, ExecutedRoutedEventArgs e)
    {
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
        ctl.Provider.AddNew();
    }

    private static void CanExecuteRemove(object target, CanExecuteRoutedEventArgs e)
    {
      bool result = false;
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
      {
        if (ctl.Provider.Data != null)
        {
          YYT.Core.BusinessBase bb = e.Parameter as YYT.Core.BusinessBase;
          IBindingList list;
          if (bb != null)
            list = bb.Parent as IBindingList;
          else
            list = ctl.Provider.Data as IBindingList;
          if (list != null)
          {
            result = list.AllowRemove;
            if (result && !YYT.Security.AuthorizationRules.CanEditObject(ctl.Provider.Data.GetType()))
              result = false;
          }
        }
      }
      e.CanExecute = result;
    }

    private static void RemoveCommand(object target, ExecutedRoutedEventArgs e)
    {
      YYTDataProviderCommandManager ctl = target as YYTDataProviderCommandManager;
      if (ctl != null && ctl.Provider != null)
        ctl.Provider.RemoveItem(e.Parameter);
    }
  }
}