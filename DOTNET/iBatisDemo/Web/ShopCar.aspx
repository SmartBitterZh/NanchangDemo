<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopCar.aspx.cs" Inherits="ShopCar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    table{background-color:#666;}
    table tr th{font-size:14px; padding:5px; font-family:Arial;}
    table tr td{ background-color:#e5e5e5; padding:5px; font-size:12px; font-family:Arial;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div><h2>我的购物车</h2></div>
    
    <table border="0" cellspacing="1">
        <tr>
            <th>NO</th>
            <th>ProductName</th>
            <th>Price</th>
            <th>Num</th>
            <th>Manage</th>
        </tr>
    <asp:Repeater id="rpt" runat="server" onitemcommand="rpt_ItemCommand">
    <ItemTemplate>
        <tr>
            <td><%# Container.ItemIndex+1 %></td>
            <td><%# Eval("ProductName") %></td>
            <td>$<%# decimal.Parse(Eval("Price").ToString()).ToString("#") %></td>
            <td><asp:Label ID="lblNum" runat="server" Text='<%# Eval("Num") %>' /><asp:TextBox ID="txtNum" runat="server" Visible="false" /></td>
            <td>
            <asp:LinkButton ID="lnkbtnDel" runat="server" CommandArgument='<%# Eval("ProductID") %>' CommandName="delete" Text="Delete" OnClientClick="return confirm('确定要删除吗？');"></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandArgument='<%# Eval("ProductID") %>' CommandName="Edit" Text="Edit"></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnSave" runat="server" CommandArgument='<%# Eval("ProductID") %>' CommandName="Save" Text="Save" Visible="false"></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnCancel" runat="server" CommandArgument='<%# Eval("ProductID") %>' CommandName="Cancel" Text="Cancel" Visible="false"></asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
    </asp:Repeater>
        <tr>
            <td colspan="5"><asp:Label ID="lblTotal" runat="server" ForeColor="Red" />　　　　
            <a href="ProductList.aspx">GO ON</a>
            <asp:Button ID="btnOrder" runat="server" Text="Order" OnClick="btnOrder_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
