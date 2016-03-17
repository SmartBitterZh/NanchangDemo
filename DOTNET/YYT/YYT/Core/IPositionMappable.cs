using System;

namespace YYT.Core
{
  interface IPositionMappable<T>
  {
    int PositionOf(T item);
  }
}
