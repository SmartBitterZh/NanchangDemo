using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer.SocketHelper
{
  public class EntityModelListResponse<TModel> : BaseResponse<TModel>
  {
    private IList<TModel> entity;
    public IList<TModel> Entity { get { return entity; } set { entity = value; } }

    public override IList<TModel> Context
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
