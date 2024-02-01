<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Rapidsetup.aspx.cs" Inherits="AutoTermLog" %>

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
            top: 165px;
            left: 674px;
            z-index: 1;
            width: 109px;
            height: 31px;
        }
        .auto-style2 {
            position: absolute;
            top: 176px;
            left: 103px;
            z-index: 1;
            width: 373px;
        }
        .auto-style3 {
            position: absolute;
            top: 283px;
            left: 104px;
            z-index: 1;
            width: 531px;
            height: 116px;
        }
        .auto-style4 {
            position: absolute;
            top: 169px;
            left: 821px;
            z-index: 1;
            width: 118px;
            height: 26px;
        }
        .auto-style5 {
            position: absolute;
            top: 282px;
            left: 772px;
            z-index: 1;
            width: 126px;
            height: 34px;
        }
        .auto-style6 {
            position: absolute;
            top: 337px;
            left: 772px;
            z-index: 1;
            width: 131px;
            height: 32px;
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
      <a class="nav-link"  href="RapidPay.aspx">RapidPayLive</a>
    </li>  
   
      <li class="nav-item" style="background-color:#808080">
      <a class="nav-link"style="color:white" href="Rapidsetup.aspx">Rapid Setup</a>
    </li>  
     <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/RapidPay%20Report">Rapid Report</a>
    </li>  

     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>
    
    <form id="form1" runat="server">
 
       
       
       
       
        
 
       
       
        <asp:Button ID="Button1" runat="server" CssClass="auto-style1" OnClick="Button1_Click" Text="Test UAT" />
        <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style2"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server" CssClass="auto-style3" TextMode="MultiLine"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style4" OnClick="Button2_Click" Text="TestPRO" />
        <asp:Button ID="Button3" runat="server" CssClass="auto-style5" OnClick="Button3_Click" Text="UAT EmpSend" />
        <asp:Button ID="Button4" runat="server" CssClass="auto-style6" OnClick="Button4_Click" Text="PRO EmpSend" />
 
       
       
       
       
        
 
       
       
    </form>
    
</body>
</html>
