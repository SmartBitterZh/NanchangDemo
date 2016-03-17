using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.Helper
{
  public sealed class ObjectFactory
  {
    public static T GetInstance<T>()
    {

      Type _type = typeof(T);
      //return _type.GetConstructor(null).Invoke(null);
      return (T)Activator.CreateInstance(_type);
    }
  }
}
