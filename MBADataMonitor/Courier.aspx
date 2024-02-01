<%@ Page Language="C#" Debug ="true" AutoEventWireup="true" CodeFile="Courier.aspx.cs" Inherits="Courier" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Courier Manifest</title>
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
            position: absolute;
            top: 140px;
            left: 447px;
            z-index: 1;
            width: 81px;
        }
        .auto-style2 {
            position: absolute;
            top: 141px;
            left: 377px;
            z-index: 1;
            width: 65px;
            height: 24px;
        }
        .auto-style3 {
            position: absolute;
            top: 141px;
            left: 569px;
            z-index: 1;
            width: 108px;
        }
        .auto-style4 {
            position: absolute;
            top: 140px;
            left: 682px;
            z-index: 1;
            width: 432px;
        }
        .auto-style5 {
            position: absolute;
            top: 199px;
            left: 165px;
            z-index: 1;
            width: 169px;
        }
        .auto-style6 {
            position: absolute;
            top: 203px;
            left: 126px;
            z-index: 1;
            width: 35px;
        }
        .auto-style7 {
            position: absolute;
            top: 203px;
            left: 962px;
            z-index: 1;
            width: 47px;
            height: 24px;
        }
        .auto-style8 {
            position: absolute;
            top: 199px;
            left: 1021px;
            z-index: 1;
            width: 89px;
        }
        .auto-style9 {
            position: absolute;
            top: 262px;
            left: 251px;
            z-index: 1;
            width: 919px;
        }
        .auto-style10 {
            position: absolute;
            top: 264px;
            left: 196px;
            z-index: 1;
            width: 49px;
        }
        .auto-style11 {
            position: absolute;
            top: 203px;
            left: 682px;
            z-index: 1;
            width: 32px;
            right: 809px;
        }
        .auto-style12 {
            position: absolute;
            top: 199px;
            z-index: 1;
            width: 86px;
            left: 719px;
            right: 710px;
        }
        .auto-style14 {
            position: absolute;
            top: 337px;
            left: 1017px;
            z-index: 1;
            width: 165px;
            margin-bottom: 9;
            height: 30px;
        }
        .auto-style15 {
            position: absolute;
            top: 140px;
            left: 163px;
            z-index: 1;
            width: 102px;
            height: 20px;
        }
        .auto-style16 {
            text-align: center;
            height: 768px;
        }
        .auto-style17 {
            position: absolute;
            top: 229px;
            left: 22px;
            z-index: 1;
            width: 46px;
        }
        .auto-style18 {
            position: absolute;
            top: 338px;
            left: 859px;
            z-index: 1;
            width: 137px;
            margin-top: 0;
            height: 30px;
        }
        .auto-style19 {
            position: absolute;
            top: 146px;
            left: 291px;
            z-index: 1;
            margin-top: 0px;
        }
        .auto-style20 {
            position: absolute;
            top: 203px;
            left: 386px;
            z-index: 1;
            width: 64px;
            right: 1073px;
        }
        .auto-style21 {
            position: absolute;
            top: 199px;
            left: 455px;
            z-index: 1;
            right: 920px;
        }
        .auto-style22 {
            position: absolute;
            top: 339px;
            left: 122px;
            z-index: 1;
            width: 254px;
            height: 26px;
        }
        .auto-style23 {
            position: absolute;
            top: 340px;
            left: 401px;
            z-index: 1;
            width: 101px;
            height: 27px;
        }
        .auto-style24 {
            width: 1103px;
            height: 396px;
            position: absolute;
            top: 388px;
            left: 118px;
            z-index: 1;
            right: 302px;
        }
    </style>




    </head>
