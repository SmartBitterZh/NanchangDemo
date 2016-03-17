using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using YYT.Serialization;
using YYT.Serialization.Mobile;
using YYT.Core.FieldManager;
using YYT.Core;
using YYT.Properties;

namespace YYT.Core
{
  /// <summary>
  /// Inherit from this base class to easily
  /// create a serializable list class.
  /// </summary>
  /// <typeparam name="T">
  /// Type of the items contained in the list.
  /// </typeparam>
#if TESTING
  [System.Diagnostics.DebuggerStepThrough]
#endif
  [Serializable]
  public class MobileBindingList<T> : BindingList<T>, IMobileObject
  {
    #region IMobileObject Members

    void IMobileObject.GetChildren(SerializationInfo info, MobileFormatter formatter)
    {
      OnGetChildren(info, formatter);
    }

    void IMobileObject.GetState(SerializationInfo info)
    {
      OnGetState(info);
    }

    /// <summary>
    /// Override this method to get custom field values
    /// from the serialization stream.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    protected virtual void OnGetState(SerializationInfo info)
    {
      info.AddValue("YYT.Core.MobileList.AllowEdit", AllowEdit);
      info.AddValue("YYT.Core.MobileList.AllowNew", AllowNew);
      info.AddValue("YYT.Core.MobileList.AllowRemove", AllowRemove);
      info.AddValue("YYT.Core.MobileList.RaiseListChangedEvents", RaiseListChangedEvents);
    }

    /// <summary>
    /// Override this method to get custom child object
    /// values from the serialization stream.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="formatter">Reference to the MobileFormatter.</param>
    protected virtual void OnGetChildren(SerializationInfo info, MobileFormatter formatter)
    {
      if (!typeof(IMobileObject).IsAssignableFrom(typeof(T)))
        throw new InvalidOperationException(Resources.CannotSerializeCollectionsNotOfIMobileObject);

      List<int> references = new List<int>();
      for (int x = 0; x < this.Count; x++)
      {
        T child = this[x];
        if (child != null)
        {
          SerializationInfo childInfo = formatter.SerializeObject(child);
          references.Add(childInfo.ReferenceId);
        }
      }
      if (references.Count > 0)
        info.AddValue("$list", references);
    }

    void IMobileObject.SetState(SerializationInfo info)
    {
      OnSetState(info);
    }

    void IMobileObject.SetChildren(SerializationInfo info, MobileFormatter formatter)
    {
      OnSetChildren(info, formatter);
    }

    /// <summary>
    /// Override this method to set custom field values
    /// into the serialization stream.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    protected virtual void OnSetState(SerializationInfo info)
    {
      AllowEdit = info.GetValue<bool>("YYT.Core.MobileList.AllowEdit");
      AllowNew = info.GetValue<bool>("YYT.Core.MobileList.AllowNew");
      AllowRemove = info.GetValue<bool>("YYT.Core.MobileList.AllowRemove");
      RaiseListChangedEvents = info.GetValue<bool>("YYT.Core.MobileList.RaiseListChangedEvents");
    }

    /// <summary>
    /// Override this method to set custom child object
    /// values into the serialization stream.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="formatter">Reference to the MobileFormatter.</param>
    protected virtual void OnSetChildren(SerializationInfo info, MobileFormatter formatter)
    {
      if (!typeof(IMobileObject).IsAssignableFrom(typeof(T)))
        throw new InvalidOperationException(Resources.CannotSerializeCollectionsNotOfIMobileObject);

      if (info.Values.ContainsKey("$list"))
      {
        List<int> references = (List<int>)info.Values["$list"].Value;
        foreach (int reference in references)
        {
          T child = (T)formatter.GetObject(reference);
          this.Add(child);
        }
      }
    }

    #endregion
  }
}
