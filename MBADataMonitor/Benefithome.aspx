<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Benefithome.aspx.cs" Inherits="Benefithome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BenefitHome</title>

        <link rel="stylesheet" href="Content/metro-all.min.css">

    <style type="text/css">
        .auto-style1 {
            width: 447px;
            height: 165px;
            position: absolute;
            top: 153px;
            left: 779px;
            z-index: 1;
        }
        .auto-style2 {
            width: 741px;
            height: 257px;
            position: absolute;
            top: 165px;
            left: 9px;
            z-index: 1;
            margin-top: 0px;
        }
        .auto-style3 {
            left: 9px;
            top: 49px;
            height: 37px;
        }
    </style>

</head>
<body style="height: 470px">
    <ul class="h-menu bg-green fg-white">
    <li><a href="default.aspx">BridgeHome</a></li>
    <li><a href="BenefitHome.aspx">Benefit</a></li>
    <li><a href="#">Setup</a></li>
    <li><a href="#">BenefitRecon</a></li>
</ul>
    <form id="form1" runat="server">
      
        <p>
            <asp:Button ID="Button1" runat="server" class="button primary" OnClick="Button1_Click1" Text="Button" />
        </p>

        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        <asp:GridView ID="GridView1" runat="server" CssClass="auto-style1" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
        <div class="auto-style3">
        </div>

          <asp:ListBox ID="ListBox1" runat="server" CssClass="auto-style2"></asp:ListBox>
    </form>
</body>
</html>
