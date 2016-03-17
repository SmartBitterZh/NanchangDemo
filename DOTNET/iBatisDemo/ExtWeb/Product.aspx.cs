using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBLL;
using SpringIoc;
using Model;
using System.Collections;
using System.Text;

public partial class Product : System.Web.UI.Page
{
    private IProductBLL productBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        productBLL = (IProductBLL)SpringContext.Context.CreateInstance("ProductBLL");

        string cmd = Request.QueryString["cmd"];
        switch (cmd)
        {
            case "list":
                List();
                break;
            case "save":
                Save();
                break;
            case "del":
                Del();
                break;
            default:
                break;
        }
    }

    private void List()
    {
        int currentPage = 1;
        int.TryParse(Request.QueryString["page"], out currentPage);
        if (currentPage <= 0)
        {
            currentPage = 1;
        }

        PagerItem pagerItem = new PagerItem();
        pagerItem.CurrentPage = currentPage;
        pagerItem.PageSize = 15;
        pagerItem.Keywords = "";

        IProductBLL productBLL = (IProductBLL)SpringContext.Context.CreateInstance("ProductBLL");
        IList list = productBLL.Select(pagerItem);
        int count = list.Count;

        StringBuilder result = new StringBuilder();
        result.Append("{result:[");
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Model.Product product = (Model.Product)list[i];
                result.Append("{ID:").Append(product.ID).Append(",");
                result.Append("ProductName:\"").Append(product.ProductName).Append("\",");
                result.Append("NormalPrice:").Append(product.NormalPrice).Append(",");
                result.Append("MemberPrice:").Append(product.MemberPrice).Append("}");
                if (i < count - 1)
                {
                    result.Append(",");
                }
            }
        }

        result.Append("]}");

        Response.Write(result.ToString());
        Response.End();
    }

    private void Save()
    {
        string productId = Request.QueryString["productId"];

        Model.Product product = new Model.Product();
        product.ProductName = Request.Form["ProductName"];
        product.NormalPrice = decimal.Parse(Request.Form["NormalPrice"]);
        product.MemberPrice = decimal.Parse(Request.Form["MemberPrice"]);

        if (string.IsNullOrEmpty(productId))
        {
            productBLL.Insert(product);
        }
        else
        {
            product.ID = int.Parse(productId);
            productBLL.Update(product);
        }
        Response.Write("{success: true}");
        Response.End();
    }

    private void Del()
    {
        string id = Request.QueryString["productId"];
        if (!string.IsNullOrEmpty(id))
        {
            int productId = 0;
            int.TryParse(id, out productId);
            
            productBLL.Delete(productId);
            Response.Write("{success: true}");
        }
        else
        {
            Response.Write("{success: false}");
        }
        Response.End();
    }
}
