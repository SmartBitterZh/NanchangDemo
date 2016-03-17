using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class ProductEidt : System.Web.UI.Page
{
    private string _productId;
    private IProductBLL _productBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        _productId = Request.QueryString["id"];
        _productBLL = (IProductBLL)SpringContext.Context.CreateSecurityProxyInstance("ProductBLL");

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(_productId))
            {
                GetInfo(int.Parse(_productId));
            }
        }
    }

    private void GetInfo(int id)
    {
        
        Product product = _productBLL.Select(id);
        if (product != null)
        {
            txtProductName.Text = product.ProductName;
            txtNormalPrice.Text = product.NormalPrice.ToString("#");
            txtMemberPrice.Text = product.MemberPrice.ToString("#");
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Product product = new Product();
        product.ProductName = txtProductName.Text;
        product.NormalPrice = decimal.Parse(txtNormalPrice.Text);
        product.MemberPrice = decimal.Parse(txtMemberPrice.Text);

        if (string.IsNullOrEmpty(_productId))
        {
            _productBLL.Insert(product);
        }
        else
        {
            product.ID = int.Parse(_productId);
            _productBLL.Update(product);
        }
        Response.Redirect("ProductList.aspx", true);
    }
}
