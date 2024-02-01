<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Apitest.aspx.cs" Inherits="Apitest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            position: absolute;
            top: 42px;
            left: 436px;
            z-index: 1;
        }
        .auto-style2 {
            position: absolute;
            top: 41px;
            left: 61px;
            z-index: 1;
            width: 300px;
        }
        .auto-style3 {
            position: absolute;
            top: 78px;
            left: 610px;
            z-index: 1;
            width: 133px;
        }
        .auto-style4 {
            position: absolute;
            top: 78px;
            left: 15px;
            z-index: 1;
            width: 389px;
            height: 338px;
        }
        .auto-style5 {
            position: absolute;
            top: 83px;
            z-index: 1;
            left: 837px;
        }
        .auto-style6 {
            position: absolute;
            top: 450px;
            left: 50px;
            z-index: 1;
            width: 564px;
            height: 74px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

        </div>
        <asp:Button ID="Button1" runat="server" CssClass="auto-style1" OnClick="Button1_Click" Text="GetSession1-25" />
        <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style2"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style3" OnClick="Button2_Click" Text="TermEE" />
        <asp:TextBox ID="TextBox2" runat="server" CssClass="auto-style4" TextMode="MultiLine"></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style5"></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style6" TextMode="MultiLine"></asp:TextBox>
    </form>
</body>
</html>
