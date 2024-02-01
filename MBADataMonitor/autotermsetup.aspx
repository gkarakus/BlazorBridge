<%@ Page Language="C#" AutoEventWireup="true" CodeFile="autotermsetup.aspx.cs" Inherits="autoterm" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Autoterm Setup</title>
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
            height: 399px;
            margin-bottom: 123px;
        }
        .auto-style2 {
            width: 1052px;
            height: 264px;
            position: absolute;
            top: 161px;
            left: 125px;
            z-index: 1;
        }
        .auto-style3 {
            position: absolute;
            top: 132px;
            left: 982px;
            z-index: 1;
            width: 129px;
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
   
 <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="autotermsetup.aspx">AutoTerm Setup</a>
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

    <center>  <h2> Auto Term Setup</h2> </center>

    <form id="form1" runat="server">
        <div class="auto-style1" aria-dropeffect="none">
          
            <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
                AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#DEDFDE" BorderStyle="None" 
                BorderWidth="1px" CellPadding="4" 
                CssClass="auto-style2" DataKeyNames="ClientID" 
                DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" ShowFooter="True">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ClientID" HeaderText="ClientID" ReadOnly="True" SortExpression="ClientID"  />                  
                    <asp:BoundField DataField="ClientName" HeaderText="ClientName" SortExpression="ClientName"  >
                    <HeaderStyle Width="350px" />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="AutoTermYesNo" HeaderText="AutoTermYesNo" SortExpression="AutoTermYesNo"  ItemStyle-HorizontalAlign="Center"   >
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CheckBoxField>
                    <asp:BoundField DataField="AutoTermDays" HeaderText="AutoTermDays" SortExpression="AutoTermDays" />
                    <asp:BoundField DataField="UpdateBy" HeaderText="UpdateBy" SortExpression="UpdateBy" ReadOnly ="True" />
                    <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" SortExpression="UpdateDate" DataFormatString = "{0:d}" ReadOnly ="True"  />
                    <asp:CommandField ButtonType="Link" ShowEditButton="true" 
                EditText="<i class='far fa-edit'></i>" 
                CancelText="<i aria-hidden='true'><button type='button' class='btn btn-outline-primary btn-sm'>Cancel </button> </i>" 
                UpdateText="<i aria-hidden='true'> <button type='button' class='btn btn-outline-secondary btn-sm'>Save </button> </i>" >

                    <HeaderStyle Width="75px" />
                    </asp:CommandField>

                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BridgeConnectionString %>" 
                DeleteCommand="DELETE FROM [AutoTermSet] WHERE [ClientID] = @ClientID" 
                InsertCommand="INSERT INTO [AutoTermSet] ([ClientID], [ClientIDz], [ClientName], [AutoTerm], [AutoTermYesNo], [AutoTermDays], [UpdateBy], [UpdateDate]) VALUES (@ClientID, @ClientIDz, @ClientName, @AutoTerm, @AutoTermYesNo, @AutoTermDays, @UpdateBy, @UpdateDate)" 
                SelectCommand="SELECT * FROM [AutoTermSet]" 
                UpdateCommand="UPDATE [AutoTermSet] SET [ClientName] = @ClientName, [AutoTermYesNo] = @AutoTermYesNo, [AutoTermDays] = @AutoTermDays, [UpdateBy] = @UpdateBy, [UpdateDate] = GETDATE() WHERE [ClientID] = @ClientID">
                <DeleteParameters>
                    <asp:Parameter Name="ClientID" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ClientID" Type="String" />
                    <asp:Parameter Name="ClientIDz" Type="String" />
                    <asp:Parameter Name="ClientName" Type="String" />
                    <asp:Parameter Name="AutoTerm" Type="String" />
                    <asp:Parameter Name="AutoTermYesNo" Type="Boolean" />
                    <asp:Parameter Name="AutoTermDays" Type="Int32" />
                    <asp:Parameter Name="UpdateBy" Type="String" />
                    <asp:Parameter DbType="Date" Name="UpdateDate"  />
                   
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ClientIDz" Type="String" />
                    <asp:Parameter Name="ClientName" Type="String" />
                    <asp:Parameter Name="AutoTerm" Type="String" />
                    <asp:Parameter Name="AutoTermYesNo" Type="Boolean" />
                    <asp:Parameter Name="AutoTermDays" Type="Int32" />
                    <asp:Parameter Name="UpdateBy" Type="String"   />
                    <asp:Parameter DbType="Date" Name="UpdateDate" />
                    <asp:Parameter Name="ClientID" Type="String" />
                    
                </UpdateParameters>
            </asp:SqlDataSource>
            
            <asp:Button ID="Button1" runat="server" CssClass="auto-style3" OnClick="Button1_Click" Text="Refresh DB" />
            
        </div>
    </form>
</body>
</html>