<body>
     <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item" >
      <a class="nav-link" style="color:white" href="Courier.aspx"> Courier DataEntry</a>
    </li> 
         <li class="nav-item" >
      <a class="nav-link" style="color:white" target="_blank" href="http://tpaa-pdb-sql01/reports_mbasql/report/Bridge/CourierReport"> Courier Report</a>
    </li> 



     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>

    <form id="form1" runat="server">
        <div class="auto-style16">
        <div>
             
       <h3 class="text-center"> <strong>Courier Manifest</strong></h3> 
     
        </div>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style1" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>




        <strong>
        <asp:Label ID="Label1" runat="server" CssClass="auto-style2" Text="ClientID"></asp:Label>
        <asp:Label ID="Label2" runat="server" CssClass="auto-style3" Text="Client Name"></asp:Label>
        </strong>
        <asp:TextBox ID="TextBox2" runat="server" CssClass="auto-style4"></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style5"></asp:TextBox>
        <strong>
        <asp:Label ID="Label3" runat="server" CssClass="auto-style6" Text="City"></asp:Label>
        <asp:Label ID="Label4" runat="server" CssClass="auto-style7" Text="COD"></asp:Label>
        </strong>
        <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style8"></asp:TextBox>
        <asp:TextBox ID="TextBox5" runat="server" CssClass="auto-style9"></asp:TextBox>
        <strong>
        <asp:Label ID="Label5" runat="server" CssClass="auto-style10" Text="Note"></asp:Label>
        <asp:Label ID="Label6" runat="server" CssClass="auto-style11" Text="Zip"></asp:Label>
        </strong>
        <asp:TextBox ID="TextBox6" runat="server" CssClass="auto-style12"></asp:TextBox>




            <asp:DropDownList ID="DropDownList1"    runat="server" CssClass="auto-style15" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Button ID="Button1" runat="server" CssClass="auto-style14" OnClick="Button1_Click" Text="Save / Update" />
            <asp:Button ID="Button2" runat="server" CssClass="auto-style18" OnClick="Button2_Click" Text="Clear / Cancel" />
            <asp:TextBox ID="TextBoxDurum" runat="server" CssClass="auto-style17" Visible="False"></asp:TextBox>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BridgeConnectionString %>" SelectCommand="SELECT * FROM [Courier] ORDER BY [RDate] DESC">
            </asp:SqlDataSource>

            <asp:GridView ID="GridView1" runat="server" CssClass="auto-style24" AllowPaging="True" AllowSorting="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged1" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerSettings Position="TopAndBottom" />
                <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="50" >  
                <ItemStyle Width="50px"></ItemStyle>  </asp:BoundField>
        <asp:BoundField DataField="ClientID" HeaderText="ClientId" ItemStyle-Width="50" >
<ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
        <asp:BoundField DataField="ClientName" HeaderText="Name" ItemStyle-Width="350" >
<ItemStyle Width="350px"></ItemStyle>
                    </asp:BoundField>
        <asp:BoundField DataField="Division" HeaderText="division" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
        <asp:BoundField DataField="City" HeaderText="City" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
         <asp:BoundField DataField="Zip" HeaderText="Zip" ItemStyle-Width="100" >
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
         <asp:BoundField DataField="Cod" HeaderText="Cod" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
         <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
         <asp:BoundField DataField="Rdate" HeaderText="date" ItemStyle-Width="150" >



<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>



    </Columns>
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
            
            
            <asp:Button ID="Button3" runat="server" CssClass="auto-style23" OnClick="Button3_Click" Text="search" />
            <asp:TextBox ID="TextBox8" runat="server" CssClass="auto-style22" OnTextChanged="TextBox8_TextChanged"></asp:TextBox>
            <asp:TextBox ID="TextBox7" runat="server" CssClass="auto-style21"></asp:TextBox>
            <strong>
            <asp:Label ID="Label8" runat="server" CssClass="auto-style20" Text="Division"></asp:Label>
            </strong>
            <asp:Label ID="Label7" runat="server" CssClass="auto-style19" Font-Size="Smaller" Text="Select or type"></asp:Label>
        </div>




    </form>

   


</body>
</html>
