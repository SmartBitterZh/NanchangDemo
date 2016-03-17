using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.MySocketServer.SocketHelper
{
  public class EntityModelListResponse<TModel> : BaseResponse
  {
    private IList<TModel> entity;
    public IList<TModel> Entity { get { return entity; } set { entity = value; } }

    public override object Context
    {
      get
      {
        return entity;
      }
      set
      {
        entity = (value as IList<TModel>);
      }
    }
  }
}
