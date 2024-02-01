<%@ Page Language="C#" AutoEventWireup="true" CodeFile="autoTermLog.aspx.cs" Inherits="AutoTermLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AutoTermHistory</title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">



    <style type="text/css">
        .auto-style1 {
            width: 1437px;
            height: 163px;
            position: absolute;
            top: 139px;
            left: 55px;
            z-index: 1;
        }
    </style>



</head>
<body>
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item">
      <a class="nav-link" href="autoterm.aspx">AutoTerm</a>
    </li>   

     <li class="nav-item">
      <a class="nav-link" href="autotermsetup.aspx">AutoTerm Setup</a>
    </li>   
         

    <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="autoTermLog.aspx">TermHistory</a>
    </li>      

  </ul>        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>
</nav>
     <center>  <h2> AutoTerm History </h2> </center>

    <form id="form1" runat="server">
        <div class="text-left">
        <div>
        </div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BridgeConnectionString %>" SelectCommand="SELECT [ClientID], [EE_ID], [EE_Sort_Name], [LastPaidInvoiceDate], [DaysWithoutPay], [TermDate], [ErrorCode], [ErrorType], [ErrorMsg], [ProcessDate], [PayRep] FROM [AutoTermHistory]"></asp:SqlDataSource>
            <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" CssClass="auto-style1" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ClientID" HeaderText="ClientID" SortExpression="ClientID" />
                    <asp:BoundField DataField="EE_ID" HeaderText="EE_ID" SortExpression="EE_ID" />
                    <asp:BoundField DataField="EE_Sort_Name" HeaderText="EE_Sort_Name" SortExpression="EE_Sort_Name" />
                    <asp:BoundField DataField="LastPaidInvoiceDate" HeaderText="LastPaidInvoiceDate" SortExpression="LastPaidInvoiceDate" DataFormatString="{0:d}">
                    </asp:BoundField>
                    <asp:BoundField DataField="DaysWithoutPay" HeaderText="DaysWithoutPay" SortExpression="DaysWithoutPay" />
                    <asp:BoundField DataField="TermDate" HeaderText="TermDate" SortExpression="TermDate" DataFormatString="{0:d}">
                    </asp:BoundField>
                    <asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode" SortExpression="ErrorCode" />
                    <asp:BoundField DataField="ProcessDate" HeaderText="ProcessDate" SortExpression="ProcessDate" DataFormatString="{0:d}">
                   
                    </asp:BoundField>
                    <asp:BoundField DataField="PayRep" HeaderText="PayRep" SortExpression="PayRep" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
