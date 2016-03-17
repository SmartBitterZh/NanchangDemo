using System;
using YYT.Server.Hosts.WcfChannel;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace YYT.Server.Hosts
{
  /// <summary>
  /// Exposes server-side DataPortal functionality
  /// through WCF.
  /// </summary>
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class WcfPortal : IWcfPortal
  {
    #region IWcfPortal Members

    /// <summary>
    /// Create a new business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    public WcfResponse Create(CreateRequest request)
    {
      YYT.Server.DataPortal portal = new YYT.Server.DataPortal();
      object result;
      try
      {
        result = portal.Create(request.ObjectType, request.Criteria, request.Context);
      }
      catch (Exception ex)
      {
        result = ex;
      }
      return new WcfResponse(result);
    }

    /// <summary>
    /// Get an existing business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    public WcfResponse Fetch(FetchRequest request)
    {
      YYT.Server.DataPortal portal = new YYT.Server.DataPortal();
      object result;
      try
      {
        result = portal.Fetch(request.ObjectType, request.Criteria, request.Context);
      }
      catch (Exception ex)
      {
        result = ex;
      }
      return new WcfResponse(result);
    }

    /// <summary>
    /// Update a business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    public WcfResponse Update(UpdateRequest request)
    {
      YYT.Server.DataPortal portal = new YYT.Server.DataPortal();
      object result;
      try
      {
        result = portal.Update(request.Object, request.Context);
      }
      catch (Exception ex)
      {
        result = ex;
      }
      return new WcfResponse(result);
    }

    /// <summary>
    /// Delete a business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    public WcfResponse Delete(DeleteRequest request)
    {
      YYT.Server.DataPortal portal = new YYT.Server.DataPortal();
      object result;
      try
      {
        result = portal.Delete(request.ObjectType, request.Criteria, request.Context);
      }
      catch (Exception ex)
      {
        result = ex;
      }
      return new WcfResponse(result);
    }

    #endregion
  }
}