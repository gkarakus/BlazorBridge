<%@ Page Language="C#"  Debug ="true" AutoEventWireup  ="true" CodeFile="autoterm.aspx.cs" Inherits="autoterm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Autoterm read to execute</title>
   

     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">

    <style type="text/css">
        .auto-style3 {
            position: absolute;
            top: 103px;
            left: 1025px;
            z-index: 1;
            width: 239px;
            height: 18px;
        }
        .gridview-style4 {
            width: 1400px;
            height: 600px;
            position: absolute;
            overflow:scroll;
            top: 152px;
            left: 24px;
            z-index: 1;
            right: 32px;
                    }

        .griddiv {
               overflow: auto;
                height: 500px;
                 border: solid 1px orange;
                height: 480px;
                width: 1450px;
        }

        .auto-style5 {
            position: absolute;
            top: 120px;
            left: 747px;
            z-index: 1;
        }
        .auto-style6 {
            position: absolute;
            top: 119px;
            left: 30px;
            z-index: 1;
            width: 418px;
            height: 28px;
            right: 886px;
        }
       
        


    </style>

      


    </head>
<body>
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="autoterm.aspx">AutoTerm</a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="autotermsetup.aspx">AutoTerm Setup</a>
    </li>         
     <li class="nav-item">
      <a class="nav-link" href="autoTermLog.aspx">TermHistory</a>
    </li>  

     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>
    
    <form id="form1" runat="server">
    
        <div class="text-center">
    
     <center>  <h2> AutoTerm Activity</h2> </center>
        <div class="text-center">
            <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style6"></asp:TextBox>
             <asp:Label ID="Label1" runat="server" CssClass="auto-style3" Text="Label"></asp:Label>

        </div>

                      
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BridgeConnectionString %>" SelectCommand="SELECT [ClientID], [EmpStatus], [EE_ID], [EE_Sort_Name], [EE_Last_Hire_Date], [LastPaidPeriodEnd], [LastInvoiceDate], [DaysWithoutPay], [AutotermSet], [DaysLeftToAutoterm], [BenStatus], [PayRepID] FROM [AutoTerm]"></asp:SqlDataSource>
      
            <asp:GridView ID="GridView1" runat="server"  AllowSorting="True" AutoGenerateColumns="False" CellPadding="2" CssClass="gridview-style4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ClientID" HeaderText="ClientID" SortExpression="ClientID" />
                    <asp:BoundField DataField="EmpStatus" HeaderText="EmpStatus" SortExpression="EmpStatus" >
                    </asp:BoundField>
                    <asp:BoundField DataField="EE_ID" HeaderText="EE_ID" SortExpression="EE_ID" />
                    <asp:BoundField DataField="EE_Sort_Name" HeaderText="EE_Sort_Name" SortExpression="EE_Sort_Name" ItemStyle-Width="550"> 
                    </asp:BoundField>
                    <asp:BoundField DataField="EE_Last_Hire_Date" HeaderText="LastHireDate" SortExpression="EE_Last_Hire_Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="LastPaidPeriodEnd" HeaderText="LastPaidPerEnd" SortExpression="LastPaidPeriodEnd" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="LastInvoiceDate" HeaderText="LastInvoice" SortExpression="LastInvoiceDate" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="DaysWithoutPay" HeaderText="WithoutPay" SortExpression="DaysWithoutPay" />
                    <asp:BoundField DataField="AutotermSet" HeaderText="AutotermSet" SortExpression="AutotermSet" />
                    <asp:BoundField DataField="DaysLeftToAutoterm" HeaderText="DaysLeft" SortExpression="DaysLeftToAutoterm" />
                    <asp:BoundField DataField="BenStatus" HeaderText="Status" SortExpression="BenStatus" />
                    <asp:BoundField DataField="PayRepID" HeaderText="PayRepID" SortExpression="PayRepID" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
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
       
        <asp:Button ID="Button1" runat="server" CssClass="auto-style5" OnClick="Button1_Click" Text="RunAutoTerm" />
       
        <p>
            &nbsp;</p>
  
        <p>
            <asp:Button ID="Button2" runat="server" CssClass="auto-style7" Text="v.1.25" OnClick="Button2_Click" style="z-index: 1; position: absolute; top: 122px; left: 1275px; width: 64px;" />
         </p>
      

        



      

        


    </form>
    

   
   


</body>
</html>
