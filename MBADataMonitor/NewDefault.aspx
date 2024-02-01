<%@ Page Language="C#" debug="true" AutoEventWireup="true" CodeFile="NewDefault.aspx.cs" Inherits="NewDefault" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 266px;
            height: 19px;
            position: absolute;
            top: 14px;
            left: 982px;
            z-index: 1;
        }
        .auto-style2 {
            position: absolute;
            top: 41px;
            left: 48px;
            z-index: 1;
            width: 113px;
            height: 27px;
        }
        .auto-style3 {
            position: absolute;
            top: 39px;
            left: 216px;
            z-index: 1;
            width: 176px;
            height: 26px;
        }
    </style>
</head>
<body style="height: 133px">
    <form id="form1" runat="server">
        <div class="auto-style1">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
        <asp:Button ID="Btn1" runat="server" CssClass="auto-style2" Text="AutoTerm" />
        <asp:Button ID="Btn2" runat="server" CssClass="auto-style3" OnClick="Btn2_Click" Text="NewHire" />
    </form>
</body>
</html>
