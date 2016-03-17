#if !CLIENTONLY
using System.Web.UI.Design;

namespace YYT.Web.Design
{

  /// <summary>
  /// Object providing access to schema information for
  /// a business object.
  /// </summary>
  /// <remarks>
  /// This object returns only one view, which corresponds
  /// to the business object used by data binding.
  /// </remarks>
  public class ObjectSchema : IDataSourceSchema
  {
    private string _typeName = "";
    private YYTDataSourceDesigner _designer;

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="designer">Data source designer object.</param>
    /// <param name="typeName">Type name for
    /// which the schema should be generated.</param>
    public ObjectSchema(YYTDataSourceDesigner designer, string typeName)
    {
      _typeName = typeName;
      _designer = designer;
    }


    /// <summary>
    /// Returns a single element array containing the
    /// schema for the YYT .NET business object.
    /// </summary>
    public System.Web.UI.Design.IDataSourceViewSchema[] GetViews()
    {
      IDataSourceViewSchema[] result = null;
      result = new IDataSourceViewSchema[] { new ObjectViewSchema(_designer, _typeName) };
      return result;
    }
  }
}
#endif