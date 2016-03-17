<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Enter.aspx.cs" Inherits="Enter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    CustomID:<asp:TextBox ID="txtCustomId" runat="server" Text="1" />
    <asp:Button ID="btnOK" runat="server" Text="Login" OnClick="btnOK_Click" />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
