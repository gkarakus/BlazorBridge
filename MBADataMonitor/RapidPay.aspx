<%@ Page Language="C#" Debug ="true" AutoEventWireup="true" CodeFile="RapidPay.aspx.cs" Inherits="AutoTermLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bridge script test </title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
  
     <script type="text/javascript">
        document.getElementById("myBtn").addEventListener("click", function () {
         //   alert("Hello World!");
        });
</script>
    


    <style type="text/css">
      
        .auto-style1 {
            position: absolute;
            top: 181px;
            left: 808px;
            z-index: 1;
            width: 127px;
            margin-top: 0;
        }
      
        .auto-style2 {
            position: absolute;
            top: 266px;
            left: 35px;
            z-index: 1;
            width: 838px;
            height: 82px;
        }
        .auto-style3 {
            position: absolute;
            top: 259px;
            left: 895px;
            z-index: 1;
            width: 124px;
        }
        .auto-style4 {
            position: absolute;
            top: 218px;
            left: 35px;
            z-index: 1;
            width: 1253px;
        }
        .auto-style5 {
            position: absolute;
            top: 309px;
            left: 891px;
            z-index: 1;
            width: 127px;
        }
        .auto-style6 {
            position: absolute;
            top: 365px;
            left: 897px;
            z-index: 1;
            width: 123px;
            height: 37px;
        }
      
        .auto-style7 {
            position: absolute;
            top: 181px;
            left: 34px;
            z-index: 1;
            width: 108px;
        }
        .auto-style8 {
            position: absolute;
            top: 181px;
            left: 241px;
            z-index: 1;
            width: 126px;
            right: 1008px;
        }
      
        .auto-style9 {
            position: absolute;
            top: 133px;
            left: 334px;
            z-index: 1;
            width: 457px;
        }
        .auto-style10 {
            position: absolute;
            top: 412px;
            left: 57px;
            z-index: 1;
            width: 685px;
        }
      
        .auto-style11 {
            position: absolute;
            top: 264px;
            left: 1180px;
            z-index: 1;
            width: 120px;
            height: 34px;
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
      <a class="nav-link" style="color:white" href="RapidPay.aspx">RapidPayLive</a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="Rapidsetup.aspx">Rapid Setup</a>
    </li>         
     <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/RapidPay%20Report"  target="_blank" >Rapid Report</a>
    </li>  

     </ul>        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
     </ul>

</nav>
    
    <form id="form1" runat="server">     
        
 
       
       
        <div class="text-center">
            <div class="text-center"> 

       
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                <strong>
                <asp:Label ID="Label1" runat="server" CssClass="auto-style9" Text="Live RapidPay Card "></asp:Label>
                </strong>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style1" Enabled="False"></asp:TextBox>        
 
       
       
        <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="auto-style7" Enabled="False"></asp:TextBox>
        <asp:TextBox ID="TextBoxEmpID" runat="server" CssClass="auto-style8" Enabled="False"></asp:TextBox>
        <br />
        <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick">
        </asp:Timer>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style4" Enabled="False"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" CssClass="auto-style3" OnClick="Button1_Click" Text="Get EE" Enabled="False" />
        <br />     
       
                <asp:Button ID="ButtonDeleteError" runat="server" CssClass="auto-style11" OnClick="ButtonDeleteError_Click" Text="Delete error" />
       
        <asp:TextBox ID="TextBox2" runat="server" CssClass="auto-style2" TextMode="MultiLine" Enabled="False"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style5" OnClick="Button2_Click" Text="SoapCALL" Enabled="False" />
        <asp:Button ID="UpdateTb" runat="server" CssClass="auto-style6" Text="UpdateTB" OnClick="Button3_Click" Enabled="False" />   
       
            </div>
            <asp:Label ID="Label2" runat="server" CssClass="auto-style10" Text="This Page Update  every 30 second"></asp:Label>
        </div>
 
 
       
    </form>
    
</body>
</html>
